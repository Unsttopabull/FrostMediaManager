using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI {

    /// <summary>Interaction logic for MovieFlagsAndInfo.xaml</summary>
    public partial class MovieFlagsAndInfo : UserControl {
        public static readonly DependencyProperty MovieProperty = 
            DependencyProperty.Register("Movie", typeof(Movie), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(Movie)));

        public MovieFlagsAndInfo() {
            InitializeComponent();
        }

        public Movie Movie {
            get { return (Movie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        public double MinRequiredWidth {
            get { return (MovieFlags.ActualWidth + MovieFlags.Margin.Left) + (MovieInfo.ActualWidth + MovieInfo.Margin.Left + MovieInfo.Margin.Right); }
        }
    }
}
