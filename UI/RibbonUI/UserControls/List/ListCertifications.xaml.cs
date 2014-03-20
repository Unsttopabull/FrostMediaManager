using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for ListCertifications.xaml</summary>
    public partial class ListCertifications : UserControl {
        public static readonly DependencyProperty CertificationsProperty = DependencyProperty.Register("Certifications", typeof(ObservableCollection<MovieCertification>), typeof(ListCertifications), new PropertyMetadata(default(ObservableCollection<MovieCertification>), OnCertificationsChanged));

        public ListCertifications() {
            InitializeComponent();
        }

        public ObservableCollection<MovieCertification> Certifications {
            get { return (ObservableCollection<MovieCertification>) GetValue(CertificationsProperty); }
            set { SetValue(CertificationsProperty, value); }
        }

        private static void OnCertificationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListCertificationsViewModel) ((ListCertifications) d).DataContext).Certifications = (ObservableCollection<MovieCertification>) e.NewValue;
        }
    }
}
