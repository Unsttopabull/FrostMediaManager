using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Common.Models.DB.MovieVo.People;
using Microsoft.Win32;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddPerson : Window {
        private ICollectionView _collectionView;

        public AddPerson(bool isActor = false) {
            InitializeComponent();

            if (!isActor) {
                ActorCharacter.IsEnabled = false;
                CharacterLabel.IsEnabled = false;
            }

            Observable.FromEventPattern<TextChangedEventArgs>(SearchBox, "TextChanged")
                      .Throttle(TimeSpan.FromSeconds(0.5))
                      .ObserveOn(SynchronizationContext.Current)
                      .Subscribe(args => _collectionView.Refresh());
        }

        private void AddOnClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void CancelOnClick(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        private void PeopleListOnLoaded(object sender, RoutedEventArgs e) {
            ICollectionView view = PeopleList.ItemsSource as ICollectionView;
            if (view != null) {
                _collectionView = CollectionViewSource.GetDefaultView(view);
                _collectionView.Filter = Filter;
            }
        }

        private bool Filter(object obj) {
            Person p = (Person) obj;

            return p.Name.IndexOf(SearchBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void ThumbSearchOnClick(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                PersonThumb.Text = ofd.FileName;
            }
        }
    }

}