using System;
using System.ComponentModel;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Models.Frost.DB;
using Frost.Models.Frost.DB.Files;
using RibbonUI.Util;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditSubtitles.xaml</summary>
    public partial class ListSubtitles : UserControl {
        public static readonly DependencyProperty SubtitlesProperty = DependencyProperty.Register("Subtitles", typeof(ObservableHashSet2<Subtitle>), typeof(ListSubtitles), new PropertyMetadata(default(ObservableHashSet2<Subtitle>)));
        private ICollectionView _collectionView;
        

        public ListSubtitles() {
            InitializeComponent();

            TypeDescriptor.GetProperties(SubtitlesList)["ItemsSource"].AddValueChanged(SubtitlesList, SubtitlesListItemSourceChanged); 
        }

        public ObservableHashSet2<Subtitle> Subtitles {
            get { return (ObservableHashSet2<Subtitle>) GetValue(SubtitlesProperty); }
            set { SetValue(SubtitlesProperty, value); }
        }

        private void SubtitlesListItemSourceChanged(object sender, EventArgs e) {
            _collectionView = (CollectionView) CollectionViewSource.GetDefaultView(SubtitlesList.ItemsSource);

            if (_collectionView == null) {
                return;
            }

            PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
            if (_collectionView.GroupDescriptions != null) {
                _collectionView.GroupDescriptions.Add(groupDescription);
            }            
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }

        private void LangEdit(object sender, RoutedEventArgs e) {
            Window window = Window.GetWindow(this);
            SelectLanguage sc = new SelectLanguage { Owner = window, DataContext = window.Resources["LanguagesSource"] };

            if (sc.ShowDialog() == true) {
                Button button = ((Button)sender);
                ((Subtitle) button.DataContext).Language = (Language) sc.SelectedLanguage.SelectedItem;

                _collectionView.Refresh();
            }
        }

        private void OnFormatSelectorLoaded(object sender, RoutedEventArgs e) {
            List<string> formats = new List<string> {
                "Adobe encore DVD",
                "Advanced Substation Alpha",
                "AQTitle",
                "ASS",
                "Captions Inc",
                "Cheeta",
                "Cheetah",
                "CPC Captioning",
                "CPC-600",
                "EBU Subtitling Format",
                "N19",
                "SAMI",
                "Sami Captioning",
                "SSA",
                "SubRip",
                "SubStation Alpha",
                "VobSub"
            };

            ComboBox cb = (ComboBox) sender;
            cb.ItemsSource = formats;
        }
    }
}
