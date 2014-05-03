using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;

namespace PokerProject.PokerGame
{
    public class Game
    {

        Table table;
        CardDeck deck;
        PlayerQueue players;

        public Game()
        {
            table = Table.Instance;
            this.deck = new CardDeck();
            players = table.Players;
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

            if (!IsTurnEnd())
            {
                table.DealFlopCards(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            if (!IsTurnEnd())
            {
                table.DealRiverCard(deck);
                players.SetBettingOrder();
                BettingPhase();
            }

            if (!IsTurnEnd())
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
            while (players.HasNextPlayer())
            {
                int currentBet = table.MainPot.LargestBet;

                Player player = players.GetNextPlayer();
                if (player.IsIngame())
                {
                    player.MakeDecision();
                }

                int afterPlayerBet = table.MainPot.LargestBet;
                if (afterPlayerBet > currentBet)
                {
                    //the current player will become the first in the order, but he shoulnd't come again.
                    players.SetPlayerFirstInOrder(player);
                    players.GetNextPlayer();
                }
            }
        }

        /*
         * The turn ends when only one player left in game.
         * */
        private bool IsTurnEnd()
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
            GameWinnerEvaluator gwEvaluator = new GameWinnerEvaluator();
            gwEvaluator.DetermineWinner();

            //Pay out the winner
            Player winner = gwEvaluator.Winner;
            Pot mainPot = table.MainPot;

            winner.IncreaseChipCount(mainPot.Size);

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
            }
        }


    }
}
