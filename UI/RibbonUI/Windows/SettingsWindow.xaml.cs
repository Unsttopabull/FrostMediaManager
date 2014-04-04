using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Common.Util;
using Frost.DetectFeatures.Util;
using RibbonUI.Properties;
using RibbonUI.UserControls.Settings;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for Settings.xaml</summary>
    public partial class SettingsWindow : Window {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public SettingsWindow() {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void SaveClick(object sender, RoutedEventArgs e) {
            SaveGeneral();
            SaveFeatureDetector();
            SaveFileNameParser();

            Settings.Default.Save();

            DialogResult = true;
            Close();
        }

        private void SaveGeneral() {
            GeneralSettingsViewModel vm = (GeneralSettingsViewModel) GeneralSettings.DataContext;

            Settings.Default.SearchFolders = new StringCollection();
            foreach (string folder in vm.SearchFolders) {
                Settings.Default.SearchFolders.Add(folder);
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e) {
            Settings.Default.Reload();
            App.LoadSettings();

            DialogResult = false;
            Close();
        }

        private void SaveFeatureDetector() {
            FeatureDetectorSettingsViewModel vm = (FeatureDetectorSettingsViewModel) FeatureDetector.DataContext;

            if (vm.VideoCodecBindings.IsDirty) {
                AddRemoveDict(vm.VideoCodecBindings, "VideoCodecBindings");
            }

            if (vm.AudioCodecBindings.IsDirty) {
                AddRemoveDict(vm.AudioCodecBindings, "AudioCodecBindings");
            }

            if (vm.KnownSubtitleExtensions.IsDirty) {
                AddRemoveStringCollection(vm.KnownSubtitleExtensions, "KnownSubtitleExtensions");
            }

            if (vm.KnownVideoExtensions.IsDirty) {
                AddRemoveStringCollection(vm.KnownVideoExtensions, "KnownVideoExtensions");
            }
        }

        private void SaveFileNameParser() {
            FileNameParserSettingsViewModel vm = (FileNameParserSettingsViewModel) FileParser.DataContext;
            if (vm.KnownSegments.IsDirty) {
                AddRemoveDict(vm.KnownSegments, "KnownSegments");
            }

            if (vm.CustomLanguageMappings.IsDirty) {
                AddRemoveDict(vm.CustomLanguageMappings, "CustomLanguageMappings");
            }

            if (vm.ExcludedSegments.IsDirty) {
                AddRemoveStringCollection(vm.ExcludedSegments, "ExcludedSegments");
            }

            if (vm.ReleaseGroups.IsDirty) {
                AddRemoveStringCollection(vm.ReleaseGroups, "ReleaseGroups");
            }
        }

        private void AddRemoveStringCollection(ChangeTrackingCollection<string> changeTrackingCollection, string setting) {
            StringCollection removedSetting = (StringCollection) Settings.Default[setting + "Removed"] ?? new StringCollection();
            StringCollection addedSetting = (StringCollection) Settings.Default[setting + "Added"] ?? new StringCollection();

            foreach (string group in changeTrackingCollection.AddedItems) {
                if (removedSetting.Contains(group)) {
                    removedSetting.Remove(group);
                }
                addedSetting.Add(group);
            }

            foreach (string group in changeTrackingCollection.RemovedItems) {
                if (addedSetting.Contains(group)) {
                    addedSetting.Remove(group);
                }
                removedSetting.Add(group);
            }
        }

        private void AddRemoveDict<T>(ChangeTrackingCollection<T> changeTrackingCollection, string setting) where T : IKeyValue, IEquatable<T> {
            StringDictionary removedSetting = (StringDictionary) Settings.Default[setting + "Removed"] ?? new StringDictionary();
            StringDictionary addedSetting = (StringDictionary) Settings.Default[setting + "Added"] ?? new StringDictionary();

            foreach (T mapping in changeTrackingCollection.AddedItems) {
                if (removedSetting.ContainsKey(mapping.Key)) {
                    removedSetting.Remove(mapping.Key);
                }
                addedSetting.Add(mapping.Key, mapping.Value);
            }

            foreach (T mapping in changeTrackingCollection.RemovedItems) {
                if (addedSetting.ContainsKey(mapping.Key)) {
                    addedSetting.Remove(mapping.Key);
                }
                removedSetting.Add(mapping.Key, mapping.Value);
            }
        }
    }

}