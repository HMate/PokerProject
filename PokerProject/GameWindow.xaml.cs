using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using PokerProject.PokerGame;
using PokerProject.PokerGame.PlayerClasses;
using PokerProject.PokerGame.CardClasses;
using System.Windows.Automation.Provider;
using System.Windows.Automation.Peers;

namespace PokerProject
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        Game game;
        System.Threading.Thread gameThread;

        public GameWindow(Game game)
        {
            InitializeComponent();
            this.game = game;

            AddPlayersToTable();
            RefreshMainPot();

            Visibility = System.Windows.Visibility.Visible;
        }

        public void StartGame()
        {
            if (gameThread == null)
            {
                gameThread = new System.Threading.Thread(new System.Threading.ThreadStart(game.PlayTheGame));
                gameThread.Start();
                MessageBox.Items.Add("New game started.");
            }
        }

        private void AddPlayersToTable()
        {
            System.Collections.IEnumerator children = GameView.Children.GetEnumerator();
            children.MoveNext();
            while (children.Current.GetType() != typeof(PlayerPresenter))
            {
                children.MoveNext();
            }

            foreach (Player player in PokerGame.Table.Instance.Players.GetPlayersList())
            {
                ((PlayerPresenter)children.Current).AddPlayer(player);
                children.MoveNext();
            }
            children.Reset();
        }

        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gameThread != null && gameThread.IsAlive)
	        {
                /*
                 * This can be dangerous!
                 * */
                gameThread.Abort();
                gameThread.Join();
                gameThread = null;
	        }
            foreach (UIElement child in GameView.Children)
            {
                if (child is PlayerPresenter)
                {
                    ((PlayerPresenter)child).DeletePlayer();
                }
            }

            PokerGame.Table.Instance.Players.Clear();
        }

        public void RefreshGameView()
        {
            RefreshMainPot();
            RefreshPlayers();
            RefreshCommunityCards();
        }

        public void RefreshMainPot()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                MainPotLabel.Content = PokerGame.Table.Instance.MainPot.Size;
                MainPotMinimumBetLabel.Content = PokerGame.Table.Instance.MainPot.LargestBet;
            }
            ));
        }

        public void RefreshPlayers()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                foreach (UIElement child in GameView.Children)
                {
                    if (child is PlayerPresenter)
                    {
                        ((PlayerPresenter)child).RefreshPlayer();
                    }
                }
            }
            ));
        }

        public void RefreshCommunityCards()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                CardList list = PokerGame.Table.Instance.CommunityCards;
                int index = 0;
                foreach (CardImage card in CommunityCardsPanel.Children)
                {
                    if (index < list.Count)
                    {
                        card.SetCard(list.ElementAt(index));
                        index++;
                    }
                    else
                    {
                        card.SetCardBack();
                    }
                }
            }
            ));
        }

        public void SetActivePlayer(Player activePlayer)
        {
            this.Dispatcher.Invoke((Action)( () =>
            {
                CardList list = activePlayer.ShowCards();
                Card1.SetCard(list[0]);
                Card2.SetCard(list[1]);
                ActivePlayerLabel.Visibility = System.Windows.Visibility.Visible;
                ActivePlayerLabel.Content = activePlayer.Name;
                ToCallLabel.Visibility = System.Windows.Visibility.Visible;
                ToCallLabel.Content = PokerGame.Table.Instance.MainPot.GetAmountToCall(activePlayer);
                PlayerControlUI.Visibility = System.Windows.Visibility.Visible;
                PlayerControlUI.SetPlayer(activePlayer);
            }
            ));
        }

        public void SetActivePlayerToShowCards(Player activePlayer)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                CardList list = activePlayer.ShowCards();
                Card1.SetCard(list[0]);
                Card2.SetCard(list[1]);
                ActivePlayerLabel.Visibility = System.Windows.Visibility.Visible;
                ActivePlayerLabel.Content = activePlayer.Name;
                ToCallLabel.Visibility = System.Windows.Visibility.Hidden;
                PlayerControlUI.Visibility = System.Windows.Visibility.Visible;
                PlayerControlUI.SetPlayerToShowCards(activePlayer);
            }
            ));
        }

        public void WriteMessage(string message)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                MessageBox.Items.Add(message);

                ListBoxAutomationPeer svAutomation = (ListBoxAutomationPeer)ScrollViewerAutomationPeer.CreatePeerForElement(MessageBox);
                
                IScrollProvider scrollInterface = (IScrollProvider)svAutomation.GetPattern(PatternInterface.Scroll);
                System.Windows.Automation.ScrollAmount scrollVertical = System.Windows.Automation.ScrollAmount.LargeIncrement;
                System.Windows.Automation.ScrollAmount scrollHorizontal = System.Windows.Automation.ScrollAmount.NoAmount;
                //If the vertical scroller is not available, the operation cannot be performed, which will raise an exception. 
                if (scrollInterface != null)
                {
                    if (scrollInterface.VerticallyScrollable)
                        scrollInterface.Scroll(scrollHorizontal, scrollVertical);
                }
            }
            ));
        }

        public void WaitForNextTurn()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                Card1.SetEmptyCard();
                Card2.SetEmptyCard();
                ActivePlayerLabel.Visibility = System.Windows.Visibility.Hidden;
                ToCallLabel.Visibility = System.Windows.Visibility.Hidden;
                NextTurnButton.Visibility = System.Windows.Visibility.Visible;
                PlayerControlUI.Visibility = System.Windows.Visibility.Hidden;
            }
            ));
        }

        private void NextTurnClick(object sender, RoutedEventArgs e)
        {
            NextTurnButton.Visibility = System.Windows.Visibility.Hidden;
            game.Continue();
        }

    }
}
