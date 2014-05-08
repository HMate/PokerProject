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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokerProject.PokerGame.PlayerClasses;

namespace PokerProject
{
    /// <summary>
    /// Interaction logic for PlayerPresenter.xaml
    /// </summary>
    public partial class PlayerPresenter : UserControl
    {
        Player player;

        public PlayerPresenter()
        {
            InitializeComponent();
        }

        public void AddPlayer(Player player)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.player = player;
                Visibility = System.Windows.Visibility.Visible;
                PlayerName.Content = player.Name;
                Chips.Content = player.ChipCount;

                foreach (CardImage image in Cards.Children)
                {
                    image.SetEmptyCard();
                }
            }
            ));
        }

        public void RefreshPlayer()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                if (player != null)
                {
                    PlayerName.Content = player.Name;
                    Chips.Content = player.ChipCount;
                    Chips.ToolTip = "Player contributed to pot with " + PokerGame.Table.Instance.MainPot.PlayerBetThisTurn(player) + "$"; 
                    if (player.IsIngame() || player.RevealCards)
                    {
                        if (player.RevealCards)
                        {
                            PokerGame.CardClasses.CardList cards = player.ShowCards();
                            ((CardImage)Cards.Children[0]).SetCard(cards[0]);
                            ((CardImage)Cards.Children[1]).SetCard(cards[1]);
                        }
                        else
                        {
                            foreach (CardImage image in Cards.Children)
                            {
                                image.SetCardBack();
                            }
                        }   
                    }
                    else
                    {
                        foreach (CardImage image in Cards.Children)
                        {
                            image.SetEmptyCard();
                        }
                    }
                }
            }
            ));
        }

        private void SetCards(Action action)
        {
            foreach (CardImage image in Cards.Children)
            {
                action();
            }
        }

        public void DeletePlayer()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.player = null;
                Visibility = System.Windows.Visibility.Hidden;
            }
            ));
        }
    }
}
