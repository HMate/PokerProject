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
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
            Pot mainPot = PokerGame.Table.Instance.MainPot;
            BetBox.Text = (mainPot.GetAmountToCall(player)).ToString();
        }

        private void RaisePressed(object sender, RoutedEventArgs e)
        {
            int value = Convert.ToInt32(BetBox.Text);
            player.Controller.MakeBetDecision(value);
        }

        private void FoldPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeFoldDecision();
        }

        private void CallPressed(object sender, RoutedEventArgs e)
        {
            player.Controller.MakeCallDecision();
        }
    }
}
