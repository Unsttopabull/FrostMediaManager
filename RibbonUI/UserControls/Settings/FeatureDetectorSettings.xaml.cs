using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Frost.DetectFeatures;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
using RibbonUI.Util;
using RibbonUI.Windows;
using File = System.IO.File;

namespace RibbonUI.UserControls.Settings {
    /// <summary>Interaction logic for EditFeatureDetector.xaml</summary>
    public partial class FeatureDetectorSettings : UserControl {
        private Window _window;

        public FeatureDetectorSettings() {
            InitializeComponent();
        }

        #region File extensions

        private void RemoveFileExtensionClicked(object sender, RoutedEventArgs e) {
            if (VideoExtensionList.SelectedIndex == -1) {
                MessageBox.Show(_window, TranslationManager.T("No {0} selected", "extension"));
                return;
            }
            FeatureDetector.VideoExtensions.Remove((string) VideoExtensionList.SelectedItem);
            VideoExtensionList.Items.Refresh();
        }

        private void AddFileExtensionClicked(object sender, RoutedEventArgs e) {
            string extension = InputBox.Show(_window, TranslationManager.T("Enter new extension:"), TranslationManager.T("Add new extension"), TranslationManager.T("Add"));
            if (string.IsNullOrEmpty(extension)) {
                return;
            }

            FeatureDetector.VideoExtensions.Add(extension);
            VideoExtensionList.Items.Refresh();
        }

        #endregion


        #region Video

        private void RemoveVideoCodecMappingClicked(object sender, RoutedEventArgs e) {
            if (VideoCodecBindings.SelectedIndex == -1) {
                MessageBox.Show(_window, TranslationManager.T("No {0} selected", "video codec mapping"));
                return;
            }

            FileFeatures.VideoCodecIdMappings.Remove((CodecIdBinding) VideoCodecBindings.SelectedItem);
            VideoCodecBindings.Items.Refresh();
        }

        private void AddVideoCodecMappingClicked(object sender, RoutedEventArgs e) {
            AddCodecMapping acm = new AddCodecMapping(true) { Owner = _window };
            if (acm.ShowDialog() != true) {
                return;
            }

            KnownCodec addedCodec = acm.AddedCodec;

            FileFeatures.VideoCodecIdMappings.Add(addedCodec.CodecId, addedCodec.Mapping);
            if (!acm.IsNew) {
                return;
            }

            try {
                File.Copy(addedCodec.ImagePath, Directory.GetCurrentDirectory() + "Images/FlagsE/vcodec_" + addedCodec.Mapping);
            }
            catch(Exception) {
                        
            }
            VideoCodecBindings.Items.Refresh();
        }

        #endregion



        #region Audio

        private void RemoveAudioCodecMappingClicked(object sender, RoutedEventArgs e) {
            if (AudioCodecBindings.SelectedIndex == -1) {
                MessageBox.Show(_window, TranslationManager.T("No {0} selected", "audio codec mapping"));
                return;
            }
            
            FileFeatures.AudioCodecIdMappings.Remove((CodecIdBinding) AudioCodecBindings.SelectedItem);
            AudioCodecBindings.Items.Refresh();
        }

        private void AddAudioCodecMappingClicked(object sender, RoutedEventArgs e) {
            AddCodecMapping acm = new AddCodecMapping(false) { Owner = Window.GetWindow(this) };
            if (acm.ShowDialog() != true) {
                return;
            }

            KnownCodec addedCodec = acm.AddedCodec;

            FileFeatures.VideoCodecIdMappings.Add(addedCodec.CodecId, addedCodec.Mapping);
            if (!acm.IsNew) {
                return;
            }

            try {
                File.Copy(addedCodec.ImagePath, Directory.GetCurrentDirectory() + "Images/FlagsE/acodec_" + addedCodec.Mapping);
            }
            catch(Exception) {
                        
            }
            AudioCodecBindings.Items.Refresh();
        }

        #endregion



        #region Subtitles

        private void AddSubtitleFileExtensionClicked(object sender, RoutedEventArgs e) {
            string extension = InputBox.Show(_window, TranslationManager.T("Enter new extension:"), TranslationManager.T("Add new extension"), TranslationManager.T("Add"));
            if (string.IsNullOrEmpty(extension)) {
                return;
            }

            FileFeatures.KnownSubtitleExtensions.Add(extension);
            SubtitleExtensionList.Items.Refresh();
        }

        private void RemoveSubtitleFileExtensionClicked(object sender, RoutedEventArgs e) {
            if (SubtitleExtensionList.SelectedIndex == -1) {
                MessageBox.Show(_window, TranslationManager.T("No {0} selected", "extension"));
                return;
            }

            FileFeatures.KnownSubtitleExtensions.Remove((string) SubtitleExtensionList.SelectedItem);
            SubtitleExtensionList.Items.Refresh();
        }

        #endregion

        private void FeatureDetectorSettingsLoaded(object sender, RoutedEventArgs e) {
            _window = Window.GetWindow(this);
        }

        private void FeatureDetectorSettingsUnloaded(object sender, RoutedEventArgs e) {
            _window = null;
        }
    }
}
