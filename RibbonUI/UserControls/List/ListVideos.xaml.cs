using System.Windows;
using System.Windows.Controls;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditVideos.xaml</summary>
    public partial class ListVideos : UserControl {
        public ListVideos() {
            InitializeComponent();
            
            //TypeDescriptor.GetProperties(VideosList)["ItemsSource"].AddValueChanged(VideosList, VideosListItemSourceChanged); 
        }

        //private void VideosListItemSourceChanged(object sender, EventArgs e) {
        //    CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(VideosList.ItemsSource);

        //    PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
        //    if (view.GroupDescriptions != null) {
        //        view.GroupDescriptions.Add(groupDescription);
        //    }            
        //}

        //private void AddOnClick(object sender, RoutedEventArgs e) {
        //    MessageBox.Show(Window.GetWindow(this), "Add");
        //}

        //private void RemoveOnClick(object sender, RoutedEventArgs e) {
        //    MessageBox.Show(Window.GetWindow(this), "Remove");
        //}

        private void OnEditClicked(object sender, RoutedEventArgs e) {
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }
    }

}