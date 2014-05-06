﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame
{
    public class Game
    {
        GameWindow window;
        Table table;
        CardDeck deck;
        PlayerQueue players;
        System.Threading.Semaphore semaphore;

        public Game()
        {
            table = Table.Instance;
            this.deck = new CardDeck();
            players = table.Players;
            semaphore = new System.Threading.Semaphore(0, 1);
        }

        public void BindGameWindow(GameWindow window)
        {
            this.window = window;
        }

        /*
         * 
         * */
        public void PlayTheGame()
        {
            SetupGame();
            while (GameIsGoing())
            {
                MainGameTurn();
            }
            //EndGame?
        }

        private void SetupGame()
        {
            //Have to decide who is the first dealer.
            Player firstPlayer = players.GetFirstPlayer();
            table.Positions.SetDealer(firstPlayer);

            table.SetBlind(50);
        }

        /*
         * If there's at least two players in game, the game is going on
         * */
        private bool GameIsGoing()
        {
            return (players.Count() > 1);
        }

        /*
         * Main game states happen here
         * */
        private void MainGameTurn()
        {
            SetupTurn();

            players.SetBettingOrder();
            PlaceBlinds();
            BettingPhase();

            if (!IsOnlyOnePlayerActive())
            {
                table.DealFlopCards(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            if (!IsOnlyOnePlayerActive())
            {
                table.DealRiverCard(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            if (!IsOnlyOnePlayerActive())
            {
                table.DealTurnCard(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            EndTurn();
        }

        /*
         * Setting up for the turn.
         * */
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

        /*
         * Every player who is still in play decides what he wants to do.
         * If somebody raises, or bets, everybody else gets to decide again.
         * */
        private void BettingPhase()
        {
            while (players.HasNextPlayer() && IsOnlyOnePlayerActive() == false)
            {
                int currentShare = table.MainPot.AmountToBeEligibleForPot;

                Player player = players.GetNextPlayer();
                if (player.IsIngame())
                {
                    window.SetActivePlayer(player);
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
        }

        /*
         * The turn ends when only one player left in game.
         * */
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

        private void EndTurn()
        {

            //Pay out the winner
            List<Player> winners = new List<Player>();
            if (IsOnlyOnePlayerActive() == false)
            {
                CardShowingPhase();
            }
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
                    window.WriteMessage("The winner is " + winners[0].Name + " with " + gwEvaluator.WinnerHand.Category + " with the rank of " + gwEvaluator.WinnerHand.Rank);
                }
                
            }
            else
            {
                foreach (Player player in players.GetPlayersList())
                {
                    if (player.IsIngame())
                    {
                        window.SetActivePlayerToShowCards(player);
                        window.RefreshGameView();
                        player.MakeRevealCardDecision();
                        winners.Add(player);
                    }
                }
            }
            
            Pot mainPot = table.MainPot;

            foreach (Player winner in winners)
            {
                winner.IncreaseChipCount(mainPot.Size);
                window.WriteMessage(winner.Name + " won " + mainPot.Size + "$"); 
            }

            //Show the final state of the turn
            window.RefreshGameView();
            window.WaitForNextTurn();
            semaphore.WaitOne();

            //Fold all cards
            foreach (Player player in players.GetPlayersList())
            {
                player.FoldCards();
            }

            //Delete players who doesnt have money left

            List<Player> playersToRemove = new List<Player>();
            foreach (Player player in players.GetPlayersList())
            {
                if (player.ChipCount <= 0)
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

        private void CardShowingPhase()
        {
            players.SetBettingOrder();

            while (players.HasNextPlayer() && IsOnlyOnePlayerActive() == false)
            {
                Player player = players.GetNextPlayer();
                if (player.IsIngame())
                {
                    window.SetActivePlayerToShowCards(player);
                    window.RefreshGameView();

                    player.MakeRevealCardDecision();
                }
            }
        }


        public void Continue()
        {
            semaphore.Release();
        }

    }
}
