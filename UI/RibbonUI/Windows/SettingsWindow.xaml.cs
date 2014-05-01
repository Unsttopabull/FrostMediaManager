using System;
using System.Collections.Specialized;
using System.Windows;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Common.Util;
using RibbonUI.Properties;
using RibbonUI.UserControls.Settings;
using RibbonUI.Util;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for Settings.xaml</summary>
    public partial class SettingsWindow : Window {

        public SettingsWindow() {
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e) {
            NativeMethods.HideWindowBorderButtons(this);
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
            Settings.Load();

            DialogResult = false;
            Close();
        }

        private void SaveFeatureDetector() {
            FeatureDetectorSettingsViewModel vm = (FeatureDetectorSettingsViewModel) FeatureDetector.DataContext;

            if (vm.VideoCodecBindings.IsDirty) {
                AddRemoveDict(vm.VideoCodecBindings, "VideoCodecIdBindings");
            }

            if (vm.AudioCodecBindings.IsDirty) {
                AddRemoveDict(vm.AudioCodecBindings, "AudioCodecIdBindings");
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
            if (Settings.Default[setting + "Removed"] == null) {
                Settings.Default[setting + "Removed"] = new StringCollection();
            }
            StringCollection removedSetting = (StringCollection) Settings.Default[setting + "Removed"];

            if (Settings.Default[setting + "Added"] == null) {
                Settings.Default[setting + "Added"] = new StringCollection();
            }
            StringCollection addedSetting = (StringCollection) Settings.Default[setting + "Added"];

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
            if (Settings.Default[setting + "Removed"] == null) {
                Settings.Default[setting + "Removed"] = new SerializableStringDictionary();
            }
            SerializableStringDictionary removedSetting = (SerializableStringDictionary) Settings.Default[setting + "Removed"];

            if (Settings.Default[setting + "Added"] == null) {
                Settings.Default[setting + "Added"] = new SerializableStringDictionary();
            }
            SerializableStringDictionary addedSetting = (SerializableStringDictionary) Settings.Default[setting + "Added"];

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

        private void ResetToFactoryClick(object sender, RoutedEventArgs e) {
            Settings.ResetToFactory();
            Settings.Load();
            Close();
        }
    }

}