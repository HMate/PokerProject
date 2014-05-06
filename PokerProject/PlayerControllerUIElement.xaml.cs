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
using PokerProject.PokerGame;
using PokerProject.PokerGame.PlayerClasses;

namespace PokerProject
{
    /// <summary>
    /// Interaction logic for HumanController.xaml
    /// </summary>
    public partial class PlayerControllerUIElement : UserControl
    {
        Player player;

        public PlayerControllerUIElement()
        {
            InitializeComponent();

            BetSlider.TickPlacement = System.Windows.Controls.Primitives.TickPlacement.TopLeft;
            BetSlider.TickFrequency = 50;
            BetSlider.IsSnapToTickEnabled = true;
        }

        public void SetPlayer(Player player)
        {
            ShowCardsButton.Visibility = System.Windows.Visibility.Hidden;

            BetSlider.Visibility = System.Windows.Visibility.Visible;
            BetBox.Visibility = System.Windows.Visibility.Visible;
            CallButton.Visibility = System.Windows.Visibility.Visible;
            RaiseButton.Visibility = System.Windows.Visibility.Visible;

            this.player = player;
            Pot mainPot = PokerGame.Table.Instance.MainPot;

            int callAmount = (player.ChipCount > mainPot.GetAmountToCall(player)) ? mainPot.GetAmountToCall(player) : player.ChipCount;
            BetBox.Text = (callAmount).ToString();
            if (callAmount > 0)
            {
                CallButton.Content = "Call";
            }
            else
            {
                CallButton.Content = "Check";
            }

            BetSlider.Minimum = (player.ChipCount > callAmount) ? callAmount : player.ChipCount;
            BetSlider.Maximum = player.ChipCount;
            BetSlider.Value = (double)callAmount;
        }

        public void SetPlayerToShowCards(Player player)
        {
            this.player = player;

            BetSlider.Visibility = System.Windows.Visibility.Hidden;
            BetBox.Visibility = System.Windows.Visibility.Hidden;
            CallButton.Visibility = System.Windows.Visibility.Hidden;
            RaiseButton.Visibility = System.Windows.Visibility.Hidden;

            ShowCardsButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void RaisePressed(object sender, RoutedEventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(BetBox.Text);
                player.Controller.MakeBetDecision(value);
            }
            catch (Exception)
            {

            }
        }

        private void FoldPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeFoldDecision();
        }

        private void CallPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeCallDecision();
        }

        private void ShowPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeShowCardsDecision();
        }

        private void SliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BetBox.Text = ((int)BetSlider.Value).ToString();
        }

        private void ValidateBetBox(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void Validate(object sender, TextChangedEventArgs e)
        {
            if (player != null)
            {
                string bettext = BetBox.Text;
                int minBet = PokerGame.Table.Instance.MainPot.GetAmountToCall(player);
                int maxBet = player.ChipCount;

                try
                {
                    int bet = Convert.ToInt32(bettext);
                    if (bet > maxBet)
                    {
                        BetBox.Text = maxBet.ToString();
                    }
                }
                catch (Exception)
                {
                    BetBox.Text = minBet.ToString();
                }
            }
        }
    }
}
