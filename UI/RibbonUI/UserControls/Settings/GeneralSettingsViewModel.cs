using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using Microsoft.WindowsAPICodePack.Dialogs;
using ComboBox = System.Windows.Controls.ComboBox;

namespace RibbonUI.UserControls.Settings {

    public class GeneralSettingsViewModel {

        public GeneralSettingsViewModel() {
            LangSelectLoadedCommand = new RelayCommand<ComboBox>(LangSelectOnLoaded);
            LangSelectionChangedCommand = new RelayCommand<CultureInfo>(LangSelectionChanged, ci => ci != null);

            RemoveMovieFolderCommand = new RelayCommand<string>(RemoveMovieFolderClicked, folder => !string.IsNullOrEmpty(folder));
            AddMovieFolderCommand = new RelayCommand(AddMovieFolderClicked);
        }

        public Window ParentWindow { get; set; }

        public ICommand<CultureInfo> LangSelectionChangedCommand { get; private set; }
        public ICommand<ComboBox> LangSelectLoadedCommand { get; private set; }

        public ICommand<string> RemoveMovieFolderCommand { get; private set; }
        public ICommand AddMovieFolderCommand { get; private set; }

        public StringCollection SearchFolders {
            get { return Properties.Settings.Default.SearchFolders; }
            set { Properties.Settings.Default.SearchFolders = value; }
        }

        private void RemoveMovieFolderClicked(string movieFolder) {
            Properties.Settings.Default.SearchFolders.Remove(movieFolder);
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

            Properties.Settings.Default.SearchFolders.Add(folderPath);
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

    }

}