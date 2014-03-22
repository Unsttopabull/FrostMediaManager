using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Messages.Subtitles;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {

    public class ListSubtitlesViewModel : ViewModelBase {
        private readonly IMoviesDataService _service;
        private ObservableCollection<MovieSubtitle> _subtitles;
        private ICollectionView _collectionView;

        public ListSubtitlesViewModel() {
            _service = LightInjectContainer.GetInstance<IMoviesDataService>();

            //IEnumerable<ILanguage> enumerable = _service.Languages;

            SubtitleFormats = new ObservableCollection<string> {
                "Adobe encore DVD",
                "Advanced Substation Alpha",
                "AQTitle",
                "ASS",
                "Captions Inc",
                "Cheeta",
                "Cheetah",
                "CPC Captioning",
                "CPC-600",
                "EBU Subtitling Format",
                "N19",
                "SAMI",
                "Sami Captioning",
                "SSA",
                "SubRip",
                "SubStation Alpha",
                "VobSub"
            };

            ChangeLanguageCommand = new RelayCommand<MovieSubtitle>(LangEdit);
            RemoveCommand = new RelayCommand<MovieSubtitle>(OnRemoveClicked, s => s != null);
        }

        public ObservableCollection<MovieSubtitle> Subtitles {
            get { return _subtitles; }
            set {
                if (Equals(value, _subtitles)) {
                    return;
                }
                _subtitles = value;

                if (_subtitles != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_subtitles);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
                    if (_collectionView.GroupDescriptions != null) {
                        _collectionView.GroupDescriptions.Add(groupDescription);
                    }
                }

                OnPropertyChanged();
            }
        }

        public Window ParentWindow { get; set; }

        public ObservableCollection<string> SubtitleFormats { get; set; }

        public RelayCommand<MovieSubtitle> ChangeLanguageCommand { get; private set; }
        public RelayCommand<MovieSubtitle> RemoveCommand { get; private set; }


        private void OnRemoveClicked(MovieSubtitle subtitle) {
            RemoveSubtitleMessage msg = new RemoveSubtitleMessage(subtitle);
            MessengerInstance.Send(msg);
        }

        private void LangEdit(MovieSubtitle subtitle) {
            SelectLanguage sc = new SelectLanguage {
                Owner = ParentWindow,
                Languages = _service.Languages.Select(l => new MovieLanguage(l))
            };

            if (subtitle.Language != null) {
                sc.SelectedLanguage.SelectedItem = subtitle.Language;
            }

            if (sc.ShowDialog() != true) {
                return;
            }

            subtitle.Language = (ILanguage) sc.SelectedLanguage.SelectedItem;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChangedHandler != null) {
                PropertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}