using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for WebArtUpdater.xaml</summary>
    public partial class WebArtUpdater : Window {
        public WebArtUpdater() {
            InitializeComponent();
        }

        public Visibility CloseButtonVisibility { get; private set; }

        public string ProgressText { get; set; }

        public string LabelText { get; set; }
    }
}
