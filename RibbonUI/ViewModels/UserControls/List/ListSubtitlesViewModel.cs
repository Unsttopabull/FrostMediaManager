﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Messages.Subtitles;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.UserControls.List {

    public class ListSubtitlesViewModel : ViewModelBase {
        private readonly IMoviesDataService _service;
        private ObservableCollection<ISubtitle> _subtitles;
        private ICollectionView _collectionView;

        public ListSubtitlesViewModel(IMoviesDataService service) {
            _service = service;
            IEnumerable<ILanguage> enumerable = _service.Languages;

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

            ChangeLanguageCommand = new RelayCommand<ISubtitle>(LangEdit);
            RemoveCommand = new RelayCommand<ISubtitle>(OnRemoveClicked, s => s != null);
        }

        public ObservableCollection<ISubtitle> Subtitles {
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

        public RelayCommand<ISubtitle> ChangeLanguageCommand { get; private set; }
        public RelayCommand<ISubtitle> RemoveCommand { get; private set; }


        private void OnRemoveClicked(ISubtitle subtitle) {
            RemoveSubtitleMessage msg = new RemoveSubtitleMessage(subtitle);
            MessengerInstance.Send(msg);
        }

        private void LangEdit(ISubtitle subtitle) {
            SelectLanguage sc = new SelectLanguage {
                Owner = ParentWindow,
                Languages = _service.Languages
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