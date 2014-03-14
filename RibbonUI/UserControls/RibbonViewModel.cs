using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Messages;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;

namespace RibbonUI.UserControls {

    public enum RibbonTabs {
        None,
        Subtitles,
        Search,
        Detect,
        Export
    }

    class RibbonViewModel : ViewModelBase {
        private ObservableMovie _selectedMovie;
        private bool _isSearchTabSelected;
        private bool _isSubtitlesTabSelected;
        private bool _isExportTabSelected;
        private bool _isDetectTabSelected;

        public RibbonViewModel() {
            OpenMovieInFolderCommand = new RelayCommand(OpenInFolder);
            OnRibbonLoadedCommand = new RelayCommand<DependencyObject>(OnRibbonLoaded);
            OptionsCommand = new RelayCommand(MenuItemOptionsOnClick);
            SearchCommand = new RelayCommand(SearchClick);

            MessengerInstance.Register<SelectRibbonMessage>(this, rts => OnRibbonTabSelect(rts.RibbonTab));
        }

        #region ICommands

        public ICommand OpenMovieInFolderCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand OptionsCommand { get; private set; }
        public ICommand<DependencyObject> OnRibbonLoadedCommand { get; private set; }

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


        #endregion

        private void OnRibbonLoaded(DependencyObject uc) {
            if (uc != null) {
                ParentWindow = Window.GetWindow(uc);
            }
        }

        private void OpenInFolder() {
            if (SelectedMovie != null) {
                string directory = SelectedMovie.DirectoryPath;
                if (!string.IsNullOrEmpty(directory)) {
                    Process.Start(directory);
                }
            }
        }

        private void OnRibbonTabSelect(RibbonTabs tab) {
            switch (tab) {
                case RibbonTabs.None:
                    IsSearchTabSelected = true;
                    break;
                case RibbonTabs.Subtitles:
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
            //using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
            //    int count = mvc.Movies.Count();
            //}

            //TestWindow tw = new TestWindow();
            //Debug.Listeners.Add(tw.Listener);

            //tw.Owner = Window.GetWindow(this);
            //tw.ShowDialog();

            //((MainWindow)((Grid)Parent).Parent).ContentGrid.;
        }

        private void MenuItemOptionsOnClick() {
            SettingsWindow sw = new SettingsWindow { Owner = ParentWindow };
            sw.ShowDialog();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChangedHandler != null) {
                PropertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
