using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ArtAndPlot.xaml</summary>
    public partial class ArtAndPlot : UserControl {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/nm{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";

        public ArtAndPlot() {
            InitializeComponent();
        }

        private void ActorImdbMouseDown(object sender, MouseButtonEventArgs e) {
            string imdbId = (string) ((Image) sender).Tag;

            if (!string.IsNullOrEmpty(imdbId)) {
                Process.Start(string.Format(IMDB_PERSON_URI, imdbId));
            }
        }

        private void GoToIMDB(object sender, MouseButtonEventArgs e) {
            string imdbId = (string) ((Image) sender).Tag;

            if (!string.IsNullOrEmpty(imdbId)) {
                Process.Start(string.Format(IMDB_TITLE_URI, imdbId));
            }
        }

        private void GoToTrailer(object sender, MouseButtonEventArgs e) {
            string trailer = (string) ((Image) sender).Tag;

            if (!string.IsNullOrEmpty(trailer)) {
                Process.Start(trailer);
            }
        }
    }

}