using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Frost.GettextMarkupExtension;
using Microsoft.WindowsAPICodePack.Dialogs;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace RibbonUI.UserControls.Settings {

    /// <summary>Interaction logic for GeneralSettings.xaml</summary>
    public partial class GeneralSettings : UserControl {
        public GeneralSettings() {
            InitializeComponent();
        }

        private void RemoveMovieFolderClicked(object sender, RoutedEventArgs e) {
            if (MovieFolders.SelectedIndex != -1) {
                Properties.Settings.Default.SearchFolders.Remove((string) MovieFolders.SelectedItem);
                MovieFolders.Items.Refresh();
            }
            else {
                MessageBox.Show(Window.GetWindow(this), "No folder selected");
            }
        }

        private void AddMovieFolderClicked(object sender, RoutedEventArgs e) {
            string folderPath;
            if (CommonFileDialog.IsPlatformSupported) {
                //Vista+

                using (CommonOpenFileDialog cfd = new CommonOpenFileDialog {
                    EnsureReadOnly = true,
                    IsFolderPicker = true,
                    AllowNonFileSystemItems = true,
                    Multiselect = false
                }) {
                    if (cfd.ShowDialog(Window.GetWindow(this)) != CommonFileDialogResult.Ok) {
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
            MovieFolders.Items.Refresh();
        }

        private void LangSelectionChanged(object sender, SelectionChangedEventArgs e) {
            ComboBox comboBox = (ComboBox) sender;
            if (comboBox.SelectedIndex == -1) {
                return;
            }

            TranslationManager.CurrentCulture = (CultureInfo) comboBox.SelectedItem;
        }

        private void LangSelectOnLoaded(object sender, RoutedEventArgs e) {
            ComboBox cb = (ComboBox) sender;

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
