using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Frost.Common;
using Frost.Common.Util;
using Frost.DetectFeatures;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost;
using GalaSoft.MvvmLight.Ioc;
using RibbonUI.Properties;
using RibbonUI.UserControls;
using RibbonUI.UserControls.List;
using RibbonUI.ViewModels;
using RibbonUI.ViewModels.UserControls;
using RibbonUI.ViewModels.UserControls.List;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        public App() {
            SimpleIoc.Default.Register<IMoviesDataService, FrostMoviesDataDataService>();
            RegisterViewModels();

            TranslationManager.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");
            ModelCreator.RegisterSystem(new FrostModelRegistrator(), true);

            //DispatcherUnhandledException += UnhandledExeption;

            LoadSettings();
        }

        private void RegisterViewModels() {
            SimpleIoc.Default.Register<ContentGridViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<EditMovieViewModel>();
            SimpleIoc.Default.Register<ListVideosViewModel>();
            SimpleIoc.Default.Register<ListAudiosViewModel>();
            SimpleIoc.Default.Register<ListSubtitlesViewModel>();
            SimpleIoc.Default.Register<ListArtViewModel>();
        }

        internal static void LoadSettings() {
            if (Settings.Default.KnownSubtitleExtensions == null) {
                SaveKnownSubtitleExtensionSetting();
            }
            else {
                FileFeatures.KnownSubtitleExtensions = new List<string>(Settings.Default.KnownSubtitleExtensions.Cast<string>());
            }

            if (Settings.Default.KnownSubtitleFormats == null) {
                SaveKnownSubtitleFormatsSetting();
            }
            else {
                FileFeatures.KnownSubtitleFormats = new List<string>(Settings.Default.KnownSubtitleFormats.Cast<string>());
            }

            if (Settings.Default.AudioCodecIdBindings == null) {
                SaveAudioCodecIdSettiing();
            }
            else {
                FileFeatures.AudioCodecIdMappings = new CodecIdMappingCollection(Settings.Default.AudioCodecIdBindings);
            }

            if (Settings.Default.VideoCodecIdBindings == null) {
                SaveVideoCodecIdBindingsSetting();
            }
            else {
                FileFeatures.VideoCodecIdMappings = new CodecIdMappingCollection(Settings.Default.VideoCodecIdBindings);
            }

            if (Settings.Default.KnownSegments == null) {
                SaveKnownSegmentsSetting();
            }
            else {
                FileNameParser.KnownSegments = new SegmentCollection(Settings.Default.KnownSegments);
            }

            if (Settings.Default.CustomLanguageMappings == null) {
                SaveCustomLanguageMappingsSetting();
            }
            else {
                FileNameParser.CustomLanguageMappings = new LanguageMappingCollection(Settings.Default.CustomLanguageMappings);
            }


            if (Settings.Default.ExcludedSegments == null) {
                SaveExcludedSegmentsSetting();
            }
            else {
                FileNameParser.ExcludedSegments = new ObservableHashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (string segment in Settings.Default.ExcludedSegments) {
                    FileNameParser.ExcludedSegments.Add(segment);
                }
            }

            if (Settings.Default.ReleaseGroups == null) {
                SaveReleaseGroupsSetting();
            }
            else {
                FileNameParser.ReleaseGroups = new ObservableHashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (string releaseGroup in Settings.Default.ReleaseGroups) {
                    FileNameParser.ReleaseGroups.Add(releaseGroup);
                }                
            }

            Settings.Default.Save();
        }

        internal static void SaveSettings() {
            SaveKnownSubtitleExtensionSetting();
            SaveKnownSubtitleFormatsSetting();
            SaveAudioCodecIdSettiing();
            SaveVideoCodecIdBindingsSetting();
            SaveKnownSegmentsSetting();
            SaveCustomLanguageMappingsSetting();
            SaveExcludedSegmentsSetting();
            SaveReleaseGroupsSetting();

            Settings.Default.Save();
        }

        #region Save settings

        private static void SaveKnownSubtitleExtensionSetting() {
            Settings.Default.KnownSubtitleExtensions = new StringCollection();
            Settings.Default.KnownSubtitleExtensions.AddRange(FileFeatures.KnownSubtitleExtensions.ToArray());
        }

        private static void SaveKnownSubtitleFormatsSetting() {
            Settings.Default.KnownSubtitleFormats = new StringCollection();
            Settings.Default.KnownSubtitleFormats.AddRange(FileFeatures.KnownSubtitleFormats.ToArray());
        }

        private static void SaveAudioCodecIdSettiing() {
            Settings.Default.AudioCodecIdBindings = new StringDictionary();
            foreach (KeyValuePair<string, string> pair in FileFeatures.AudioCodecIdMappings) {
                Settings.Default.AudioCodecIdBindings.Add(pair.Key, pair.Value);
            }
        }

        private static void SaveVideoCodecIdBindingsSetting() {
            Settings.Default.VideoCodecIdBindings = new StringDictionary();
            foreach (KeyValuePair<string, string> pair in FileFeatures.VideoCodecIdMappings) {
                Settings.Default.VideoCodecIdBindings.Add(pair.Key, pair.Value);
            }
        }

        private static void SaveKnownSegmentsSetting() {
            Settings.Default.KnownSegments = new StringDictionary();
            foreach (SegmentMapping mapping in FileNameParser.KnownSegments) {
                Settings.Default.KnownSegments.Add(mapping.Segment, mapping.SegmentType.ToString());
            }
        }

        private static void SaveCustomLanguageMappingsSetting() {
            Settings.Default.CustomLanguageMappings = new StringDictionary();
            foreach (LanguageMapping mapping in FileNameParser.CustomLanguageMappings) {
                Settings.Default.CustomLanguageMappings.Add(mapping.Mapping, mapping.ISO639Alpha3);
            }
        }

        private static void SaveExcludedSegmentsSetting() {
            Settings.Default.ExcludedSegments = new StringCollection();
            Settings.Default.ExcludedSegments.AddRange(FileNameParser.ExcludedSegments.ToArray());
        }

        private static void SaveReleaseGroupsSetting() {
            Settings.Default.ReleaseGroups = new StringCollection();
            Settings.Default.ReleaseGroups.AddRange(FileNameParser.ReleaseGroups.ToArray());
        }

        #endregion

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
           MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }

}