using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using Frost.Models.Frost.DB;
using RibbonUI.ViewModels.UserControls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for MovieFlagsAndInfo.xaml</summary>
    public partial class MovieFlagsAndInfo : UserControl {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register("MinRequiredWidth", typeof(double), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(double)));
        public static readonly DependencyProperty SelectedMovieProperty = DependencyProperty.Register("SelectedMovie", typeof(IMovie), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(IMovie), SelectedMovieChanged));

        public MovieFlagsAndInfo() {
            InitializeComponent();
        }

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }
      
        public IMovie SelectedMovie {
            get { return (IMovie) GetValue(SelectedMovieProperty); }
            set { SetValue(SelectedMovieProperty, value); }
        }

        private static void SelectedMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) {
            ((MovieFlagsAndInfoViewModel) ((MovieFlagsAndInfo) d).DataContext).SelectedMovie = (IMovie) args.NewValue;
        }
    }
}
