using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using Microsoft.WindowsAPICodePack.Dialogs;
using RibbonUI.Annotations;
using ComboBox = System.Windows.Controls.ComboBox;

namespace RibbonUI.UserControls.Settings {

    public class GeneralSettingsViewModel : INotifyPropertyChanged {
        private ObservableCollection<string> _searchFolders;
        public event PropertyChangedEventHandler PropertyChanged;

        public GeneralSettingsViewModel() {
            LangSelectLoadedCommand = new RelayCommand<ComboBox>(LangSelectOnLoaded);
            LangSelectionChangedCommand = new RelayCommand<CultureInfo>(LangSelectionChanged, ci => ci != null);

            RemoveMovieFolderCommand = new RelayCommand<string>(movieFolder => SearchFolders.Remove(movieFolder), folder => !string.IsNullOrEmpty(folder));
            AddMovieFolderCommand = new RelayCommand(AddMovieFolderClicked);
        }

        public Window ParentWindow { get; set; }

        public ICommand<CultureInfo> LangSelectionChangedCommand { get; private set; }
        public ICommand<ComboBox> LangSelectLoadedCommand { get; private set; }

        public ICommand<string> RemoveMovieFolderCommand { get; private set; }
        public ICommand AddMovieFolderCommand { get; private set; }

        public ObservableCollection<string> SearchFolders {
            get {
                if (_searchFolders == null) {
                    if (Properties.Settings.Default.SearchFolders == null) {
                        Properties.Settings.Default.SearchFolders = new StringCollection();
                    }

                    _searchFolders = new ObservableCollection<string>(Properties.Settings.Default.SearchFolders.Cast<string>());
                }
                return _searchFolders;
            }
            set {
                _searchFolders = value;
                OnPropertyChanged();
            }
        }

        private void AddMovieFolderClicked() {
            string folderPath;
            if (CommonFileDialog.IsPlatformSupported) {
                //Vista+

                using (CommonOpenFileDialog cfd = new CommonOpenFileDialog {
                    EnsureReadOnly = true,
                    IsFolderPicker = true,
                    AllowNonFileSystemItems = true,
                    Multiselect = false
                }) {
                    if (cfd.ShowDialog(ParentWindow) != CommonFileDialogResult.Ok) {
                        return;
                    }
                    folderPath = cfd.FileName;
                }
            }
            else {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog()) {
                    if (fbd.ShowDialog() != DialogResult.OK) {
                        return;
                    }
                    folderPath = fbd.SelectedPath;
                }
            }

            if (Properties.Settings.Default.SearchFolders == null) {
                Properties.Settings.Default.SearchFolders = new StringCollection();
            }

            SearchFolders.Add(folderPath);
        }

        private void LangSelectionChanged(CultureInfo selectedCulture) {
            TranslationManager.CurrentCulture = selectedCulture;
        }

        private void LangSelectOnLoaded(ComboBox cb) {
            List<CultureInfo> itemsSource = cb.ItemsSource as List<CultureInfo>;
            if (itemsSource == null) {
                return;
            }

            int idx = itemsSource.IndexOf(Thread.CurrentThread.CurrentUICulture);
            if (idx == -1) {
                idx = itemsSource.IndexOf(Thread.CurrentThread.CurrentUICulture.Parent);
            }

            if (idx != -1) {
                cb.SelectedIndex = idx;
            }
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