using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows.Add;

namespace RibbonUI.UserControls.List {

    public class ListSubtitlesViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IMoviesDataService _service;
        private ICollectionView _collectionView;
        private ObservableMovie _selectedMovie;

        public ListSubtitlesViewModel() {
            _service = Gettext.IsInDesignMode
                ? new DesignMoviesDataService()
                : LightInjectContainer.GetInstance<IMoviesDataService>();

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
            RemoveCommand = new RelayCommand<MovieSubtitle>(subtitle => SelectedMovie.RemoveSubtitle(subtitle), s =>  SelectedMovie != null && s != null);
        }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_selectedMovie.Subtitles);
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

        private void LangEdit(MovieSubtitle subtitle) {
            IEnumerable<ILanguage> languages = _service.Languages ?? UIHelper.GetLanguages();

            SelectLanguage sc = new SelectLanguage {
                Owner = ParentWindow,
                Languages =  languages.Select(l => new MovieLanguage(l))
            };

            if (sc.ShowDialog() != true) {
                return;
            }

            subtitle.Language = ((MovieLanguage) sc.SelectedLanguage.SelectedItem).ObservedEntity;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}