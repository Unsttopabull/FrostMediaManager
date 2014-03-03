using System;
using System.Windows;
using System.Windows.Controls;
using Frost.Models.Frost.DB;
using RibbonUI.UserControls.ViewModels;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ArtAndPlot.xaml</summary>
    public partial class ArtAndPlot : UserControl {
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(Movie), typeof(ArtAndPlot), new PropertyMetadata(default(Movie), SelectedMovieChanged));

        public ArtAndPlot() {
            InitializeComponent();
        }

        private static void SelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((ArtAndPlotViewModel) ((ArtAndPlot) d).DataContext).SelectedMovie = (Movie) args.NewValue;
        }

        public Movie SelectedMovie {
            get { return (Movie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }
    }

}