using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using RibbonUI.ViewModels.UserControls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(IMovie), typeof(Ribbon), new PropertyMetadata(default(IMovie), OnMovieChanged));

        public Ribbon() {
            InitializeComponent();
        }

        public IMovie Movie {
            get { return (IMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((RibbonViewModel) ((Ribbon) d).DataContext).SelectedMovie = (IMovie) e.NewValue;
        }
    }

}