using System;
using System.Windows;
using System.Windows.Controls;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.Settings {
    /// <summary>Interaction logic for EditFeatureDetector.xaml</summary>
    public partial class FeatureDetectorSettings : UserControl {
        public FeatureDetectorSettings() {
            InitializeComponent();
        }

        #region File extensions

        private void RemoveFileExtensionClicked(object sender, RoutedEventArgs e) {
        }

        private void AddFileExtensionClicked(object sender, RoutedEventArgs e) {
        }

        #endregion


        #region Video

        private void RemoveVideoCodecMappingClicked(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void AddVideoCodecMappingClicked(object sender, RoutedEventArgs e) {
            AddCodecMapping acm = new AddCodecMapping(true) { Owner = Window.GetWindow(this) };
            acm.ShowDialog();
        }

        #endregion



        #region Audio

        private void RemoveAudioCodecMappingClicked(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void AddAudioCodecMappingClicked(object sender, RoutedEventArgs e) {
            AddCodecMapping acm = new AddCodecMapping(false) { Owner = Window.GetWindow(this) };
            acm.ShowDialog();
        }

        #endregion



        #region Subtitles

        private void AddSubtitleFileExtensionClicked(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        private void RemoveSubtitleFileExtensionClicked(object sender, RoutedEventArgs e) {
            throw new NotImplementedException();
        }

        #endregion

    }
}
