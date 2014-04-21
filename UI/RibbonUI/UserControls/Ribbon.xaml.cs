using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(Ribbon), new PropertyMetadata(default(ObservableMovie), OnMovieChanged));
        public static readonly DependencyProperty SelectedTabProperty = DependencyProperty.Register("SelectedTab", typeof(RibbonTabs), typeof(Ribbon), new PropertyMetadata(default(RibbonTabs), SelectedTabChanged));

        public Ribbon() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        public RibbonTabs SelectedTab {
            get { return (RibbonTabs) GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }

        private static void SelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((RibbonViewModel) ((Ribbon) d).DataContext).OnRibbonTabSelect(e.NewValue is RibbonTabs ? (RibbonTabs) e.NewValue : RibbonTabs.None);
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((RibbonViewModel) ((Ribbon) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }
    }

}