using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Frost.DetectFeatures;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
using RibbonUI.Properties;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        public App() {
            TranslationManager.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");

            DispatcherUnhandledException += UnhandledExeption;

            LoadSettings();
        }

        internal static void LoadSettings() {
            if (Settings.Default.KnownSubtitleExtensions == null) {
                Settings.Default.KnownSubtitleExtensions = new StringCollection();
                Settings.Default.KnownSubtitleExtensions.AddRange(FileFeatures.KnownSubtitleExtensions.ToArray());
            }
            else {
                FileFeatures.KnownSubtitleExtensions = new List<string>(Settings.Default.KnownSubtitleExtensions.Cast<string>());
            }

            if (Settings.Default.KnownSubtitleFormats == null) {
                Settings.Default.KnownSubtitleFormats = new StringCollection();
                Settings.Default.KnownSubtitleFormats.AddRange(FileFeatures.KnownSubtitleFormats.ToArray());
            }
            else {
                FileFeatures.KnownSubtitleFormats = new List<string>(Settings.Default.KnownSubtitleFormats.Cast<string>());
            }

            if (Settings.Default.AudioCodecIdBindings == null) {
                Settings.Default.AudioCodecIdBindings = new StringDictionary();
                foreach (KeyValuePair<string, string> pair in FileFeatures.AudioCodecIdMappings) {
                    Settings.Default.AudioCodecIdBindings.Add(pair.Key, pair.Value);
                }
            }
            else {
                FileFeatures.AudioCodecIdMappings = new CodecIdMappingCollection();
                foreach (DictionaryEntry entry in Settings.Default.AudioCodecIdBindings) {
                    FileFeatures.AudioCodecIdMappings.Add((string) entry.Key, (string) entry.Value);
                }
            }

            if (Settings.Default.VideoCodecIdBindings == null) {
                Settings.Default.VideoCodecIdBindings = new StringDictionary();
                foreach (KeyValuePair<string, string> pair in FileFeatures.VideoCodecIdMappings) {
                    Settings.Default.VideoCodecIdBindings.Add(pair.Key, pair.Value);
                }
            }
            else {
                FileFeatures.AudioCodecIdMappings = new CodecIdMappingCollection();
                foreach (DictionaryEntry entry in Settings.Default.VideoCodecIdBindings) {
                    FileFeatures.VideoCodecIdMappings.Add((string) entry.Key, (string) entry.Value);
                }                
            }
        }

        internal static void SaveSettings() {
            
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }

}