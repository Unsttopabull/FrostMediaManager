using System.Windows;
using System.Windows.Controls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for MovieFlagsAndInfo.xaml</summary>
    public partial class MovieFlagsAndInfo : UserControl {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register(
            "MinRequiredWidth", typeof(double), typeof(MovieFlagsAndInfo), new PropertyMetadata(default(double)));

        public MovieFlagsAndInfo() {
            InitializeComponent();
        }

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e) {
            MinRequiredWidth = MovieFlags.RenderSize.Width + MovieInfo.RenderSize.Width;
        }
    }
}
