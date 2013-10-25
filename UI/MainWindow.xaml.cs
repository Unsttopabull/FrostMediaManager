using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Common.Models.DB.MovieVo;

namespace WPF_Jukebox {
    using System.Windows.Controls;

    public partial class MainWindow {
        private readonly NotifyIcon _ni;
        private MovieVoContainer _xjb;


        public MainWindow() {
            InitializeComponent();

            InitXjb();
            _ni = new NotifyIcon();
        }

        private void InitXjb() {
            if (_xjb == null) {
                _xjb = new MovieVoContainer();

            }
        }

        private IEnumerable<Movie> Movies{
            get {
                InitXjb();

                Movies filmi = new Movies();
                filmi.AddRange(_xjb.Movie.Select(movie => movie));

                return filmi;
            }
        }

        private void BallonTip(int rowIndex) {
            _ni.BalloonTipText = string.Format("Event {0}", rowIndex);
            _ni.Text = @"Event";
            _ni.Icon = Properties.Resources.xtreamer;
            _ni.Visible = true;
            _ni.ShowBalloonTip(100);
        }



        /// <summary>Attaches events and names to compiled content. </summary>
        /// <param name="connectionId">An identifier token to distinguish calls.</param><param name="target">The target to connect events and names to.</param>
        public void Connect(int connectionId, object target) {
            throw new NotImplementedException();
        }

        private void MoviesListOnLoadingRowDetails(object sender, DataGridRowDetailsEventArgs e) {
            //DataGrid dg = (DataGrid)sender;
            //Movie mv = (Movie)dg.SelectedItem;

            //long id = mv.Id;

            //var movieFiles = _xjb.Movie.Select(m => m.File).FirstOrDefault();
            //if (movieFiles != null) {
            //    FileVo file = movieFiles.FirstOrDefault();
            //    if (file != null) {
            //        TryLoadCover(file.PathOnDrive, file.Name.WithoutExtension());
            //    }
            //}

            //BallonTip(dg.SelectedIndex);
        }

        private void ButtonClick(object sender, RoutedEventArgs e) {
            Reload r = new Reload();
            r.ShowDialog();
        }

        private void FilmiOnInitialized(object sender, EventArgs e) {
            ListBox lb = (ListBox) sender;
            lb.ItemsSource = Movies;
        }
    }
}
