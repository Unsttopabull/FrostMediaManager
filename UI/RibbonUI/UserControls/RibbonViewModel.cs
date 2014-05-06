﻿using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Frost.Common;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Design;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;
using RibbonUI.Windows.Search;
using RibbonUI.Windows.WebUpdate;
using MessageBox = System.Windows.MessageBox;
using SettingsEx = RibbonUI.Properties.Settings;

namespace RibbonUI.UserControls {

    public enum RibbonTabs {
        None,
        Subtitles,
        Search,
        Detect,
        Export
    }

    public enum SubtitleSites {
        PodnapisiNet,
        OpenSubtitles
    }

    internal class RibbonViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;
        private bool _isSearchTabSelected;
        private bool _isSubtitlesTabSelected;
        private bool _isExportTabSelected;
        private bool _isDetectTabSelected;
        private Visibility _subtitlesContextVisible;
        private readonly IMoviesDataService _service;
        private ICommand _saveChangesCommand;
        private ICommand<string> _updateMovieCommand;
        private ICommand _updatePromotionalVideosCommand;
        private ICommand _exportMoviesCommand;
        private ICommand _playMovieCommand;
        private ICommand _downloadSubtitlesCommand;
        private ICommand<string> _updateMovieArtCommand;
        private ICommand _removeMovieCommand;

        public RibbonViewModel() {
            _service = Gettext.IsInDesignMode
                           ? new DesignMoviesDataService()
                           : LightInjectContainer.GetInstance<IMoviesDataService>();

            OpenMovieInFolderCommand = new RelayCommand(OpenInFolder);
            OnRibbonLoadedCommand = new RelayCommand<DependencyObject>(OnRibbonLoaded);
            OptionsCommand = new RelayCommand(MenuItemOptionsOnClick);
            SearchCommand = new RelayCommand(SearchClick);

            SubtitlesContextVisible = Visibility.Collapsed;
        }

        #region ICommands

        public ICommand OpenMovieInFolderCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand OptionsCommand { get; private set; }

        public ICommand<string> UpdateMovieCommand {
            get {
                if (_updateMovieCommand == null) {
                    _updateMovieCommand = new RelayCommand<string>(UpdateMovie, s => !string.IsNullOrEmpty(s));
                }
                return _updateMovieCommand;
            }
            private set { _updateMovieCommand = value; }
        }

        public ICommand<string> UpdateMovieArtCommand {
            get {
                if (_updateMovieArtCommand == null) {
                    _updateMovieArtCommand = new RelayCommand<string>(UpdateMovieArt, s => !string.IsNullOrEmpty(s) && SelectedMovie != null && SelectedMovie["Art"]);
                }
                return _updateMovieArtCommand;
            }
            private set { _updateMovieArtCommand = value; }
        }

        public ICommand<DependencyObject> OnRibbonLoadedCommand { get; private set; }

        public ICommand SaveChangesCommand {
            get {
                if (_saveChangesCommand != null) {
                    return _saveChangesCommand;
                }

                return _saveChangesCommand = new RelayCommand(() => {
                    try {
                        _service.SaveChanges();
                    }
                    catch (Exception e) {
                        UIHelper.HandleProviderException(e);
                    }
                });
            }
            set { _saveChangesCommand = value; }
        }

        public ICommand UpdatePromotionalVideosCommand {
            get {
                if (_updatePromotionalVideosCommand == null) {
                    _updateMovieCommand = new RelayCommand<string>(UpdatePromotionalVideos);
                }
                return _updateMovieCommand;
            }
            set { _updatePromotionalVideosCommand = value; }
        }

        public ICommand ExportMoviesCommand {
            get {
                if (_exportMoviesCommand == null) {
                    _exportMoviesCommand = new RelayCommand(ExportMovies);
                }
                return _exportMoviesCommand;
            }
            set { _exportMoviesCommand = value; }
        }

        public ICommand PlayMovieCommand {
            get {
                if (_playMovieCommand == null) {
                    _playMovieCommand = new RelayCommand(PlayMovie, o => SelectedMovie != null && !string.IsNullOrEmpty(SelectedMovie.FirstFileName));
                }
                return _playMovieCommand;
            }
            set { _playMovieCommand = value; }
        }

        public ICommand DownloadSubtitlesCommand {
            get {
                if (_downloadSubtitlesCommand == null) {
                    _downloadSubtitlesCommand = new RelayCommand<SubtitleSites>(DownloadSubtitles, o => SelectedMovie != null && !string.IsNullOrEmpty(SelectedMovie.DirectoryPath));
                }
                return _downloadSubtitlesCommand;
            }
            set { _downloadSubtitlesCommand = value; }
        }


