using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Subtitles;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Util.WebUpdate;

namespace RibbonUI.Windows.WebUpdate {

    /// <summary>Interaction logic for WebSubtitleUpdater.xaml</summary>
    public partial class WebSubtitleUpdater : Window {
        private readonly IEnumerable<string> _languages;

        public WebSubtitleUpdater(string downloader, ObservableMovie movie, IEnumerable<string> languages) {
            _languages = languages;
            ISubtitleClient cli = LightInjectContainer.TryGetInstance<ISubtitleClient>(downloader);
            if (cli == null) {
                MessageBox.Show(Gettext.T("Error accessing subtitle provider."));
                return;
            }

            DataContext = new SubtitleUpdater(cli, movie);
            Loaded += OnWindowLoaded;

            InitializeComponent();
        }

        public IEnumerable<ISubtitleInfo> SubtitleInfos { get; private set; }

        private async void OnWindowLoaded(object sender, RoutedEventArgs e) {
            SubtitleInfos = await Update();

            if (SubtitleInfos != null) {
                DialogResult = true;
            }

            Close();
        }

        private async Task<IEnumerable<ISubtitleInfo>> Update() {
            IEnumerable<ISubtitleInfo> subs;
            try {
                subs = await ((SubtitleUpdater) DataContext).Update(_languages);
            }
            catch (Exception e) {
                UIHelper.HandleProviderException(e);
                subs = null;
            }

            return subs;
        }

        private void WebUpdaterOnClosed(object sender, EventArgs e) {
            
        }
    }
}
