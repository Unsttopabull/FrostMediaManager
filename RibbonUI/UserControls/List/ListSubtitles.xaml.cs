using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditSubtitles.xaml</summary>
    public partial class ListSubtitles : UserControl {
        public ListSubtitles() {
            InitializeComponent();

            TypeDescriptor.GetProperties(SubtitlesList)["ItemsSource"].AddValueChanged(SubtitlesList, SubtitlesListItemSourceChanged); 
        }

        private void SubtitlesListItemSourceChanged(object sender, EventArgs e) {
            CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(SubtitlesList.ItemsSource);

            if (view == null) {
                return;
            }

            PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
            if (view.GroupDescriptions != null) {
                view.GroupDescriptions.Add(groupDescription);
            }            
        }

        private void OnEditClicked(object sender, RoutedEventArgs e) {
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }
    }
}
