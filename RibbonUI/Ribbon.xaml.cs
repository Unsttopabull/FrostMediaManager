using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(Movie), typeof(Ribbon),
            new FrameworkPropertyMetadata(default(Movie)));

        public Movie Movie {
            get { return (Movie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        public Ribbon() {
            InitializeComponent();
        }

        private void OpenInFolder_Click(object sender, RoutedEventArgs e) {
            if (Movie != null) {
                string directory = Movie.DirectoryPath;
                if (!string.IsNullOrEmpty(directory)) {
                    Process.Start(directory);
                }
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e) {
            using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
                int count = mvc.Movies.Count();
            }

            TestWindow tw = new TestWindow();
            Debug.Listeners.Add(tw.Listener);

            tw.Owner = Window.GetWindow(this);
            tw.ShowDialog();
        }
    }

}