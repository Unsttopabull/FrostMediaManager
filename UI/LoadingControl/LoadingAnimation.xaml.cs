using System.Windows;
using System.Windows.Controls;

namespace LoadingControl {

    /// <summary>Interaction logic for LoadingAnimation.xaml</summary>
    public partial class LoadingAnimation : UserControl {
        public static readonly DependencyProperty LoadingTextProperty = DependencyProperty.Register("LoadingText", typeof(string), typeof(LoadingAnimation), new PropertyMetadata("Loading..."));

        public LoadingAnimation() {
            InitializeComponent();
        }

        public string LoadingText {
            get { return (string) GetValue(LoadingTextProperty); }
            set { SetValue(LoadingTextProperty, value); }
        }
    }

}