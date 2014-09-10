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
        
        static StatisticalAIController()
        {
            startingValues = new Dictionary<StarterHand, double>();

            using (System.IO.FileStream file = new System.IO.FileStream(@"AIfiles/statisticalStartingHands.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                startingValues = (Dictionary<StarterHand, double>)formatter.Deserialize(file);
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
            else if (table.CurrentGamePhase == GamePhase.PreFlop)
            {
                MakePreFlopDecision();
            }
            else
            {
                if (table.CurrentGamePhase == GamePhase.Flop)
                {
                    //opponent hands: 178365(47 choose 4),16215(47 choose 3) 1081(47 choose 2)
                    DetermineChancesAndMakeDecision(2, 3);
                }
                else if (table.CurrentGamePhase == GamePhase.River)
                {
                    //opponent hands: 15180(46 choose 3) 
                    DetermineChancesAndMakeDecision(1, 3);
                }
                else//Turn
                {
                    //opponent hands: 990(45 choose 2) 
                    DetermineChancesAndMakeDecision(0, 2);
                }
            }
            lastPhase = table.CurrentGamePhase;
        }

        #region PreFlop Decision
        /// <summary>
        /// Make a decison based on the starting values dictionary
        /// </summary>
        private void MakePreFlopDecision()
        {
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
                if (table.MainPot.GetAmountToCall(player) <= player.ChipCount / 3)
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
        #endregion

        /// <summary>
        /// Determine what are the best hands i can get.
        /// Determine opponent hands.
        /// Make decision based on theese hands.
        /// </summary>
        /// <param name="missingCards"></param>
        /// <param name="enemyMissingCards"></param>
        private void DetermineChancesAndMakeDecision(int missingCards, int enemyMissingCards)
        {
            CardList handSoFar = MakeHandSoFar();

            CalculateMyCards(handSoFar, missingCards);
            CalculateWinningChances(handSoFar, enemyMissingCards);

            DecisionByCardStrength();
        }

        private CardList MakeHandSoFar()
        {
            CardList handSoFar = player.ShowCards();
            handSoFar.AddRange(table.CommunityCards);
            return handSoFar;
        }

        # region My Cards
        /// <summary>
        /// Fills probableCategories with probabilities about given hand with given number of missing cards 
        /// </summary>
        private void CalculateMyCards(CardList handSoFar, int missingCards)
        {
            if (lastPhase == table.CurrentGamePhase)
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
        #endregion

        #region Enemy Cards
        /// <summary>
        /// Gives the win/total with the given hand against somebody who gets 'enemyMissingCards' number of cards 
        /// </summary>
        /// <param name="handSoFar"></param>
        /// <param name="enemyMissingCards"></param>
        private void CalculateWinningChances(CardList handSoFar, int enemyMissingCards)
        {
            if (lastPhase == table.CurrentGamePhase)
            {
                return;
            }

            HandEvaluator evaluator = new HandEvaluator();
            double wins = 0;
            double ties = 0;
            double losses = 0;
            //CardList otherCards = GetFullDeckWithoutGivenCards(handSoFar);
            PokerCard[] otherCards = GetFullDeckWithoutGivenCards(handSoFar).ToArray();
            
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

        /// <summary>
        /// Gives back a full deck without the given cards
        /// </summary>
        /// <param name="myCards">Cards to be left out from the deck</param>
        /// <returns></returns>
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

        private HandStrength EvaluteHandStrength(double wins, double ties, double losses)
        {
            double total = wins + ties + losses;
            if ((wins + ties) > total * 0.70)
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
        #endregion

        private void DecisionByCardStrength()
        {
            AppendInfo("Hand Strength: " + strength);
            //Defensive playstyle
            if (table.MainPot.GetAmountToCall(player) == 0)
            {
                decision = new CallDecision(player);
                AppendInfo("I call");
            }
            else if (strength == HandStrength.VeryGood)
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
