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
            PokerGame.Table table = PokerGame.Table.Instance;
            PokerGame.PlayerClasses.PlayerQueue players = table.Players;
            
            foreach (PlayerAdder pAdder in AdderPanel.Children)
            {
                if ((bool)pAdder.playerCheckBox.IsChecked)
                {
                    Player player = new Player(pAdder.playerNameBox.Text);
                    player.Controller = (PlayerController)pAdder.playerControllerComboBox.SelectedItem;

                    players.AddPlayer(player);
                }
            }

            this.Visibility = System.Windows.Visibility.Hidden;
            PokerGame.Game game = new PokerGame.Game();

            GameWindow gWindow = new GameWindow(game);
            gWindow.Visibility = System.Windows.Visibility.Visible;

            gWindow.Closed += ReappearMainWindow;
        }

        void ReappearMainWindow(object sender, EventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Visible;
            Focus();
        }
    }
}
