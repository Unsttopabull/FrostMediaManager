using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for EditMovie.xaml</summary>
    public partial class EditMovie : UserControl {
        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(Movie), typeof(EditMovie),
            new FrameworkPropertyMetadata(default(Movie), FrameworkPropertyMetadataOptions.AffectsRender));

        public EditMovie() {
            InitializeComponent();
        }

        public Movie Movie {
            get { return (Movie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }
    }

}