using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Frost.Common;
using Frost.GettextMarkupExtension;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for EditMovie.xaml</summary>
    public partial class EditMovie : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(ObservableMovie), typeof(EditMovie), new PropertyMetadata(default(ObservableMovie), OnSelectedMovieChanged));

        public EditMovie() {
            InitializeComponent();
        }

        public ObservableMovie SelectedMovie {
            get { return (ObservableMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private EditMovieViewModel ViewModel {
            get { return DataContext as EditMovieViewModel; }
        }

        private static void OnSelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditMovie) d).ViewModel.SelectedMovie = (ObservableMovie) e.NewValue;
        }

        private void OnControlLoaded(object sender, RoutedEventArgs e) {
            ViewModel.ParentWindow = Window.GetWindow(this);
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

        private void OnAddPlotClick(object sender, RoutedEventArgs e) {
            EditMovieViewModel viewModel = DataContext as EditMovieViewModel;
            if (viewModel == null) {
                return;
            }

            Task.Run(() => {
                while (Dispatcher.Invoke(() => viewModel.IsAddingPlot)) {
                }
            }).ContinueWith(t => Dispatcher.Invoke(() => MoviePlotCombo.SelectedIndex = MoviePlotCombo.Items.Count - 1));
        }
    }

}