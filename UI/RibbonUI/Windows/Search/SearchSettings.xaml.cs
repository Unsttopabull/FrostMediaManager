using System.Windows;
using RibbonUI.Util;

namespace RibbonUI.Windows.Search {

    /// <summary>Interaction logic for SearchSettings.xaml</summary>
    public partial class SearchSettings : Window {
        public SearchSettings() {
            Loaded += SearchSettingsLoaded;
            InitializeComponent();
        }

        public bool SearchArt { get; set; }
        public bool SearchInfo { get; set; }
        public bool SearchVideos { get; set; }

        public Plugin ArtPlugin { get; set; }
        public Plugin InfoPlugin { get; set; }
        public Plugin VideoPlugin { get; set; }

        private void SearchSettingsLoaded(object sender, RoutedEventArgs e) {
            NativeMethods.HideWindowBorderButtons(this);
        }

        private void SearchSettingsOnCancelClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void OnSearchClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
