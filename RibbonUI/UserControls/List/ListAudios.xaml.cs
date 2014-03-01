using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Models.Frost.DB.Files;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditAudios.xaml</summary>
    public partial class ListAudios : UserControl {
        private ICollectionView _collectionView;

        public ListAudios() {
            InitializeComponent();
            TypeDescriptor.GetProperties(AudiosList)["ItemsSource"].AddValueChanged(AudiosList, SubtitlesListItemSourceChanged); 
        }

        private void SubtitlesListItemSourceChanged(object sender, EventArgs e) {
            _collectionView = CollectionViewSource.GetDefaultView(AudiosList.ItemsSource);

            if (_collectionView == null) {
                return;
            }

            PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
            if (_collectionView.GroupDescriptions != null) {
                _collectionView.GroupDescriptions.Add(groupDescription);
            }            
        }

        private void OnEditClicked(object sender, RoutedEventArgs e) {
            Audio selectedAudio = (Audio) ((Button) sender).DataContext;

            Window window = Window.GetWindow(this);
            EditAudio editAudio = new EditAudio {
                Owner = window,
                DataContext = selectedAudio,
                SelectedLanguage = {
                    ItemsSource = ((CollectionViewSource)window.Resources["LanguagesSource"]).View
                }
            };

            editAudio.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }
    }
}
