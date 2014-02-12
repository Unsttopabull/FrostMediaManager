using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Common.Models.DB.MovieVo.Files;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditVideos.xaml</summary>
    public partial class ListVideos : UserControl {
        private ICollectionView _collectionView;

        public ListVideos() {
            InitializeComponent();
            
            TypeDescriptor.GetProperties(VideosList)["ItemsSource"].AddValueChanged(VideosList, VideosListItemSourceChanged); 
        }

        private void VideosListItemSourceChanged(object sender, EventArgs e) {
            _collectionView = CollectionViewSource.GetDefaultView(VideosList.ItemsSource);

            PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
            if (_collectionView.GroupDescriptions != null) {
                _collectionView.GroupDescriptions.Add(groupDescription);
            }            
        }

        private void OnEditClicked(object sender, RoutedEventArgs e) {
            Video selectedVideo = (Video) ((Button) sender).DataContext;

            Window window = Window.GetWindow(this);
            EditVideo editVideo = new EditVideo {
                Owner = window,
                DataContext = selectedVideo,
                SelectedLanguage = {
                    ItemsSource = ((CollectionViewSource)window.Resources["LanguagesSource"]).View
                }
            };

            editVideo.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }
    }

}