using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Frost.Common;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost.DB;
using RibbonUI.ViewModels.UserControls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for EditMovie.xaml</summary>
    public partial class EditMovie : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(Movie), typeof(EditMovie), new PropertyMetadata(default(Movie), OnSelectedMovieChanged));

        public EditMovie() {
            InitializeComponent();
        }

        public Movie SelectedMovie {
            get { return (Movie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private EditMovieViewModel ViewModel {
            get { return DataContext as EditMovieViewModel; }
        }

        private static void OnSelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditMovie) d).ViewModel.SelectedMovie = (Movie) e.NewValue;
        }

         private void OnControlLoaded(object sender, RoutedEventArgs e) {
            ViewModel.ParentWindow = Window.GetWindow(this);
        }

        private void CbMovieGenreCheckBoxLoaded(object sender, RoutedEventArgs e) {
            CheckBox cb = (CheckBox) sender;
            if (cb == null) {
                return;
            }

            Genre g = (Genre) cb.DataContext;
            if (ViewModel.SelectedMovie.Genres.Contains(g)) {
                cb.IsChecked = true;
            }
        }

        private void ActorsListOnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            if (e.EditAction == DataGridEditAction.Commit) {
                TextBox textBox = ((TextBox) e.EditingElement);
                string text = textBox.Text;
                if (string.IsNullOrEmpty(text) || (!string.IsNullOrEmpty(text) && text.OrdinalEquals(TranslationManager.T("Unknown")))) {
                    textBox.Text = null;
                }
            }
        }
    }

}