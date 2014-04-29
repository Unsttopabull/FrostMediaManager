using System;
using System.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for WebArtUpdater.xaml</summary>
    public partial class WebArtUpdater : Window {
        private bool _shown;

        public WebArtUpdater() {
            ContentRendered += OnContentRendered;

            InitializeComponent();
        }

        public Visibility CloseButtonVisibility { get; private set; }

        public string ProgressText { get; set; }

        public string LabelText { get; set; }

        private void OnContentRendered(object sender, EventArgs e) {
            if (_shown) {
                return;
            }

            _shown = true;
            Update();
        }

        private void Update() {
            
        }
    }
}
