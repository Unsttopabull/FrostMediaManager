using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Models.Frost.DB.Files;
using RibbonUI.Util;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditAudios.xaml</summary>
    public partial class ListAudios : UserControl {
        public static readonly DependencyProperty AudiosProperty = DependencyProperty.Register("Audios", typeof(ObservableHashSet2<Audio>), typeof(ListAudios), new PropertyMetadata(default(ObservableHashSet2<Audio>)));
        private ICollectionView _collectionView;

        public ListAudios() {
            InitializeComponent();
            TypeDescriptor.GetProperties(AudiosList)["ItemsSource"].AddValueChanged(AudiosList, SubtitlesListItemSourceChanged); 
        }

        public ObservableHashSet2<Audio> Audios {
            get { return (ObservableHashSet2<Audio>) GetValue(AudiosProperty); }
            set { SetValue(AudiosProperty, value); }
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
                SelectedAudio = selectedAudio,
                SelectedLanguage = {
                    ItemsSource = ((CollectionViewSource) window.Resources["LanguagesSource"]).View
                }
            };

            editAudio.ShowDialog();

            _collectionView.Refresh();
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }
    }
}
