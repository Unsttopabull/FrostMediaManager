using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using Frost.Common;

namespace RibbonUI.UserControls {

    using Translation;

    /// <summary>Interaction logic for ArtAndPlot.xaml</summary>
    public partial class ArtAndPlot : UserControl {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/nm{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";

        public ArtAndPlot() {
            InitializeComponent();
        }

        private void ActorImdbMouseDown(object sender, MouseButtonEventArgs e) {
            string imdbId = (string) ((Image) sender).ToolTip;

            if (imdbId != "Not Available") {
                Process.Start(string.Format(IMDB_PERSON_URI, imdbId));
            }
        }

        private void GoToIMDB(object sender, MouseButtonEventArgs e) {
            string imdbId = (string) ((Image) sender).ToolTip;

            if (imdbId != "Not Available") {
                Process.Start(string.Format(IMDB_TITLE_URI, imdbId));
            }
        }

        private void GoToTrailer(object sender, MouseButtonEventArgs e) {
             string trailer = (string) ((Image) sender).ToolTip;

            if (trailer != "Not Available") {
                Process.Start(trailer);
            }
        }

        private void ActorsListOnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                TextBox textBox = ((TextBox) e.EditingElement);
                string text = textBox.Text;
                if (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(text) && text.OrdinalEquals(Gettext.T("Unknown")))) {
                    textBox.Text = null;
                }
            }
        }
    }

}