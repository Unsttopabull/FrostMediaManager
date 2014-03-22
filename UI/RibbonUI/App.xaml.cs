using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Frost.Common.Util;
using Frost.DetectFeatures;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
using RibbonUI.Design;
using RibbonUI.Properties;
using RibbonUI.UserControls;
using RibbonUI.UserControls.List;
using RibbonUI.Util;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        internal static List<Provider> Systems { get; private set; }

        static App() {
            Systems = new List<Provider>();
        }

        public App() {
            FileStream debugLog = File.Create("debug.txt");
            Debug.Listeners.Add(new TextWriterTraceListener(debugLog));
            Debug.AutoFlush = true;

            LoadPlugins();
            RegisterViewModels();

            TranslationManager.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");
            //DispatcherUnhandledException += UnhandledExeption;
        }

        private void LoadPlugins() {
            if (Directory.Exists("plugins")) {
                string[] plugins;
                try {
                    plugins = Directory.GetFiles("plugins", "*.dll");
                }
                catch (Exception) {
                    MessageBox.Show("Could not access plugin folder. Program will now exit");
                    Shutdown();
                    return;
                }

                int numFailed = 0;
                foreach (string plugin in plugins) {
                    Assembly assembly;
                    Provider provider;
                    if (!AssemblyEx.CheckIsPlugin(plugin, out assembly, out provider)) {
                        numFailed++;
                        continue;
                    }

                    provider.AssemblyPath = plugin;
                    Systems.Add(provider);
                }

                if (numFailed == plugins.Length) {
                    MessageBox.Show("Couldn't load any plugins in the plugin folder. Program will now exit.", "Error loading providers", MessageBoxButton.OK);
                    Shutdown();
                }
                return;
            }

            MessageBox.Show("No plugins for movie library manipulation found. Program will now exit.");
            Shutdown();
        }

        private void RegisterViewModels() {
            if (TranslationManager.IsInDesignMode) {
                LightInjectContainer.RegisterFrom<DesignCompositionRoot>();
            }

            LightInjectContainer.Register<ContentGridViewModel>();
            LightInjectContainer.Register<MainWindowViewModel>();
            LightInjectContainer.Register<EditMovieViewModel>();
            LightInjectContainer.Register<ListVideosViewModel>();
            LightInjectContainer.Register<ListAudiosViewModel>();
            LightInjectContainer.Register<ListSubtitlesViewModel>();
            LightInjectContainer.Register<ListArtViewModel>();
        }

        internal static void LoadSettings() {
            Task[] settings = new Task[8];

            settings[0] = Task.Run(() => {
                if (Settings.Default.KnownSubtitleExtensions == null) {
                    SaveKnownSubtitleExtensionSetting();
                }
                else {
                    FileFeatures.KnownSubtitleExtensions = new List<string>(Settings.Default.KnownSubtitleExtensions.Cast<string>());
                }
            });

            settings[1] = Task.Run(() => {
                if (Settings.Default.KnownSubtitleFormats == null) {
                    SaveKnownSubtitleFormatsSetting();
                }
                else {
                    FileFeatures.KnownSubtitleFormats = new List<string>(Settings.Default.KnownSubtitleFormats.Cast<string>());
                }
            });

            settings[2] = Task.Run(() => {
                if (Settings.Default.AudioCodecIdBindings == null) {
                    SaveAudioCodecIdSettiing();
                }
                else {
                    FileFeatures.AudioCodecIdMappings = new CodecIdMappingCollection(Settings.Default.AudioCodecIdBindings);
                }
            });

            settings[3] = Task.Run(() => {
                if (Settings.Default.VideoCodecIdBindings == null) {
                    SaveVideoCodecIdBindingsSetting();
                }
                else {
                    FileFeatures.VideoCodecIdMappings = new CodecIdMappingCollection(Settings.Default.VideoCodecIdBindings);
                }
            });

            settings[4] = Task.Run(() => {
                if (Settings.Default.KnownSegments == null) {
                    SaveKnownSegmentsSetting();
                }
                else {
                    FileNameParser.KnownSegments = new SegmentCollection(Settings.Default.KnownSegments);
                }
            });

            settings[5] = Task.Run(() => {
                if (Settings.Default.CustomLanguageMappings == null) {
                    SaveCustomLanguageMappingsSetting();
                }
                else {
                    FileNameParser.CustomLanguageMappings = new LanguageMappingCollection(Settings.Default.CustomLanguageMappings);
                }
            });


            settings[6] = Task.Run(() => {
                if (Settings.Default.ExcludedSegments == null) {
                    SaveExcludedSegmentsSetting();
                }
                else {
                    FileNameParser.ExcludedSegments = new ObservableHashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (string segment in Settings.Default.ExcludedSegments) {
                        FileNameParser.ExcludedSegments.Add(segment);
                    }
                }
            });

            settings[7] = Task.Run(() => {
                if (Settings.Default.ReleaseGroups == null) {
                    SaveReleaseGroupsSetting();
                }
                else {
                    FileNameParser.ReleaseGroups = new ObservableHashSet<string>(StringComparer.OrdinalIgnoreCase);
                    foreach (string releaseGroup in Settings.Default.ReleaseGroups) {
                        FileNameParser.ReleaseGroups.Add(releaseGroup);
                    }
                }
            });

            Task.WaitAll(settings);

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