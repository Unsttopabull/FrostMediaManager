using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Frost.Common;
using Frost.Common.Util;
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
        public static string SystemType { get; internal set; }

        public static List<string> Systems { get; private set; }

        static App() {
            SystemType = "XBMC";
            Systems = new List<string>();
        }

        public App() {
            LoadPlugins();
            RegisterViewModels();

            TranslationManager.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");
            //DispatcherUnhandledException += UnhandledExeption;

            LoadSettings();
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
                    string systemName;
                    if (!CheckIsPlugin(plugin, out assembly, out systemName)) {
                        numFailed++;
                        break;
                    }

                    try {
                        LightInjectContainer.RegisterAssembly(assembly);
                        Systems.Add(systemName);
                    }
                    catch (Exception e) {
                        numFailed++;
                    }
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

        private bool CheckIsPlugin(string plugin, out Assembly assembly, out string pluginSystemName) {
            string path = Path.Combine(Directory.GetCurrentDirectory(), plugin);
            if (!CheckIsAssembly(path)) {
                assembly = null;
                pluginSystemName = null;
                return false;
            }

            try {
                Assembly asm = Assembly.LoadFile(path);
                IsPluginAttribute isPlugin = asm.GetCustomAttribute<IsPluginAttribute>();
                if (isPlugin != null) {
                    assembly = asm;
                    pluginSystemName = isPlugin.SystemName;
                    return true;
                }

                assembly = null;
                pluginSystemName = null;
                return false;
            }
            catch (FileLoadException e) {
                assembly = null;
                pluginSystemName = null;
                return false;
            }
            catch (Exception e) {
                assembly = null;
                pluginSystemName = null;
                return false;
            }
        }

        /// <summary>Checks if the file is a valid CLR assembly.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <remarks>Taken from http://geekswithblogs.net/rupreet/archive/2005/11/02/58873.aspx </remarks>
        private bool CheckIsAssembly(string fileName) {
            uint[] dataDictionaryRva = new uint[16];
            uint[] dataDictionarySize = new uint[16];

            using (Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                using (BinaryReader reader = new BinaryReader(fs)) {

                    //PE Header starts @ 0x3C (60). Its a 4 byte header.
                    fs.Position = 0x3C;

                    uint peHeader = reader.ReadUInt32();

                    //Moving to PE Header start location...
                    fs.Position = peHeader;
                    reader.ReadUInt32();

                    //We can also show all these value, but we will be       
                    //limiting to the CLI header test.

                    reader.ReadUInt16();
                    reader.ReadUInt16();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt16();
                    reader.ReadUInt16();

                    /*
                     *    Now we are at the end of the PE Header and from here, the PE Optional Headers starts...
                     *    To go directly to the datadictionary, we'll increase the stream’s current position to with 96 (0x60). 96 because,
                     *    28 for Standard fields, 68 for NT-specific fields
                     *    From here DataDictionary starts...and its of total 128 bytes.
                     *    DataDictionay has 16 directories in total, doing simple maths 128/16 = 8.
                     * 
                     *    So each directory is of 8 bytes.
                     *    In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size.
                     *
                     *    btw, the 15th directory consist of CLR header! if its 0, its not a CLR file :)
                     */
                    ushort dataDictionaryStart = Convert.ToUInt16(Convert.ToUInt16(fs.Position) + 0x60);
                    fs.Position = dataDictionaryStart;
                    for (int i = 0; i < 15; i++) {
                        dataDictionaryRva[i] = reader.ReadUInt32();
                        dataDictionarySize[i] = reader.ReadUInt32();
                    }

                    return dataDictionaryRva[14] != 0;
                }
            }
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

        /// <summary>Raises the <see cref="E:System.Windows.Application.Exit"/> event.</summary>
        /// <param name="e">An <see cref="T:System.Windows.ExitEventArgs"/> that contains the event data.</param>
        protected override void OnExit(ExitEventArgs e) {
            LightInjectContainer.Dispose();
            base.OnExit(e);
        }
    }

}