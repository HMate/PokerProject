using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;
using System.IO;

namespace PokerProject.PokerGame
{
    public class Game
    {
        private const int startingChips = 2000;

        private GameWindow window;
        private System.Threading.Semaphore semaphore;
        private Table table;
        private CardDeck deck;
        private PlayerQueue players;
        private List<Player> startingPlayers;
        string outputFileName;

        private int turns;
        private bool autoTurnEnd;

        public Game()
        {
            semaphore = new System.Threading.Semaphore(0, 1);
            table = Table.Instance;
            this.deck = new CardDeck();
            players = table.Players;
            startingPlayers = new List<Player>();
            outputFileName = "LastGame.txt";

            autoTurnEnd = false;
        }

        public void BindGameWindow(GameWindow window)
        {
            this.window = window;
        }

        /// <summary>
        /// Continues the game if it waits at the end of a turn.
        /// </summary>
        public void Continue()
        {
            semaphore.Release();
        }

        /// <summary>
        /// Sets that the game should wait for a call to the method Continue() at the end of every turn.
        /// </summary>
        /// <param name="enabled"></param>
        public void SetAutoTurnEnd(bool enabled)
        {
            autoTurnEnd = enabled;
        }

        /// <summary>
        /// Players added by this method will be added to the game when it starts every time.
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayerToGame(Player player)
        {
            startingPlayers.Add(player);
        }

        /// <summary>
        /// Gives back the players that are added to a game at the start of a new game.
        /// </summary>
        /// <returns></returns>
        public List<Player> GetPlayerList()
        {
            return startingPlayers;
        }

        /// <summary>
        /// Sets the target output file to the given file in the /stats directory.
        /// </summary>
        /// <param name="filename"></param>
        public void SetOutPutFile(string filename)
        {
            outputFileName = filename;
        }

