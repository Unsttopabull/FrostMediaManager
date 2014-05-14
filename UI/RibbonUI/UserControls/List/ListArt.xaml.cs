using System.Windows;
using System.Windows.Controls;
using Frost.RibbonUI.Util.ObservableWrappers;

namespace Frost.RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditArt.xaml</summary>
    public partial class ListArt : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(ListArt), new PropertyMetadata(default(ObservableMovie), MovieChanged));

        public ListArt() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void MovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListArtViewModel) ((ListArt) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }

    }
}
