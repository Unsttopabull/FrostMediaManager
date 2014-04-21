using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for ListCertifications.xaml</summary>
    public partial class ListCertifications : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(ListCertifications), new PropertyMetadata(default(ObservableMovie), OnCertificationsChanged));

        public ListCertifications() {
            InitializeComponent();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void OnCertificationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListCertificationsViewModel) ((ListCertifications) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }
    }
}
