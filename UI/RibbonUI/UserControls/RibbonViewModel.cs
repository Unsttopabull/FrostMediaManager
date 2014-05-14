using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Frost.Common;
using Frost.GettextMarkupExtension;
using Frost.RibbonUI.Design;
using Frost.RibbonUI.Properties;
using Frost.RibbonUI.Util;
using Frost.RibbonUI.Util.ObservableWrappers;
using Frost.RibbonUI.Util.WebUpdate;
using Frost.RibbonUI.Windows;
using Frost.RibbonUI.Windows.Add;
using Frost.RibbonUI.Windows.Search;
using Frost.RibbonUI.Windows.WebUpdate;
using Frost.XamlControls.Commands;
using log4net;

namespace Frost.RibbonUI.UserControls {

    public enum RibbonTabs {
        None,
        Subtitles,
        Search,
        Detect,
        Export
    }

    internal class RibbonViewModel : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RibbonViewModel));
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

            try {
                _service = Gettext.IsInDesignMode
                               ? new DesignMoviesDataService()
                               : LightInjectContainer.GetInstance<IMoviesDataService>();
            }
            catch (Exception e) {
                MessageBox.Show(Gettext.T("Failed to access provider service"));

                if (Log.IsFatalEnabled) {
                    Log.Fatal("Failed to access provider service", e);
                }
                Application.Current.Shutdown();
                return;
            }

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
                        UIHelper.HandleProviderException(Log, e);
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
                    _playMovieCommand = new RelayCommand(PlayMovie,
                        o => SelectedMovie != null && SelectedMovie["FirstFileName"] && !string.IsNullOrEmpty(SelectedMovie.FirstFileName));
                }
                return _playMovieCommand;
            }
            set { _playMovieCommand = value; }
        }

        public ICommand DownloadSubtitlesCommand {
            get {
                if (_downloadSubtitlesCommand == null) {
                    _downloadSubtitlesCommand = new RelayCommand<string>(DownloadSubtitles, o => SelectedMovie != null && !string.IsNullOrEmpty(SelectedMovie.DirectoryPath));
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

        private void ExportMovies() {
            ExportMoviesAsNfo export = new ExportMoviesAsNfo(_service.Movies) { Owner = ParentWindow };
            export.ShowDialog();
        }

        private void PlayMovie() {
            try {
                Process.Start(SelectedMovie.FirstFileName);
            }
            catch (IOException e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("File \"{0}\" of movie {1} could not be accessed to play.", SelectedMovie.FirstFileName, SelectedMovie.Title));
                }

                MessageBox.Show(Gettext.T("File could not be accessed: ") + e.Message);
            }
            catch (Exception e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Unknown error occured while accessing file \"{0}\" of movie {1} to play.", SelectedMovie.FirstFileName, SelectedMovie.Title), e);
                }

                MessageBox.Show(Gettext.T("An error has occured opening file: ") + SelectedMovie.FirstFileName);
            }
        }

        private void OpenInFolder() {
            if (SelectedMovie != null) {
                string directory = SelectedMovie.DirectoryPath;
                if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory)) {
                    try {
                        Process.Start(directory);
                    }
                    catch (Exception e) {
                        if (Log.IsWarnEnabled) {
                            Log.Warn(string.Format("Could not open movie folder with path: \"{0}\" of movie \"{1}\".", directory, SelectedMovie.Title), e);
                        }

                        MessageBox.Show(Gettext.T("Error opening movie folder"));
                    }
                }
                else {
                    MessageBox.Show(ParentWindow, Gettext.T("Folder not accessible or doesn't exists."));
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
            if (global::Frost.RibbonUI.Properties.Settings.Default.SearchFolders == null) {
                global::Frost.RibbonUI.Properties.Settings.Default.SearchFolders = new StringCollection();
            }

            if (global::Frost.RibbonUI.Properties.Settings.Default.SearchFolders.Count > 0) {
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
                    Gettext.T("No folders to search have been added yet. Please add them in the options menu.\nWould you like to open the options now?"),
                    Gettext.T("No folders to search."),
                    MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes) {
                    return;
                }

                SettingsWindow sw = new SettingsWindow { Owner = ParentWindow };
                sw.ShowDialog();
            }
        }

        private async void DownloadSubtitles(string downloader) {
            SelectLanguages sl = new SelectLanguages() { Owner = ParentWindow };
            if (sl.ShowDialog() == true) {
                List<string> languages = sl.Languages.Select(l => l.ISO3166.Alpha3).ToList();
                if (languages.Count == 0) {
                    if (MessageBox.Show(Gettext.T("No languages selected, search all?"), Gettext.T("No languages selected"), MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                        return;
                    }
                    languages = null;
                }

                WebSubtitleUpdater wsu = new WebSubtitleUpdater(downloader, SelectedMovie, languages) { Owner = ParentWindow };
                if (wsu.ShowDialog() != true) {
                    return;
                }

                SelectSubtitles ss = new SelectSubtitles(wsu.SubtitleInfos) { Owner = ParentWindow };
                if (ss.ShowDialog() == true) {
                    ProgressIndicator pi = new ProgressIndicator { Owner = ParentWindow };
                    pi.Show();

                    SubtitleDownloader sd = new SubtitleDownloader(ss.SubtitleInfo, SelectedMovie);
                    await sd.Download();

                    pi.Close();
                }
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