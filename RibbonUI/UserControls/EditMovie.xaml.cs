using System.Windows;
using System.Windows.Controls;
using Frost.Common;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using GalaSoft.MvvmLight.Ioc;
using RibbonUI.ViewModels.UserControls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for EditMovie.xaml</summary>
    public partial class EditMovie : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(IMovie), typeof(EditMovie), new PropertyMetadata(default(IMovie), OnSelectedMovieChanged));

        public EditMovie() {
            InitializeComponent();
            DataContext = SimpleIoc.Default.GetInstance<EditMovieViewModel>();
        }

        public IMovie SelectedMovie {
            get { return (IMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private EditMovieViewModel ViewModel {
            get { return DataContext as EditMovieViewModel; }
        }

        private static void OnSelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditMovie) d).ViewModel.SelectedMovie = (IMovie) e.NewValue;
        }

         private void OnControlLoaded(object sender, RoutedEventArgs e) {
            ViewModel.ParentWindow = Window.GetWindow(this);
        }

        private void CbMovieGenreCheckBoxLoaded(object sender, RoutedEventArgs e) {
            CheckBox cb = (CheckBox) sender;
            if (cb == null) {
                return;
            }

            IGenre g = (IGenre) cb.DataContext;
            if (ViewModel.Genres.Contains(g)) {
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