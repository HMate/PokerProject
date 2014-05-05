using PokerProject.PokerGame.CardClasses;
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
    /// Interaction logic for CardImage.xaml
    /// </summary>
    public partial class CardImage : UserControl
    {

        private const int cardWidth = 225;
        private const int cardHeight = 315;

        public CardImage()
        {
            InitializeComponent();

            SetCardBack();
        }

        public void SetCard(PokerCard card)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                Uri deckUri = new Uri("Media/pokercards.png", UriKind.RelativeOrAbsolute);
                BitmapImage deckImage = new BitmapImage(deckUri);
                int cardPosHorizontalIndex = 0;
                int cardPosVerticalIndex = 0;

                if (card.Rank != CardRank.Ace)
                {
                    cardPosHorizontalIndex = (int)card.Rank - 1;
                }
                cardPosVerticalIndex = (int)card.Suite;

                //deckImage.DecodePixelHeight = (int)ActualHeight;

                CroppedBitmap cardImage = new CroppedBitmap(deckImage, new Int32Rect(cardPosHorizontalIndex * cardWidth, cardPosVerticalIndex * cardHeight, 225, 315));
            
                image.Source = cardImage;
            }
            ));

        }

        public void SetCardBack()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                Uri backUri = new Uri("Media/cardback.jpg", UriKind.RelativeOrAbsolute);
                BitmapImage backImage = new BitmapImage(backUri);
                backImage.DecodePixelHeight = 315;
                CroppedBitmap cardImage = new CroppedBitmap(backImage, new Int32Rect(0, 0, 225, 315));
                image.Source = backImage;
            }
            ));
        }

        public void SetEmptyCard()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                image.Source = null;
            }
            ));
        }
    }
}
