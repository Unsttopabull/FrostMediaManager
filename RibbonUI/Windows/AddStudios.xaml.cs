using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Models.Frost.DB;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddStudios : Window {
        private ICollectionView _collectionView;

        public AddStudios() {
            InitializeComponent();

            Observable.FromEventPattern<TextChangedEventArgs>(SearchBox, "TextChanged")
                      .Throttle(TimeSpan.FromSeconds(0.5))
                      .ObserveOn(SynchronizationContext.Current)
                      .Subscribe(args => _collectionView.Refresh());

            Observable.FromEventPattern<TextChangedEventArgs>(NewStudioName, "TextChanged")
                      .Throttle(TimeSpan.FromSeconds(0.5))
                      .ObserveOn(SynchronizationContext.Current)
                      .Subscribe(CheckStudioExists);
        }

        private void CheckStudioExists(EventPattern<TextChangedEventArgs> args) {
            string newStudio = NewStudioName.Text;
            if (StudiosList.ItemsSource
                           .Cast<Studio>()
                           .Any(studio => studio.Name.Equals(newStudio, StringComparison.CurrentCultureIgnoreCase))
                ) {
                Error.Visibility = Visibility.Visible;
            }
            else if(Error.Visibility == Visibility.Visible){
                Error.Visibility = Visibility.Collapsed;
            }
        }

        private void AddOnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void CancelOnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        private void StudiosList_Loaded(object sender, RoutedEventArgs e) {
            if (StudiosList.ItemsSource == null) {
                return;
            }

            _collectionView = CollectionViewSource.GetDefaultView(StudiosList.ItemsSource);
            _collectionView.Filter = Filter;
        }

        private bool Filter(object obj) {
            Studio p = (Studio) obj;

            return p.Name.IndexOf(SearchBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void StudiosListSelectedChanged(object sender, SelectionChangedEventArgs e) {
            NewStudioName.Text = null;
        }

        private void NewStudioNameOnTextChanged(object sender, TextChangedEventArgs e) {
            if (StudiosList.SelectedIndex != -1) {
                StudiosList.SelectedIndex = -1;
            }
        }
    }

}