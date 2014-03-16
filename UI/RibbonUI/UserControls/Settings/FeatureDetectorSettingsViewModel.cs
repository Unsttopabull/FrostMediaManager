using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Frost.DetectFeatures;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Util;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.Settings {
    public class FeatureDetectorSettingsViewModel {

        public FeatureDetectorSettingsViewModel() {
            RemoveFileExtensionCommand = new RelayCommand<string>(
                ext => FeatureDetector.VideoExtensions.Remove(ext),
                ext => !string.IsNullOrEmpty(ext)
            );
            AddFileExtensionCommand = new RelayCommand(AddFileExtension);

            RemoveAudioCodecMappingCommand = new RelayCommand<CodecIdBinding>(
                cb => FileFeatures.AudioCodecIdMappings.Remove(cb),
                cb => cb != null
            );
            AddAudioCodecMappingCommand = new RelayCommand(AddAudioCodecMapping);

            RemoveAudioCodecMappingCommand = new RelayCommand<CodecIdBinding>(
                cb => FileFeatures.VideoCodecIdMappings.Remove(cb),
                cb => cb != null
            );
            AddVideoCodecMappingCommand = new RelayCommand(AddVideoCodecMapping);

            RemoveSubtitleFileExtensionCommand = new RelayCommand<string>(
                ext => FileFeatures.KnownSubtitleExtensions.Remove(ext),
                ext => !string.IsNullOrEmpty(ext)
            );

            AddSubtitleFileExtensionCommand = new RelayCommand(AddSubtitleFileExtension);
        }

        public Window ParentWindow { get; set; }

        #region Commands

        public ICommand<string> RemoveFileExtensionCommand { get; private set; }

        public ICommand AddFileExtensionCommand { get; private set; }

        public ICommand<string> RemoveSubtitleFileExtensionCommand { get; private set; }

        public ICommand AddSubtitleFileExtensionCommand { get; private set; }

        public ICommand<CodecIdBinding> RemoveAudioCodecMappingCommand { get; private set; }

        public ICommand AddAudioCodecMappingCommand { get; private set; }

        public ICommand<CodecIdBinding> RemoveVideoCodecMappingCommand { get; private set; }

        public ICommand AddVideoCodecMappingCommand { get; private set; }

        #endregion

        private void AddFileExtension() {
            string extension = InputBox.Show(ParentWindow, TranslationManager.T("Enter new extension:"), TranslationManager.T("Add new extension"), TranslationManager.T("Add"));
            if (string.IsNullOrEmpty(extension)) {
                return;
            }

            FeatureDetector.VideoExtensions.Add(extension);
        }

        private void AddVideoCodecMapping() {
            AddCodecMapping acm = new AddCodecMapping(true) { Owner = ParentWindow };
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
        }

        private void AddAudioCodecMapping() {
            AddCodecMapping acm = new AddCodecMapping(false) { Owner = ParentWindow };
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
        }

        private void AddSubtitleFileExtension() {
            string extension = InputBox.Show(ParentWindow, TranslationManager.T("Enter new extension:"), TranslationManager.T("Add new extension"), TranslationManager.T("Add"));
            if (string.IsNullOrEmpty(extension)) {
                return;
            }

            FileFeatures.KnownSubtitleExtensions.Add(extension);
        }

    }
}
