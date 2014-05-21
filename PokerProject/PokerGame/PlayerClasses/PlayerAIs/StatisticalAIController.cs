using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.PlayerClasses.Decisions;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    class StatisticalAIController : AbstractAIController
    {
        private Table table;
        private static Dictionary<StarterHand, double> startingValues;
        private Dictionary<PokerHand.HandCategory, int> probableCategories;
        private Dictionary<PokerHand, int> probablePokerHands;
        private HandStrength strength;
        private GamePhase currentPhase;
        private GamePhase lastPhase;

        protected enum HandStrength
        {
            Neutral,
            VeryGood,
            Good,
            LikelyDraw,
            Bad,
            VeryBad,
            Undecidable
        }

        public enum GamePhase
        {
            PreFlop,
            Flop,
            Turn,
            River
        }

        static StatisticalAIController()
        {
            startingValues = new Dictionary<StarterHand, double>();
            CardList allCards = GetHalfDeckWithoutGivenCards(new CardList());
            HashSet<StarterHand> allStarters = new HashSet<StarterHand>();

            CombinatoricsUtilities.GetCombinations<PokerCard>(allCards, startingCards =>
            {
                CardList startingHand = new CardList(startingCards);
                startingHand.Sort();
                allStarters.Add(new StarterHand(startingHand));
            }
            , 2, false);

            foreach (StarterHand hand in allStarters)
            {
                double value = ApproximatedPlayabilityAgainstCategory(hand);
                startingValues.Add(hand, value);
            }
        }

        public StatisticalAIController()
        {
            table = Table.Instance;
            probableCategories = new Dictionary<PokerHand.HandCategory, int>();
            probablePokerHands = new Dictionary<PokerHand, int>();
        }

        protected override void MakeAIDecision()
        {
            bool everybodyInactive = true;
            foreach (Player otherPlayer in table.Players.GetPlayersList())
            {
                if (everybodyInactive == true && otherPlayer.IsIngame() == true && otherPlayer.ChipCount > 0)
                {
                    everybodyInactive = false;
                }
            }

            if (everybodyInactive && table.MainPot.GetAmountToCall(player) == 0)
            {
                decision = new CallDecision(player);
            }
            else if (table.CommunityCards.Count == 0)
            {
                currentPhase = GamePhase.PreFlop;
                CardList myCards = player.ShowCards();
                myCards.Sort();

                double hcPlayability = startingValues[new StarterHand(myCards)];
                AppendInfo("hc pbility: " + hcPlayability);
                double better = 0;
                foreach (var pair in startingValues)
                {
                    if (pair.Value <= hcPlayability)
                    {
                        better += 1;
                    }
                }
                double ratio = better / startingValues.Count;
                AppendInfo("better/all: " + better + "/" + startingValues.Count);
                AppendInfo("ratio: " + ratio);

                if (ratio > 0.66)
                {
                    if (table.MainPot.GetAmountToCall(player) <= player.ChipCount/3)
                    {
                        decision = new CallDecision(player);
                        AppendInfo("hand is good so i call");
                    }
                    else
                    {
                        decision = new FoldDecision(player);
                        AppendInfo("have to call too much, so I fold");
                    }
                }
                else if (ratio > 0.20)
                {
                    if (table.MainPot.GetAmountToCall(player) <= player.ChipCount / 10)
                    {
                        decision = new CallDecision(player);
                        AppendInfo("hand is decent so i call");
                    }
                    else
                    {
                        decision = new FoldDecision(player);
                        AppendInfo("hand not good enough to call, so I fold");
                    }
                }
                else
                {
                    if (table.MainPot.GetAmountToCall(player) <= player.ChipCount / 20)
                    {
                        decision = new CallDecision(player);
                        AppendInfo("nobody raised so i call");
                    }
                    else
                    {
                        decision = new FoldDecision(player);
                        AppendInfo("hand is bad, I fold");
                    }
                }
            }
            else
            {
                if (table.CommunityCards.Count == 3)//Flop
                {
                    currentPhase = GamePhase.Flop;
                    //determine what are the best hands i can get
                    CardList handSoFar = MakeHandSoFar();
                    CalculateMyCards(handSoFar, 2);

                    //opponent hands: 178365(47 choose 4),16215(47 choose 3) 1081(47 choose 2)
                    CalculateWinningChances(handSoFar, 3);

                    DecisionByCardStrength();
                }
                else if (table.CommunityCards.Count == 4)//River
                {
                    currentPhase = GamePhase.River;
                    //determine what are the best hands i can get
                    CardList handSoFar = MakeHandSoFar();
                    CalculateMyCards(handSoFar, 1);

                    //opponent hands: 15180(46 choose 3) 
                    CalculateWinningChances(handSoFar, 3);

                    DecisionByCardStrength();
                }
                else//Turn
                {
                    currentPhase = GamePhase.Turn;
                    //determine what are the best hands i can get
                    CardList handSoFar = MakeHandSoFar();
                    CalculateMyCards(handSoFar, 0);

                    //opponent hands: 990(45 choose 2) 
                    CalculateWinningChances(handSoFar, 2);

                    DecisionByCardStrength();
                }
            }

            lastPhase = currentPhase;
        }

        private CardList MakeHandSoFar()
        {
            CardList handSoFar = player.ShowCards();
            handSoFar.AddRange(table.CommunityCards);
            return handSoFar;
        }

        //returns the playability value of a given starter hand
        private static double ApproximatedPlayabilityAgainstCategory(StarterHand myHand)
        {
            //total: 24 choose 3
            double totalCombinations = 2024;
            double playablityCount = 0;
            CardList cardDeck = GetHalfDeckWithoutGivenCards(myHand.GetExampleCards());

            CombinatoricsUtilities.GetCombinations<PokerCard>(cardDeck, imaginaryCards =>
            {
                CardList imaginary5Card = new CardList();
                imaginary5Card.AddRange(imaginaryCards);
                imaginary5Card.AddRange(myHand.GetExampleCards());

                PokerHand imaginaryHand = new PokerHand(imaginary5Card);

                if (imaginaryHand.Category > PokerHand.HandCategory.HighCard)
                {
                    playablityCount += 1;
                }

            }, 3, false);

            return playablityCount * 10 / totalCombinations;
        }

        //Fills probableCategories with probabilities about given hand with given number of missing cards 
        private void CalculateMyCards(CardList handSoFar, int missingCards)
        {
            if (lastPhase == currentPhase)
            {
                return;
            }
            probableCategories.Clear();
            probablePokerHands.Clear();
            HandEvaluator evaluator = new HandEvaluator();

            if (missingCards > 0)
            {
                CardList otherCards = GetFullDeckWithoutGivenCards(handSoFar);
                int total = 0;
                CombinatoricsUtilities.GetCombinations<PokerCard>(otherCards, imaginaryCards =>
                {
                    CardList imaginary7Card = new CardList(handSoFar);
                    imaginary7Card.AddRange(imaginaryCards);
                    evaluator.DetermineBestHand(imaginary7Card);
                    PokerHand imaginaryHand = evaluator.GetBestHand();

                    AddToProbablePokerHands(imaginaryHand);
                    AddToProbableCategories(imaginaryHand.Category);
                    total++;

                }, missingCards, false);

                foreach (PokerHand.HandCategory category in probableCategories.Keys)
                {
                    double percent = ((double)probableCategories[category]) * 100 / total;
                    percent = Math.Round(percent, 1);
                    AppendInfo(String.Format("{0} have: {1} ({2}%)", category, probableCategories[category], percent));
                } 
            }
            else
            {
                evaluator.DetermineBestHand(handSoFar);
                PokerHand myHand = evaluator.GetBestHand();

                AddToProbablePokerHands(myHand);
                AddToProbableCategories(myHand.Category);

                AppendInfo("I have: " + myHand.Category);
            }
        }

        private void AddToProbablePokerHands(PokerHand hand)
        {
            if (probablePokerHands.ContainsKey(hand))
            {
                probablePokerHands[hand] += 1;
            }
            else
            {
                probablePokerHands.Add(hand, 1);
            }
        }

        private void AddToProbableCategories(PokerHand.HandCategory category)
        {
            if (probableCategories.ContainsKey(category))
            {
                probableCategories[category] += 1;
            }
            else
            {
                probableCategories.Add(category, 1);
            }
        }

        //Gives the win/total with the given hand against somebody who gets 'enemyMissingCards' number of cards 
        private void CalculateWinningChances(CardList handSoFar, int enemyMissingCards)
        {
            if (lastPhase == currentPhase)
            {
                return;
            }

            HandEvaluator evaluator = new HandEvaluator();
            double wins = 0;
            double ties = 0;
            double losses = 0;
            CardList otherCards = GetFullDeckWithoutGivenCards(handSoFar);

            CombinatoricsUtilities.GetCombinations<PokerCard>(otherCards, imaginaryCards =>
            {
                CardList imaginary7Card = new CardList(table.CommunityCards);
                imaginary7Card.AddRange(imaginaryCards);
                evaluator.DetermineBestHand(imaginary7Card);

                PokerHand imaginaryHand = evaluator.GetBestHand();

                int beatHand = 0;
                int looseToHand = 0;
                int tieWithHand = 0;

                foreach (PokerHand hand in probablePokerHands.Keys)
                {
                    if (hand > imaginaryHand)
                    {
                        beatHand += probablePokerHands[hand];
                    }
                    else if (hand == imaginaryHand)
                    {
                        tieWithHand += probablePokerHands[hand];
                    }
                    else
                    {
                        looseToHand += probablePokerHands[hand];
                    }
                }

                if (beatHand > looseToHand && beatHand > tieWithHand)
                {
                    wins += 1;
                }
                else if (tieWithHand > looseToHand && tieWithHand > beatHand)
                {
                    ties += 1;
                }
                else
                {
                    losses += 1;
                }
            }, enemyMissingCards, false);

            AppendInfo("win/tie/loss: " + wins +"/" + ties + "/" + losses);
            strength = EvaluteHandStrength(wins, ties, losses);

            return;
        }

        private void DecisionByCardStrength()
        {
            AppendInfo("Hand Strength: " + strength);
            //Ideal
            if (table.MainPot.GetAmountToCall(player) == 0)
            {
                decision = new CallDecision(player);
                AppendInfo("I call");
            }
            else if (strength== HandStrength.VeryGood)
            {
                //make random bet
                Random randomGenerator = new Random();

                int minBet = Table.Instance.MainPot.AmountToBeEligibleForPot + Table.Instance.MainPot.LargestBet;
                int atMax = player.ChipCount;
                int atLeast = (player.ChipCount < minBet) ? player.ChipCount : minBet;

                int randBet = randomGenerator.Next(atLeast, atMax);
                randBet = (int)Math.Round((randBet/100.0))*100;
                if (randBet < atLeast)
                {
                    randBet = atLeast;
                }
                if (randBet > atMax)
                {
                    randBet = atMax;
                }

                decision = new BetDecision(player, randBet);
                AppendInfo("I bet " + randBet);
            }
            else if ((strength == HandStrength.Bad || strength == HandStrength.VeryBad || strength == HandStrength.Undecidable) 
                && table.MainPot.GetAmountToCall(player) > player.ChipCount / 10)
            {
                decision = new FoldDecision(player);
                AppendInfo("I fold");
            }
            else
            {
                decision = new CallDecision(player);
                AppendInfo("I call");
            }
        }

        private HandStrength EvaluteHandStrength(double wins, double ties, double losses)
        {
            double total = wins + ties + losses;
            if ((wins + ties) > total * 0.70 )
            {
                if (wins > (ties + losses))
                {
                    return HandStrength.VeryGood;
                }
                else if ((ties + losses) > total * 0.70)
                {
                    return HandStrength.LikelyDraw;
                }
                else
                {
                    return HandStrength.Good;
                }
            }
            if ((ties + losses) > total * 0.70)
            {
                if (losses > (ties + wins))
                {
                    return HandStrength.VeryBad;
                }
                else if ((ties + wins) > total * 0.70)
                {
                    return HandStrength.LikelyDraw;
                }
                else
                {
                    return HandStrength.Bad;
                }
            }
            if (ties < total * 0.3)
            {
                return HandStrength.Undecidable;
            }
            return HandStrength.Neutral;
        }

        private static CardList GetHalfDeckWithoutGivenCards(CardList myCards)
        {
            CardList cardDeck = GetFullDeckWithoutGivenCards(myCards);

            CardList copy = new CardList(cardDeck);
            foreach (PokerCard card in copy)
            {
                if (card.Suite != CardSuite.Diamonds && card.Suite != CardSuite.Clubs)
                {
                    cardDeck.Remove(card);
                }
            }

            return cardDeck;
        }

        private static CardList GetFullDeckWithoutGivenCards(CardList myCards)
        {
            CardList cardDeck = new CardList();
            foreach (CardSuite suiteIndex in (CardSuite[])Enum.GetValues(typeof(CardSuite)))
            {
                foreach (CardRank rankIndex in (CardRank[])Enum.GetValues(typeof(CardRank)))
                {
                    cardDeck.Add(new PokerCard(rankIndex, suiteIndex));
                }
            }

            foreach (PokerCard card in myCards)
            {
                cardDeck.Remove(card);
            }

            return cardDeck;
        }

        protected override void MakeRevealCardAIDecision()
        {
            decision = new ShowCardsDecision(player);
            AppendInfo("I show my cards");
        }

        public override string ToString()
        {
            return "Statistical AI";
        }

        public override PlayerController Clone()
        {
            return new StatisticalAIController();
        }
    }
}
