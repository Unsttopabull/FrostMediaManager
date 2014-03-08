using System;
using System.Windows;
using System.Windows.Controls;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddPerson : Window {
        
        public AddPerson(bool isActor = false) {
            InitializeComponent();

            if (!isActor) {
                ActorCharacter.IsEnabled = false;
                CharacterLabel.IsEnabled = false;
            }
        }

        private void AddPersonOnClosed(object sender, EventArgs e) {
            if (DataContext != null) {
                ((AddPersonViewModel) DataContext).Dispose();
            }
        }

        private void AddPersonOnLoaded(object sender, RoutedEventArgs e) {
            if (DataContext != null) {
                ((AddPersonViewModel) DataContext).ParentWindow = Owner;
            }
        }

        private void PeopleList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            PeopleList.ScrollIntoView(PeopleList.SelectedItem);
        }
    }

}