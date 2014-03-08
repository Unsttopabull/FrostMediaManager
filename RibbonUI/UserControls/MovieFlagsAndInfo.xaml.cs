using System.Windows;
using System.Windows.Controls;
using Frost.Models.Frost.DB;
using RibbonUI.ViewModels.UserControls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for MovieFlagsAndInfo.xaml</summary>
    public partial class MovieFlagsAndInfo : UserControl {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register("MinRequiredWidth", typeof(double), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(Movie), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(Movie), SelectedMovieChanged));

        public MovieFlagsAndInfo() {
            InitializeComponent();
        }

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }
      
        public Movie SelectedMovie {
            get { return (Movie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private static void SelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((MovieFlagsAndInfoViewModel) ((MovieFlagsAndInfo) d).DataContext).SelectedMovie = (Movie) args.NewValue;
        }
    }
}
