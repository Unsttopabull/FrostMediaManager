using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Frost.DetectFeatures;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
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
                if (Settings.Default.KnownSubtitleFormatsAdded != null) {
                    foreach (string format in Settings.Default.KnownSubtitleFormatsAdded) {
                        FileFeatures.KnownSubtitleFormats.Add(format);
                    }
                }

                if (Settings.Default.KnownSubtitleFormatsRemoved != null) {
                    foreach (string format in Settings.Default.KnownSubtitleFormatsRemoved) {
                        FileFeatures.KnownSubtitleFormats.Remove(format);
                    }
                }
            });

            settings[1] = Task.Run(() => {
                if (Settings.Default.KnownSubtitleExtensionsAdded != null) {
                    foreach (string extension in Settings.Default.KnownSubtitleExtensionsAdded) {
                        FileFeatures.KnownSubtitleExtensions.Add(extension);
                    }
                }

                if (Settings.Default.KnownSubtitleExtensionsRemoved != null) {
                    foreach (string extension in Settings.Default.KnownSubtitleExtensionsRemoved) {
                        FileFeatures.KnownSubtitleExtensions.Remove(extension);
                    }
                }

                if (Settings.Default.KnownVideoExtensionsAdded != null) {
                    foreach (string format in Settings.Default.KnownVideoExtensionsAdded) {
                        FeatureDetector.VideoExtensions.Add(format);
                    }
                }

                if (Settings.Default.KnownVideoExtensionsRemoved != null) {
                    foreach (string format in Settings.Default.KnownVideoExtensionsRemoved) {
                        FeatureDetector.VideoExtensions.Remove(format);
                    }
                }
            });

            settings[2] = Task.Run(() => {
                if (Settings.Default.AudioCodecIdBindingsAdded != null) {
                    foreach (DictionaryEntry binding in Settings.Default.AudioCodecIdBindingsAdded) {
                        FileFeatures.AudioCodecIdMappings.Add(new CodecIdBinding((string) binding.Key, (string) binding.Value));
                    }
                }

                if (Settings.Default.AudioCodecIdBindingsRemoved != null) {
                    foreach (DictionaryEntry binding in Settings.Default.AudioCodecIdBindingsRemoved) {
                        FileFeatures.AudioCodecIdMappings.Remove((string) binding.Key);
                    }
                }
            });

            settings[3] = Task.Run(() => {
                if (Settings.Default.VideoCodecIdBindingsAdded != null) {
                    foreach (DictionaryEntry binding in Settings.Default.VideoCodecIdBindingsAdded) {
                        FileFeatures.VideoCodecIdMappings.Add(new CodecIdBinding((string) binding.Key, (string) binding.Value));
                    }
                }

                if (Settings.Default.VideoCodecIdBindingsRemoved != null) {
                    foreach (DictionaryEntry binding in Settings.Default.VideoCodecIdBindingsRemoved) {
                        FileFeatures.VideoCodecIdMappings.Remove((string) binding.Key);
                    }
                }
            });

            settings[4] = Task.Run(() => {
                if (Settings.Default.KnownSegmentsAdded != null) {
                    foreach (DictionaryEntry segment in Settings.Default.KnownSegmentsAdded) {
                        FileNameParser.KnownSegments.Add((string) segment.Key, (SegmentType) segment.Value);
                    }
                }

                if (Settings.Default.KnownSegmentsRemoved != null) {
                    foreach (DictionaryEntry segment in Settings.Default.KnownSegmentsRemoved) {
                        FileNameParser.KnownSegments.Remove((string) segment.Key);
                    }
                }
            });

            settings[5] = Task.Run(() => {
                if (Settings.Default.CustomLanguageBindingsAdded != null) {
                    foreach (DictionaryEntry binding in Settings.Default.CustomLanguageBindingsAdded) {
                        FileNameParser.CustomLanguageMappings.Add((string) binding.Key, (string) binding.Value);
                    }
                }

                if (Settings.Default.CustomLanguageBindingsRemoved != null) {
                    foreach (DictionaryEntry binding in Settings.Default.CustomLanguageBindingsRemoved) {
                        FileNameParser.CustomLanguageMappings.Remove((string) binding.Key);
                    }
                }
            });


            settings[6] = Task.Run(() => {
                if (Settings.Default.ExcludedSegmentsAdded != null) {
                    foreach (string segment in Settings.Default.ExcludedSegmentsAdded) {
                        FileNameParser.ExcludedSegments.Add(segment);
                    }
                }

                if (Settings.Default.ExcludedSegmentsRemoved != null) {
                    foreach (string segment in Settings.Default.ExcludedSegmentsRemoved) {
                        FileNameParser.ExcludedSegments.Remove(segment);
                    }
                }
            });

            settings[7] = Task.Run(() => {
                if (Settings.Default.ReleaseGroupsAdded != null) {
                    foreach (string group in Settings.Default.ReleaseGroupsAdded) {
                        FileNameParser.ReleaseGroups.Add(group);
                    }
                }
                if (Settings.Default.ReleaseGroupsRemoved != null) {
                    foreach (string group in Settings.Default.ReleaseGroupsRemoved) {
                        FileNameParser.ReleaseGroups.Remove(group);
                    }
                }
            });

            Task.WaitAll(settings);

            Settings.Default.Save();
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }

}