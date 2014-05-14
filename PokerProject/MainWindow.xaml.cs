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
using PokerProject.PokerGame.PlayerClasses.PlayerAIs;

namespace PokerProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            int index = 1;
            foreach (PlayerAdder item in AdderPanel.Children)
	        {
                item.playerCheckBox.Content = index++;
                item.playerControllerComboBox.ItemsSource = PlayerControllerFactory.GetItemsSource();
                item.playerControllerComboBox.SelectedItem = item.playerControllerComboBox.Items[0];
	        }
        }

        private void LoadPlayerAdder1(object sender, RoutedEventArgs e)
        {
            PlayerAdder1.playerCheckBox.Content = "1";
            PlayerAdder1.playerCheckBox.IsChecked = true;
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            PokerGame.Game game = new PokerGame.Game();
            
            foreach (PlayerAdder pAdder in AdderPanel.Children)
            {
                if ((bool)pAdder.playerCheckBox.IsChecked)
                {
                    Player player = new Player(pAdder.playerNameBox.Text);
                    player.Controller = (PlayerController)pAdder.playerControllerComboBox.SelectedItem;

                    game.AddPlayerToGame(player);
                }
            }

            bool automated = (bool)AutoAISwitch.IsChecked;
            foreach (Player player in game.GetPlayerList())
            {
                player.Controller.SetAutomated(automated);
            }


            bool autoTurnEnd = (bool)AutoTurnEndSwitch.IsChecked;
            game.SetAutoTurnEnd(autoTurnEnd);

            int gameTurns = 1;
            if (GameTurnsCheckBox.IsChecked == true)
            {
                try
                {
                    gameTurns = Convert.ToInt32(GameTurnsCounter.Text);
                }
                catch (Exception)
                {
                    return;
                }
            }

            if (OutputFileCheckBox.IsChecked == true)
            {
                game.SetOutPutFile(OutputFileTextBox.Text);
            }

            GameWindow gWindow = new GameWindow(game);
            gWindow.StartGame(gameTurns);
            this.Visibility = System.Windows.Visibility.Hidden;

            //After closing the game window, this window should appear again.
            gWindow.Closed += ReappearMainWindow;
        }

        void ReappearMainWindow(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            Focus();
        }
    }
}
