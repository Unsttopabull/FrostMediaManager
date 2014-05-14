using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.InfoParsers.Models.Subtitles;
using Frost.XamlControls.Commands;

namespace Frost.RibbonUI.Windows {
    /// <summary>Interaction logic for SelectSubtitles.xaml</summary>
    public partial class SelectSubtitles : Window {
        private IEnumerable<ISubtitleInfo> _subtitles;
        private ICollectionView _collectionView;
        private ICommand _downloadSubtitleCommand;

        public SelectSubtitles(IEnumerable<ISubtitleInfo> subtitles) {
            Subtitles = subtitles;
            InitializeComponent();
        }

        public ISubtitleInfo SubtitleInfo { get; private set; }

        public IEnumerable<ISubtitleInfo> Subtitles {
            get { return _subtitles; }
            set {
                _subtitles = value;
                if (_subtitles != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_subtitles);

                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("LanguageName");
                    if (_collectionView.GroupDescriptions != null) {
                        _collectionView.GroupDescriptions.Add(groupDescription);
                        _collectionView.SortDescriptions.Add(new SortDescription("DownloadCount", ListSortDirection.Descending));
                    } 
                }
            }
        }

        public ICommand DownloadSubtitleCommand {
            get {
                if (_downloadSubtitleCommand == null) {
                    _downloadSubtitleCommand = new RelayCommand<ISubtitleInfo>(DownloadSubtitle, s => s != null);
                }
                return _downloadSubtitleCommand;
            }
            set { _downloadSubtitleCommand = value; }
        }

        private void DownloadSubtitle(ISubtitleInfo sub) {
            SubtitleInfo = sub;

            DialogResult = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
