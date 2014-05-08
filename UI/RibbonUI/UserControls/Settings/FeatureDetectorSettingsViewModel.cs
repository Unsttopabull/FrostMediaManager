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
using log4net;
using RibbonUI.Util;
using RibbonUI.Windows;
using RibbonUI.Windows.Add;

namespace RibbonUI.UserControls.Settings {

    public class FeatureDetectorSettingsViewModel {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FeatureDetectorSettingsViewModel));
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
            get { return _audioBindings ?? (_audioBindings = new ObservableChangeTrackingCollection<CodecIdBinding>(FileFeatures.AudioCodecIdMappings)); }
            set { _audioBindings = value; }
        }

        public ObservableChangeTrackingCollection<CodecIdBinding> VideoCodecBindings {
            get { return _videoBindings ?? (_videoBindings = new ObservableChangeTrackingCollection<CodecIdBinding>(FileFeatures.VideoCodecIdMappings)); }
            set { _videoBindings = value; }
        }

        public ObservableChangeTrackingCollection<string> KnownSubtitleExtensions {
            get {
                return _knownSubtiteExtensions ??
                       (_knownSubtiteExtensions = new ObservableChangeTrackingCollection<string>(new ObservableCollection<string>(FileFeatures.KnownSubtitleExtensions)));
            }
            set { _knownSubtiteExtensions = value; }
        }

        public ObservableChangeTrackingCollection<string> KnownVideoExtensions {
            get {
                return _knownVideoExtensions ??
                       (_knownVideoExtensions = new ObservableChangeTrackingCollection<string>(new ObservableCollection<string>(FeatureDetector.VideoExtensions)));
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
            string extension = InputBox.Show(ParentWindow, Gettext.T("Enter new extension:"), Gettext.T("Add new extension"), Gettext.T("Add"));
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

            string destFileName = Directory.GetCurrentDirectory() + "Images/FlagsE/vcodec_" + addedCodec.Mapping;
            try {
                File.Copy(addedCodec.ImagePath, destFileName);
            }
            catch (Exception) {
                if (Log.IsErrorEnabled) {
                    Log.Error(string.Format("Could not copy file \"{0}\" to path \"{1}\".", addedCodec.ImagePath, destFileName));
                }
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

            string destFileName = Directory.GetCurrentDirectory() + "Images/FlagsE/acodec_" + addedCodec.Mapping;
            try {
                File.Copy(addedCodec.ImagePath, destFileName);
            }
            catch (Exception) {
                if (Log.IsErrorEnabled) {
                    Log.Error(string.Format("Could not copy file \"{0}\" to path \"{1}\".", addedCodec.ImagePath, destFileName));
                }
            }
        }

        private void AddSubtitleFileExtension() {
            string extension = InputBox.Show(ParentWindow, Gettext.T("Enter new extension:"), Gettext.T("Add new extension"), Gettext.T("Add"));
            if (string.IsNullOrEmpty(extension)) {
                return;
            }

            FileFeatures.KnownSubtitleExtensions.Add(extension);
        }
    }

}