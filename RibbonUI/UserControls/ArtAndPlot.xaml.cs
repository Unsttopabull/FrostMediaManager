using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ArtAndPlot.xaml</summary>
    public partial class ArtAndPlot : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(ObservableMovie), typeof(ArtAndPlot), new PropertyMetadata(default(ObservableMovie), SelectedMovieChanged));

        public ArtAndPlot() {
            InitializeComponent();
        }

        private static void SelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ArtAndPlotViewModel) ((ArtAndPlot) d).DataContext).SelectedMovie = (ObservableMovie) args.NewValue;
        }

        public ObservableMovie SelectedMovie {
            get { return (ObservableMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }
    }

}