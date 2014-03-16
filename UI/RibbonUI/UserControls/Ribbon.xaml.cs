using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(Ribbon), new PropertyMetadata(default(ObservableMovie), OnMovieChanged));

        public Ribbon() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((RibbonViewModel) ((Ribbon) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }
    }

}