        #endregion

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;
                OnPropertyChanged();
            }
        }

        public Window ParentWindow { get; set; }

        #region Tab Visisbility

        public bool IsSearchTabSelected {
            get { return _isSearchTabSelected; }
            set {
                if (value.Equals(_isSearchTabSelected)) {
                    return;
                }
                _isSearchTabSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsDetectTabSelected {
            get { return _isDetectTabSelected; }
            set {
                if (value.Equals(_isDetectTabSelected)) {
                    return;
                }
                _isDetectTabSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsExportTabSelected {
            get { return _isExportTabSelected; }
            set {
                if (value.Equals(_isExportTabSelected)) {
                    return;
                }
                _isExportTabSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsSubtitlesTabSelected {
            get { return _isSubtitlesTabSelected; }
            set {
                if (value.Equals(_isSubtitlesTabSelected)) {
                    return;
                }
                _isSubtitlesTabSelected = value;
                OnPropertyChanged();
            }
        }

        public Visibility SubtitlesContextVisible {
            get { return _subtitlesContextVisible; }
            set {
                if (value == _subtitlesContextVisible) {
                    return;
                }
                _subtitlesContextVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand RemoveMovieCommand {
            get {
                if (_removeMovieCommand == null) {
                    _removeMovieCommand = new RelayCommand(RemoveMovie, o => SelectedMovie != null);
                }
                return _removeMovieCommand;
            }
            set { _removeMovieCommand = value; }
        }

        public ICommand DetectSubtitleLanguage { get; set; }

        #endregion

        private void OnRibbonLoaded(DependencyObject uc) {
            if (uc != null) {
                ParentWindow = Window.GetWindow(uc);
            }
        }


        private void RemoveMovie() {
            if (MessageBox.Show(Gettext.T("Do you really want to remove {0}?", "movie"), Gettext.T("Remove {0}", "movie"), MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                _service.RemoveMovie(SelectedMovie.ObservedEntity);
            }
        }

        private void DownloadSubtitles(SubtitleSites site) {
            
        }

        private void ExportMovies() {
            ExportMoviesAsNfo export = new ExportMoviesAsNfo(_service.Movies) { Owner = ParentWindow };
            export.ShowDialog();
        }

        private void PlayMovie() {
            try {
                Process.Start(SelectedMovie.FirstFileName);
            }
            catch (IOException e) {
                MessageBox.Show(Gettext.T("File could not be accessed: " + e.Message));
            }
            catch (Exception e) {
                MessageBox.Show(Gettext.T("An error has occured opening file: "+SelectedMovie.FirstFileName));
            }
        }

        private void OpenInFolder() {
            if (SelectedMovie != null) {
                string directory = SelectedMovie.DirectoryPath;
                if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory)) {
                    try {
                        Process.Start(directory);
                    }
                    catch {
                        MessageBox.Show("Error opening movie folder");
                    }
                }
                else {
                    MessageBox.Show(ParentWindow, Gettext.T("Folder not accessible"));
                }
            }
        }

        public void OnRibbonTabSelect(RibbonTabs tab) {
            IsSearchTabSelected = false;
            IsSubtitlesTabSelected = false;
            IsSearchTabSelected = false;
            IsDetectTabSelected = false;
            IsExportTabSelected = false;
            SubtitlesContextVisible = Visibility.Collapsed;

            switch (tab) {
                case RibbonTabs.None:
                    IsSearchTabSelected = true;
                    break;
                case RibbonTabs.Subtitles:
                    SubtitlesContextVisible = Visibility.Visible;
                    IsSubtitlesTabSelected = true;
                    break;
                case RibbonTabs.Search:
                    IsSearchTabSelected = true;
                    break;
                case RibbonTabs.Detect:
                    IsDetectTabSelected = true;
                    break;
                case RibbonTabs.Export:
                    IsExportTabSelected = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("tab");
            }
        }

        private void SearchClick() {
            SearchSettings ss = new SearchSettings { Owner = ParentWindow };
            if (ss.ShowDialog() == true) {
                Plugin art = null;
                if (ss.SearchArt && ss.ArtPlugin != null) {
                    art = ss.ArtPlugin;
                }

                Plugin info = null;
                if (ss.SearchInfo && ss.InfoPlugin != null) {
                    info = ss.InfoPlugin;
                }

                Plugin videos = null;
                if (ss.SearchVideos && ss.VideoPlugin != null) {
                    videos = ss.VideoPlugin;
                }

                SearchMovies(ss.SearchArt, art, ss.SearchInfo, info, ss.SearchVideos, videos);
            }
        }

        private void SearchMovies(bool searchArt, Plugin art, bool searchInfo, Plugin info, bool searchVideos, Plugin videos) {
            if (SettingsEx.Default.SearchFolders == null) {
                SettingsEx.Default.SearchFolders = new StringCollection();
            }

            if (SettingsEx.Default.SearchFolders.Count > 0) {
                SearchMovies sm = new SearchMovies(searchInfo, searchArt, searchVideos, info, art, videos) {
                    Owner = ParentWindow,
                    TaskbarItemInfo = new TaskbarItemInfo {
                        ProgressState = TaskbarItemProgressState.Indeterminate
                    }
                };

                if (sm.ShowDialog() == true) {
                }
                GC.Collect();
            }
            else {
                MessageBoxResult result = MessageBox.Show(ParentWindow,
                    "No folders to search have been added yet. Please add them in the options menu.\nWould you like to open the options now?", "No folders to search.",
                    MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes) {
                    return;
                }

                SettingsWindow sw = new SettingsWindow { Owner = ParentWindow };
                sw.ShowDialog();
            }
        }

        private void UpdateMovie(string downloader) {
            WebUpdater wu = new WebUpdater(downloader, SelectedMovie) { Owner = ParentWindow };
            wu.ShowDialog();
        }

        private void UpdateMovieArt(string downloader) {
            WebArtUpdater wau = new WebArtUpdater(downloader, SelectedMovie) { Owner = ParentWindow };
            wau.ShowDialog();
        }

        private void UpdatePromotionalVideos(string downloader) {
            WebPromoVideoUpdater wpvu = new WebPromoVideoUpdater(downloader, SelectedMovie) { Owner = ParentWindow };
            wpvu.ShowDialog();
        }

        private void MenuItemOptionsOnClick() {
            SettingsWindow sw = new SettingsWindow { Owner = ParentWindow };
            sw.ShowDialog();
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