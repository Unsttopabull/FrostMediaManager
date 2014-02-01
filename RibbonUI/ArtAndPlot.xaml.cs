using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for ArtAndPlot.xaml</summary>
    public partial class ArtAndPlot : UserControl {
        private const string IMDB_PERSON_URI = "http://www.imdb.com/name/nm{0}";
        private const string IMDB_TITLE_URI = "http://www.imdb.com/title/{0}";

        private static readonly FrameworkPropertyMetadata MoviePropertyMetadata = new FrameworkPropertyMetadata(default(Movie), FrameworkPropertyMetadataOptions.AffectsRender);
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(Movie), typeof(ArtAndPlot), MoviePropertyMetadata);

        public ArtAndPlot() {
            InitializeComponent();
        }

        public Movie Movie {
            get { return (Movie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
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
                if (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(text) && text.OrdinalEquals("Unknown"))) {
                    textBox.Text = null;
                }
            }
        }
    }

}