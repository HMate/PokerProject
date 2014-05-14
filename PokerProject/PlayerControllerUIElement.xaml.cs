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
    /// UI for controlling the current active player
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
            player.Controller.InfoChanged -= AddAiInfo;
            this.player = player;
            HideEverything();

            if (player.Controller is PokerGame.PlayerClasses.PlayerAIs.HumanController)
            {
                BetSlider.Visibility = System.Windows.Visibility.Visible;
                BetBox.Visibility = System.Windows.Visibility.Visible;
                CallButton.Visibility = System.Windows.Visibility.Visible;
                RaiseButton.Visibility = System.Windows.Visibility.Visible;
                FoldButton.Visibility = System.Windows.Visibility.Visible;

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
            else
            {
                AiOKButton.Visibility = System.Windows.Visibility.Visible;
                AiInfoBox.Visibility = System.Windows.Visibility.Visible;
                AiInfoBox.Items.Clear();
                player.Controller.InfoChanged += AddAiInfo;
            }
        }

        public void SetPlayerToShowCards(Player player)
        {
            player.Controller.InfoChanged -= AddAiInfo;
            this.player = player;
            HideEverything();

            if (player.Controller is PokerGame.PlayerClasses.PlayerAIs.HumanController )
            {
                ShowCardsButton.Visibility = System.Windows.Visibility.Visible;
                FoldButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                AiOKButton.Visibility = System.Windows.Visibility.Visible;
                AiInfoBox.Visibility = System.Windows.Visibility.Visible;
                AiInfoBox.Items.Clear();
                player.Controller.InfoChanged += AddAiInfo;
            }
        }

        private void RaisePressed(object sender, RoutedEventArgs e)
        {
            try
            {
                int value = Convert.ToInt32(BetBox.Text);
                player.Controller.MakeBetDecision(value);
                HideEverything();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
        }

        private void FoldPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeFoldDecision();
            HideEverything();
        }

        private void CallPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeCallDecision();
            HideEverything();
        }

        private void ShowPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeShowCardsDecision();
            HideEverything();
        }

        private void SliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BetBox.Text = ((int)BetSlider.Value).ToString();
        }

        private void AiOKClicked(object sender, RoutedEventArgs e)
        {
            player.Controller.ApproveDecision();
            HideEverything();
        }

        private void AddAiInfo(List<string> messages)
        {
            Dispatcher.Invoke((Action)(() =>
            {
                foreach (string message in messages)
                {
                    AiInfoBox.Items.Add(message);
                }
            }
            ));
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
                    if (bet < 0)
                    {
                        BetBox.Text = minBet.ToString();
                    }
                }
                catch (Exception)
                {
                    BetBox.Text = minBet.ToString();
                }
            }
        }

        private void HideEverything()
        {
            ShowCardsButton.Visibility = System.Windows.Visibility.Hidden;
            BetSlider.Visibility = System.Windows.Visibility.Hidden;
            BetBox.Visibility = System.Windows.Visibility.Hidden;
            CallButton.Visibility = System.Windows.Visibility.Hidden;
            RaiseButton.Visibility = System.Windows.Visibility.Hidden;
            FoldButton.Visibility = System.Windows.Visibility.Hidden;
            AiOKButton.Visibility = System.Windows.Visibility.Hidden;
            AiInfoBox.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
