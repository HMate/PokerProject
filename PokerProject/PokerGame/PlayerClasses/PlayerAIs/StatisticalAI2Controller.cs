using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerProject.PokerGame.PlayerClasses.Decisions;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame.PlayerClasses.PlayerAIs
{
    class StatisticalAI2Controller : AbstractAIController
    {
        protected Table table;
        protected static Dictionary<StarterHand, double> startingValues;
        protected Dictionary<PokerHand.HandCategory, int> probableCategories;
        protected Dictionary<PokerHand, int> probablePokerHands;
        protected HandStrength handStrength;
        protected GamePhase lastPhase;

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
        
        static StatisticalAI2Controller()
        {
            startingValues = new Dictionary<StarterHand, double>();

            using (System.IO.FileStream file = new System.IO.FileStream(@"AIfiles/statisticalStartingHands.txt", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                startingValues = (Dictionary<StarterHand, double>)formatter.Deserialize(file);
            }
        }

        public StatisticalAI2Controller()
        {
            table = Table.Instance;
            probableCategories = new Dictionary<PokerHand.HandCategory, int>();
            probablePokerHands = new Dictionary<PokerHand, int>();
        }

        /// <summary>
        /// Makes a decision about what to do in the current turn, based on statistical calculations
        /// </summary>
        protected override void MakeAIDecision()
        {
            bool everybodyInactive = true;
            foreach (Player otherPlayer in table.Players.GetPlayersList())
            {
                if (otherPlayer != player && otherPlayer.IsIngame() == true && otherPlayer.ChipCount > 0)
                {
                    everybodyInactive = false;
                    break;
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
                    DetermineChancesAndMakeDecision(2);
                }
                else if (table.CurrentGamePhase == GamePhase.Turn)
                {
                    //opponent hands: 15180(46 choose 3) 
                    DetermineChancesAndMakeDecision(1);
                }
                else//River
                {
                    //opponent hands: 990(45 choose 2) 
                    DetermineChancesAndMakeDecision(0);
                }
            }
            lastPhase = table.CurrentGamePhase;
        }

        #region PreFlop Decision
        /// <summary>
        /// Make a decison based on the starting values dictionary
        /// </summary>
        protected void MakePreFlopDecision()
        {
            double ratio = ComputePreFlopPlayabilityRatio();

            DecisionByRatio(ratio);
        }

        /// <summary>
        /// Counts, how many starting hands are there, that has worse playability, than our hand.
        /// </summary>
        /// <returns>Percent of hands, which our hand beats</returns>
        protected virtual double ComputePreFlopPlayabilityRatio()
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

            return ratio;
        }

        /// <summary>
        /// Decides what to do preFlop, based on the playabilityRatio
        /// Plays defensively
        /// </summary>
        /// <param name="ratio"></param>
        protected virtual void DecisionByRatio(double ratio)
        {
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
            else if (ratio > 0.33)
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
        protected void DetermineChancesAndMakeDecision(int missingCards)
        {
            CalculateChances(missingCards);

            DecisionByCardStrength();
        }
        
        # region My Cards
        private struct WinsTiesLosses{
            public double wins;
            public double ties;
            public double losses;

            public WinsTiesLosses(double w, double t, double l)
            {
                wins = w;
                ties = t;
                losses = l;
            }
        }

        protected void CalculateChances(int missingCards)
        {
            if (lastPhase == table.CurrentGamePhase)
            {
                return;
            }

            CardList knownCards = GetKnownCards();
            HandEvaluator evaluator = new HandEvaluator();
            WinsTiesLosses wtl = new WinsTiesLosses(0, 0, 0);

            if (missingCards > 0)
            {
                CardList otherCards = GetFullDeckWithoutGivenCards(knownCards);
                int total = 0;
                CombinatoricsUtilities.GetCombinations<PokerCard>(otherCards, imaginaryCards =>
                {
                    CardList imaginary7Card = new CardList(knownCards);
                    imaginary7Card.AddRange(imaginaryCards);
                    evaluator.DetermineBestHand(imaginary7Card);
                    PokerHand imaginaryHand = evaluator.GetBestHand();

                    AddToProbablePokerHands(imaginaryHand);
                    AddToProbableCategories(imaginaryHand.Category);
                    total++;

                    CardList communityCards = new CardList(table.CommunityCards);
                    communityCards.AddRange(imaginaryCards);
                    wtl = DetermineWinsTiesLosses(wtl, imaginary7Card, imaginaryHand, communityCards);
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
                evaluator.DetermineBestHand(knownCards);
                PokerHand myHand = evaluator.GetBestHand();

                AddToProbablePokerHands(myHand);
                AddToProbableCategories(myHand.Category);

                AppendInfo("I have: " + myHand.Category);

                wtl = DetermineWinsTiesLosses(wtl, knownCards, myHand, table.CommunityCards);
            }

            AppendInfo("win/tie/loss: " + wtl.wins + "/" + wtl.ties + "/" + wtl.losses);
            handStrength = EvaluteHandStrength(wtl.wins, wtl.ties, wtl.losses);

            return;
        }

        protected CardList GetKnownCards()
        {
            CardList handSoFar = player.ShowCards();
            handSoFar.AddRange(table.CommunityCards);
            return handSoFar;
        }
        
        protected void AddToProbablePokerHands(PokerHand hand)
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

        protected void AddToProbableCategories(PokerHand.HandCategory category)
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

        private WinsTiesLosses DetermineWinsTiesLosses(WinsTiesLosses wtl, CardList knownCards, PokerHand myHand, CardList communityCards)
        {
            HandEvaluator evaluator = new HandEvaluator();
            int beatHand = 0;
            int looseToHand = 0;
            int tieWithHand = 0;
            CardList possibleOpponentCards = GetFullDeckWithoutGivenCards(knownCards);
            CombinatoricsUtilities.GetCombinations<PokerCard>(possibleOpponentCards, opponentCards =>
            {
                CardList opponent7Card = new CardList(communityCards);
                opponent7Card.AddRange(opponentCards);
                evaluator.DetermineBestHand(opponent7Card);

                PokerHand opponentHand = evaluator.GetBestHand();

                if (myHand > opponentHand)
                {
                    beatHand += 1;
                }
                else if (myHand == opponentHand)
                {
                    tieWithHand += 1;
                }
                else
                {
                    looseToHand += 1;
                }
            }, 2, false);

            if (beatHand > looseToHand && beatHand > tieWithHand)
            {
                wtl.wins += 1;
            }
            else if (tieWithHand > looseToHand && tieWithHand > beatHand)
            {
                wtl.ties += 1;
            }
            else
            {
                wtl.losses += 1;
            }
            AppendInfo("win/tie/loss: " + wtl.wins + "/" + wtl.ties + "/" + wtl.losses);

            return wtl;
        }
        #endregion

        #region Cards
        
        /// <summary>
        /// Gives back a full deck without the given cards
        /// </summary>
        /// <param name="myCards">Cards to be left out from the deck</param>
        /// <returns></returns>
        protected static CardList GetFullDeckWithoutGivenCards(CardList myCards)
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

        /// <summary>
        /// Gives back the strength of the hand, based on wins,ties and losses.
        /// </summary>
        /// <param name="wins"></param>
        /// <param name="ties"></param>
        /// <param name="losses"></param>
        /// <returns></returns>
        protected HandStrength EvaluteHandStrength(double wins, double ties, double losses)
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

        #region Decisions
        /// <summary>
        /// Decides what to do in current turn, based on the HandStrength
        /// Plays defensively
        /// </summary>
        protected virtual void DecisionByCardStrength()
        {
            AppendInfo("Hand Strength: " + handStrength);
            
            if (table.MainPot.GetAmountToCall(player) == 0)
            {
                decision = new CallDecision(player);
                AppendInfo("I call");
            }
            else if (handStrength == HandStrength.VeryGood)
            {
                int randBet = MakeRandomBet();
                decision = new BetDecision(player, randBet);
                AppendInfo("I bet " + randBet);
            }
            else if ((handStrength == HandStrength.Bad || handStrength == HandStrength.VeryBad || handStrength == HandStrength.Undecidable) 
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

        protected int MakeRandomBet()
        {
            Random randomGenerator = new Random();

            int minBet = Table.Instance.MainPot.AmountToBeEligibleForPot + Table.Instance.MainPot.LargestBet;
            int maxBet = player.ChipCount;
            int atLeastBet = (player.ChipCount < minBet) ? player.ChipCount : minBet;

            int randBet = randomGenerator.Next(atLeastBet, maxBet);
            randBet = (int)Math.Round((randBet / 100.0)) * 100;
            if (randBet < atLeastBet)
            {
                randBet = atLeastBet;
            }
            if (randBet > maxBet)
            {
                randBet = maxBet;
            }

            return randBet;
        }

        #endregion

        protected override void MakeRevealCardAIDecision()
        {
            decision = new ShowCardsDecision(player);
            AppendInfo("I show my cards");
        }

        public override string ToString()
        {
            return "Statistical AI 2";
        }

        public override PlayerController Clone()
        {
            return new StatisticalAIController();
        }
    }
}
