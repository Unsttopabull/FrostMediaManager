using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.DetectFeatures;
using Frost.DetectFeatures.Util;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Util;
using RibbonUI.Windows;
using RibbonUI.Windows.Add;

namespace RibbonUI.UserControls.Settings {
    public class FeatureDetectorSettingsViewModel {
        private ObservableChangeTrackingCollection<CodecIdBinding> _audioBindings;
        private ObservableChangeTrackingCollection<CodecIdBinding> _videoBindings;
        private ObservableChangeTrackingCollection<string> _knownSubtiteExtensions;
        private ObservableChangeTrackingCollection<string> _knownVideoExtensions;

        public FeatureDetectorSettingsViewModel() {
            RemoveFileExtensionCommand = new RelayCommand<string>(
                ext => KnownVideoExtensions.Remove(ext),
                ext => !string.IsNullOrEmpty(ext)
            );
            AddFileExtensionCommand = new RelayCommand(AddFileExtension);

            AddAudioCodecMappingCommand = new RelayCommand(AddAudioCodecMapping);
            RemoveAudioCodecMappingCommand = new RelayCommand<CodecIdBinding>(
                cb => AudioCodecBindings.Remove(cb),
                cb => cb != null
            );
            

            AddVideoCodecMappingCommand = new RelayCommand(AddVideoCodecMapping);
            RemoveVideoCodecMappingCommand = new RelayCommand<CodecIdBinding>(
                cb => VideoCodecBindings.Remove(cb),
                cb => cb != null
            );
           

            AddSubtitleFileExtensionCommand = new RelayCommand(AddSubtitleFileExtension);
            RemoveSubtitleFileExtensionCommand = new RelayCommand<string>(
                ext => KnownSubtitleExtensions.Remove(ext),
                ext => !string.IsNullOrEmpty(ext)
            );
        }

        public ObservableChangeTrackingCollection<CodecIdBinding> AudioCodecBindings {
            get {
                if (_audioBindings == null) {
                    _audioBindings = new ObservableChangeTrackingCollection<CodecIdBinding>(FileFeatures.AudioCodecIdMappings);
                }
                return _audioBindings;
            }
            set { _audioBindings = value; }
        }

        public ObservableChangeTrackingCollection<CodecIdBinding> VideoCodecBindings {
            get {
                if (_videoBindings == null) {
                    _videoBindings = new ObservableChangeTrackingCollection<CodecIdBinding>(FileFeatures.VideoCodecIdMappings);
                }
                return _videoBindings;
            }
            set { _videoBindings = value; }            
        }

        public ObservableChangeTrackingCollection<string> KnownSubtitleExtensions {
            get {
                if (_knownSubtiteExtensions == null) {
                    _knownSubtiteExtensions = new ObservableChangeTrackingCollection<string>(new ObservableCollection<string>(FileFeatures.KnownSubtitleExtensions));
                }
                return _knownSubtiteExtensions;
            }
            set { _knownSubtiteExtensions = value; }
        }

        public ObservableChangeTrackingCollection<string> KnownVideoExtensions {
            get {
                if (_knownVideoExtensions == null) {
                    _knownVideoExtensions = new ObservableChangeTrackingCollection<string>(new ObservableCollection<string>(FeatureDetector.VideoExtensions));
                }
                return _knownVideoExtensions;
            }
            set { _knownVideoExtensions = value; }
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
