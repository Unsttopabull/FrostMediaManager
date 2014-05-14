using System.Windows;
using System.Windows.Controls;
using Frost.RibbonUI.Util.ObservableWrappers;

namespace Frost.RibbonUI.UserControls {

    /// <summary>Interaction logic for MovieFlagsAndInfo.xaml</summary>
    public partial class MovieFlagsAndInfo : UserControl {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register("MinRequiredWidth", typeof(double), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(ObservableMovie), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(ObservableMovie), SelectedMovieChanged));

        public MovieFlagsAndInfo() {
            InitializeComponent();
        }

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }
      
        public ObservableMovie SelectedMovie {
            get { return (ObservableMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private static void SelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((MovieFlagsAndInfoViewModel) ((MovieFlagsAndInfo) d).DataContext).SelectedMovie = (ObservableMovie) args.NewValue;
        }
    }
}
