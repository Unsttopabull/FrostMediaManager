using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models.Subtitles;
using Frost.RibbonUI.Util;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.RibbonUI.Util.WebUpdate;
using log4net;

namespace Frost.RibbonUI.Windows.WebUpdate {

    /// <summary>Interaction logic for WebSubtitleUpdater.xaml</summary>
    public partial class WebSubtitleUpdater : Window {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebSubtitleUpdater));
        private readonly IEnumerable<string> _languages;

        public WebSubtitleUpdater(string downloader, ObservableMovie movie, IEnumerable<string> languages) {
            _languages = languages;
            ISubtitleClient cli = LightInjectContainer.TryGetInstance<ISubtitleClient>(downloader);
            if (cli == null) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Failed to instantie ISubtitleClient with service name: \"{0}\".", downloader));
                }

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
                UIHelper.HandleProviderException(Log, e);
                subs = null;
            }

            return subs;
        }

        private void WebUpdaterOnClosed(object sender, EventArgs e) {
            
        }
    }
}
