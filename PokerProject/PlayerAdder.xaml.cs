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

namespace PokerProject
{
    /// <summary>
    /// Interaction logic for PlayerAdder.xaml
    /// </summary>
    public partial class PlayerAdder : UserControl
    {

        private string defaultName;

        public PlayerAdder()
        {
            InitializeComponent();
            defaultName = NameGenerator.GenerateName();
            playerNameBox.Text = defaultName;
        }

        private void ResetDefaultName(object sender, RoutedEventArgs e)
        {
            if (playerNameBox.Text == defaultName)
            {
                playerNameBox.Text = "";
            }
        }
    }
}
