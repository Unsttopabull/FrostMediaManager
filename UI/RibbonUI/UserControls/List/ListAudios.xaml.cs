using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditAudios.xaml</summary>
    public partial class ListAudios : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(ListAudios), new PropertyMetadata(default(ObservableMovie), MovieChanged));

        public ListAudios() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void MovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListAudiosViewModel) ((ListAudios) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }

        private void ListAudiosOnLoaded(object sender, RoutedEventArgs e) {
            ((ListAudiosViewModel) DataContext).ParentWindow = Window.GetWindow(this);
        }
    }
}
