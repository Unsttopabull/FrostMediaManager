using System.Windows;
using System.Windows.Controls;
using Frost.RibbonUI.Util.ObservableWrappers;

namespace Frost.RibbonUI.UserControls.List {

    /// <summary>Interaction logic for ListPromotionalVideos.xaml</summary>
    public partial class ListPromotionalVideos : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(ObservableMovie), typeof(ListPromotionalVideos), new PropertyMetadata(default(ObservableMovie), OnMovieChanged));

        public ListPromotionalVideos() {
            InitializeComponent();
        }

        public ObservableMovie SelectedMovie {
            get { return (ObservableMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListPromotionalVideosViewModel) ((ListPromotionalVideos) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }
    }
}
