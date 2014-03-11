using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for SelectCountry.xaml</summary>
    public partial class AddPerson : Window {
        public static readonly DependencyProperty PeopleProperty = DependencyProperty.Register("People", typeof(IEnumerable<IPerson>), typeof(AddPerson), new PropertyMetadata(default(IEnumerable<IPerson>), OnPeopleChanged));

        public AddPerson(bool isActor = false) {
            InitializeComponent();

            if (!isActor) {
                ActorCharacter.IsEnabled = false;
                CharacterLabel.IsEnabled = false;
            }
        }

        public IEnumerable<IPerson> People {
            get { return (IEnumerable<IPerson>) GetValue(PeopleProperty); }
            set { SetValue(PeopleProperty, value); }
        }

        private void AddPersonOnClosed(object sender, EventArgs e) {
            if (DataContext != null) {
                ((AddPersonViewModel) DataContext).Dispose();
            }
        }

        private static void OnPeopleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            AddPerson addPerson = ((AddPerson) d);
            if (addPerson.DataContext != null) {
                ((AddPersonViewModel) addPerson.DataContext).People = (IEnumerable<IPerson>) e.NewValue;
            }
        }

        private void PeopleList_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            PeopleList.ScrollIntoView(PeopleList.SelectedItem);
        }
    }

}