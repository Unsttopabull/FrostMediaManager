using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Media;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.ProcessDatabase;
using File = Frost.Common.Models.DB.MovieVo.Files.File;

namespace Frost.UI {
    /// <summary>
    /// Interaction logic for OpisFilma.xaml
    /// </summary>
    public partial class OpisFilma {
        private MovieVoContainer _xjb;
        private string _dbLoc;

        public OpisFilma() {
            InitializeComponent();
            InitXjb();
        }

        private void InitXjb() {
            if (_xjb == null) {
                _xjb = new MovieVoContainer();
                _dbLoc = DBCheck.FindXjbDriveLoc();
            }
        }

        /// <summary>Attaches events and names to compiled content. </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target) {
            throw new NotImplementedException();
        }

        private void TryLoadCover(string fp, string fn) {
            if (String.IsNullOrEmpty(fp) || String.IsNullOrEmpty(fn)) {
                return;
            }

            string slika = _dbLoc + fp + @"/" + fn + "_xjb_cover.jpg";

            bool cover = false;
            try {
                MovieCover.SetAnimatedSource(slika);
                cover = true;
            }
            catch (DirectoryNotFoundException) {
                Console.WriteLine(@"Pot do datoteke ni veljavna");
            }
            catch (UriFormatException) {
                Console.WriteLine(@"URI format napačen ali pot nedostopna");
            }
            catch (Exception ex) {
                if (ex is WebException || ex is FileNotFoundException) {
                    Console.WriteLine(@"Datoteka ni dostopna");
                }
                else {
                    throw;
                }
            }
            finally {
                if (!cover) {
                    Console.WriteLine(@"No Cover found");
                    MovieCover.SetAnimatedSource("pack://application:,,,/WPF_Jukebox;component/Images/NoCover.png");
                }
            }
        }


        private void UserControlDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
            MovieCover.SetAnimatedSource((ImageSource) Application.Current.Resources["Loading"]);

            Movie mv = e.NewValue as Movie;
            if (mv == null) {
                return;
            }

            var movieFiles = _xjb.Movies.Where(m => m.Id == mv.Id).Select(m => m.Files).FirstOrDefault();
            if (movieFiles != null) {
                File file = movieFiles.FirstOrDefault();
                if (file != null) {
                    TryLoadCover(file.FolderPath, file.Name.WithoutExtension());
                }
            }
        }
    }
}
