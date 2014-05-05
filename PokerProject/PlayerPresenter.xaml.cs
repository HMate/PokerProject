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
                    if (player.IsIngame())
                    {
                        foreach (CardImage image in Cards.Children)
                        {
                            image.SetCardBack();
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
