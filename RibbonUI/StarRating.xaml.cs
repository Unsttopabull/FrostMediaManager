using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RibbonUI {

    /// <summary>Interaction logic for StarRating.xaml</summary>
    public partial class StarRating : UserControl {
        private const string PACK_URI_FORMAT = "pack://application:,,,/RibbonUI;component/{0}";
        private static readonly BitmapImage StarEmpty;
        private static readonly BitmapImage StarHalf;
        private static readonly BitmapImage StarFull;

        public static readonly FrameworkPropertyMetadata RatingPropertyMetadata = new FrameworkPropertyMetadata(default(double?), RatingChanged);

        public static readonly DependencyProperty RatingProperty = 
            DependencyProperty.Register("Rating", typeof(double?), typeof(StarRating), RatingPropertyMetadata);

        static StarRating() {
            StarFull = new BitmapImage(new Uri(string.Format(PACK_URI_FORMAT, "Images/Stars/star.png")));
            StarHalf = new BitmapImage(new Uri(string.Format(PACK_URI_FORMAT, "Images/Stars/starhalf.png")));
            StarEmpty = new BitmapImage(new Uri(string.Format(PACK_URI_FORMAT, "Images/Stars/starempty.png")));
        }

        public StarRating() {
            InitializeComponent();
        }

        public double? Rating {
            get { return (double?) GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        private static void RatingChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e) {
            StarRating sr = (StarRating) obj;
            sr.ChangeRating((double?) e.NewValue);
        }

        private void ChangeRating(double? value) {
            if (!value.HasValue) {
                value = 0;
            }
            else {
                value /= 2;
            }

            if (value < 0 && value < 0.5) {
                Star1.Source = StarEmpty;
                Star2.Source = StarEmpty;
                Star3.Source = StarEmpty;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 0.5 && value < 1) {
                Star1.Source = StarHalf;
                Star2.Source = StarEmpty;
                Star3.Source = StarEmpty;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 1 && value < 1.5) {
                Star1.Source = StarFull;
                Star2.Source = StarEmpty;
                Star3.Source = StarEmpty;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 1.5 && value < 2) {
                Star1.Source = StarFull;
                Star2.Source = StarHalf;
                Star3.Source = StarEmpty;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 2 && value < 2.5) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarEmpty;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 2.5 && value < 3) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarHalf;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 3 && value < 3.5) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarFull;
                Star4.Source = StarEmpty;
                Star5.Source = StarEmpty;
            }

            if (value >= 3.5 && value < 4) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarFull;
                Star4.Source = StarHalf;
                Star5.Source = StarEmpty;
            }

            if (value >= 4 && value < 4.5) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarFull;
                Star4.Source = StarFull;
                Star5.Source = StarEmpty;
            }

            if (value >= 4.5 && value < 5) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarFull;
                Star4.Source = StarFull;
                Star5.Source = StarHalf;
            }

            if (value >= 5) {
                Star1.Source = StarFull;
                Star2.Source = StarFull;
                Star3.Source = StarFull;
                Star4.Source = StarFull;
                Star5.Source = StarFull;
            }
        }
    }

}