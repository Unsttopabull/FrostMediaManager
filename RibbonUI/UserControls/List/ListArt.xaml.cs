using System.Windows;
using System.Windows.Controls;
using Frost.Models.Frost.DB.Arts;
using RibbonUI.Util;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditArt.xaml</summary>
    public partial class ListArt : UserControl {
        public static readonly DependencyProperty ArtProperty = DependencyProperty.Register("Art", typeof(ObservableHashSet2<Art>), typeof(ListArt), new PropertyMetadata(default(ObservableHashSet2<Art>)));

        public ListArt() {
            InitializeComponent();
        }

        public ObservableHashSet2<Art> Art {
            get { return (ObservableHashSet2<Art>) GetValue(ArtProperty); }
            set { SetValue(ArtProperty, value); }
        }

        private void RemoveOnClick(object sender, RoutedEventArgs e) {
            
        }

        private void AddOnClick(object sender, RoutedEventArgs e) {
            
        }
    }
}
