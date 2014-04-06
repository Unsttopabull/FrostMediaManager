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
using Frost.Common.Properties;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using RibbonUI.Messages;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;
using SettingsEx = RibbonUI.Properties.Settings;

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
                if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory)) {
                    Process.Start(directory);
                }
                else {
                    MessageBox.Show(ParentWindow, TranslationManager.T("Folder not accessible"));
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
            if (SettingsEx.Default.SearchFolders == null) {
                SettingsEx.Default.SearchFolders = new StringCollection();
            }

            if (SettingsEx.Default.SearchFolders.Count > 0) {
                SearchMovies sm = new SearchMovies { 
                    Owner = ParentWindow,
                    TaskbarItemInfo =  new TaskbarItemInfo {
                        ProgressState = TaskbarItemProgressState.Indeterminate
                    }
                };

                if (sm.ShowDialog() == true) {
                    
                }
                GC.Collect();
            }
            else {
                MessageBoxResult result = MessageBox.Show(ParentWindow, "No folders to search have been added yet. Please add them in the options menu.\nWould you like to open the options now?", "No folders to search.", MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes) {
                    return;
                }

                SettingsWindow sw = new SettingsWindow { Owner = ParentWindow };
                sw.ShowDialog();
            }
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