        /// <summary>
        /// Starts the game. Restarts the game after it finishes the given number of times.
        /// </summary>
        /// <param name="turns"> The number of how many games would be played after each other.</param>
        public void PlayTheGame(int gameRounds = 1)
        {
            List<string> toFile = new List<string>();
            Dictionary<Player, int> winners = new Dictionary<Player, int>();
            DateTime startTime = DateTime.Now;

            for (int round = 0; round < gameRounds; round++)
            {
                window.WriteMessage(round+1 + ". game started!");

                GameMain();

                Player winner = players.GetPlayersList()[0];
                window.WriteMessage(winner.Name + " has won the game!");
                window.WriteMessage("-------------------");

                if (winners.ContainsKey(winner))
                {
                    winners[winner] += 1;
                }
                else
                {
                    winners.Add(winner, 1);
                }
                
                toFile.Add(String.Format("Game {0}: Winner: {1}, Type: {2}", round, winner.Name, winner.Controller.ToString() ));
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"stats/" + outputFileName))
            {
                file.WriteLine(String.Format("Date: {0}, Duration: {1}", DateTime.Now, DateTime.Now - startTime));
                file.WriteLine("Players:");
                foreach (Player player in startingPlayers)
                {
                    string name = player.Name;
                    string type = player.Controller.ToString();
                    file.WriteLine(String.Format("{0} ({1}) ", name, type));
                }

                file.WriteLine("");
                file.WriteLine("Results:");
                foreach (Player player in winners.Keys)
                {
                    string name = player.Name;
                    string type = player.Controller.ToString();
                    int roundsWon = winners[player];
                    double wonPercent = ((double)winners[player]) * 100 / gameRounds;
                    file.WriteLine(String.Format("{0} ({1}) won {2} games ({3}%)", name, type, roundsWon, wonPercent));
                }

                file.WriteLine("");
                file.WriteLine("Games:");
                foreach (string line in toFile)
                {
                    file.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// The actual game happens here.
        /// </summary>
        private void GameMain()
        {
            SetupGame();
            while (GameIsGoing())
            {
                MainGameTurn();
                AdjustBlinds();
            }
        }

        private void SetupGame()
        {
            SetupPlayers();

            //Have to decide who is the first dealer.
            Player firstPlayer = players.GetFirstPlayer();
            table.Positions.SetDealer(firstPlayer);

            turns = 1;
            table.SetBigBlind(50);
            table.SetSmallBlind(25);
            table.MainPot.TotalSize = players.Count() * startingChips;
        }

        /// <summary>
        /// Puts the players in game and gives them chips based on the startingPlayers variable
        /// </summary>
        private void SetupPlayers()
        {
            players.Clear();
            foreach (Player player in startingPlayers)
            {
                player.ChipCount = startingChips;
                players.AddPlayer(player);
            }
        }

        private void AdjustBlinds()
        {
            if (turns % 30 == 0)
            {
                table.SetBigBlind(table.GetBigBlind() + 50);
                table.SetSmallBlind(table.GetSmallBlind() + 25);
            }
        }

        /// <summary>
        /// If there's at least two players in game, the game is going on
        /// </summary>
        private bool GameIsGoing()
        {
            return (players.Count() > 1);
        }

        /// <summary>
        /// Main game states happen here
        /// </summary>
        private void MainGameTurn()
        {
            SetupTurn();

            table.CurrentGamePhase = GamePhase.PreFlop;
            players.SetBettingOrder();
            PlaceBlinds();
            BettingPhase();

            if (!IsOnlyOnePlayerActive())
            {
                table.CurrentGamePhase = GamePhase.Flop;
                table.DealFlopCards(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            if (!IsOnlyOnePlayerActive())
            {
                table.CurrentGamePhase = GamePhase.River;
                table.DealRiverCard(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            if (!IsOnlyOnePlayerActive())
            {
                table.CurrentGamePhase = GamePhase.Turn;
                table.DealTurnCard(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            EndTurn();
        }

        /// <summary>
        /// Setting up for the turn.
        /// </summary>
        private void SetupTurn()
        {
            //Put everybody back to the game
            foreach (Player player in players.GetPlayersList())
            {
                player.SetIngame(true);
            }

            table.MainPot.Empty();
            deck.createNewPokerDeck();
            table.ResetCommunityCards();
            window.WriteMessage(turns + ". turn started");

            //Set the next dealer for the turn.
            table.Positions.SetNextHandPositions();

            DealPlayerCards();
        }

        private void DealPlayerCards()
        {
            //Deal Player cards
            players.SetBettingOrder();
            while (players.HasNextPlayer())
            {
                Player player = players.GetNextPlayer();
                player.DrawCard(deck);
                player.DrawCard(deck);
            }
        }

        private void PlaceBlinds()
        {
            Player smallBlind = players.GetNextPlayer();
            smallBlind.PostBlind();

            Player bigBlind = players.GetNextPlayer();
            bigBlind.PostBlind();

            Player firstPlayer = players.GetNextPlayerAfterPlayer(bigBlind);
            players.SetPlayerFirstInOrder(firstPlayer);

        }

        /// <summary>
        /// Every player who is still in play decides what he wants to do.
        /// If somebody raises, or bets, everybody else gets to decide again.
        /// </summary>
        private void BettingPhase()
        {
            while (players.HasNextPlayer() && IsOnlyOnePlayerActive() == false)
            {
                int currentShare = table.MainPot.AmountToBeEligibleForPot;

                Player player = players.GetNextPlayer();
                if (player.IsIngame())
                {
                    if (player.Controller.IsAutomated() == false)
                    {
                        window.SetActivePlayer(player);
                    }
                    window.RefreshGameView();

                    player.MakeDecision();
                }

                int afterPlayerShare = table.MainPot.AmountToBeEligibleForPot;
                if (afterPlayerShare > currentShare)
                {
                    //the current player will become the first in the order, but he shouldn't come again.
                    players.SetPlayerFirstInOrder(player);
                    players.GetNextPlayer();
                }
            }
            table.MainPot.PayBackUnmatchedBets();
        }

        private void EndTurn()
        {
            //Pay out the winners
            CardShowingPhase();
            window.RefreshGameView();
            DetermineAndPayOutWinners();

            //Show the final state of the turn
            if (!autoTurnEnd)
            {
                window.WaitForNextTurn();
                semaphore.WaitOne(); 
            }

            //Fold all cards
            foreach (Player player in players.GetPlayersList())
            {
                player.FoldCards();
            }

            //Delete players who doesnt have money left
            RemovePlayersFromGame();

            turns++;
            window.WriteMessage("");
        }

        private void CardShowingPhase()
        {
            players.SetBettingOrder();

            while (players.HasNextPlayer() && IsOnlyOnePlayerActive() == false)
            {
                Player player = players.GetNextPlayer();
                if (player.IsIngame())
                {
                    if (player.Controller.IsAutomated() == false)
                    {
                        window.SetActivePlayerToShowCards(player);
                        window.RefreshGameView(); 
                    }

                    player.MakeRevealCardDecision();
                }
            }
        }

        private void DetermineAndPayOutWinners()
        {
            Pot mainPot = table.MainPot;
            while (mainPot.Size > 0 && AtLeastOnePlayerActive())
            {
                List<Player> winners = new List<Player>();
                if (IsOnlyOnePlayerActive() == false)
                {
                    GameWinnerEvaluator gwEvaluator = new GameWinnerEvaluator();
                    gwEvaluator.DetermineWinner();
                    if (gwEvaluator.IsTied())
                    {
                        winners = gwEvaluator.GetTiedWinners();
                        window.WriteMessage("The winner hand is " + gwEvaluator.WinnerHand.Category + " with the rank of " + gwEvaluator.WinnerHand.Rank);
                        string tiedWinners = "";
                        foreach (Player winner in winners)
                        {
                            tiedWinners = tiedWinners + " " + winner.Name;
                        }
                        window.WriteMessage("The winners are" + tiedWinners);

                    }
                    else
                    {
                        winners.Add(gwEvaluator.Winner);
                        window.WriteMessage("The winner is " + winners[0].Name + " with " + gwEvaluator.WinnerHand.Category + " ranked: " + gwEvaluator.WinnerHand.Rank);
                    }

                }
                else
                {
                    foreach (Player player in players.GetPlayersList())
                    {
                        if (player.IsIngame())
                        {
                            winners.Add(player);
                            window.WriteMessage("The winner is " + winners[0].Name);
                        }
                    }
                }

                int recieved = mainPot.PayOutPlayer(winners);
                window.WriteMessage("Winners won " + recieved + "$");

                foreach (Player winner in winners)
                {
                    if (mainPot.PlayerBetThisTurn(winner) == 0)
                    {
                        winner.SetIngame(false);
                    }
                }
            }
        }

        /// <summary>
        /// The turn ends when only one player left in game.
        /// </summary>
        private bool IsOnlyOnePlayerActive()
        {
            int activePlayers = 0;
            foreach (Player player in players.GetPlayersList())
            {
                if (player.IsIngame())
                {
                    activePlayers++;
                }
            }
            return (activePlayers < 2) ? true : false;
        }

        private bool AtLeastOnePlayerActive()
        {

            int activePlayers = 0;
            foreach (Player player in players.GetPlayersList())
            {
                if (player.IsIngame())
                {
                    activePlayers++;
                }
            }
            return (activePlayers > 0) ? true : false;
        }

        private void RemovePlayersFromGame()
        {
            List<Player> playersToRemove = new List<Player>();
            foreach (Player player in players.GetPlayersList())
            {
                if (player.ChipCount < table.GetBigBlind())
                {
                    playersToRemove.Add(player);
                }
            }

            foreach (Player player in playersToRemove)
            {
                players.DeletePlayer(player);
                window.WriteMessage(player.Name + " left the game");
            }
        }

    }
}
