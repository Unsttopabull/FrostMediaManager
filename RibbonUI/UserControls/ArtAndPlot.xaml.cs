using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ArtAndPlot.xaml</summary>
    public partial class ArtAndPlot : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(IMovie), typeof(ArtAndPlot), new PropertyMetadata(default(IMovie), SelectedMovieChanged));

        public ArtAndPlot() {
            InitializeComponent();
        }

        private static void SelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ArtAndPlotViewModel) ((ArtAndPlot) d).DataContext).SelectedMovie = (IMovie) args.NewValue;
        }

        public IMovie SelectedMovie {
            get { return (IMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }
    }

}