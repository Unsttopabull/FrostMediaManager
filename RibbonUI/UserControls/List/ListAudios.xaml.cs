﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Common.Models.DB.MovieVo.Files;
using RibbonUI.Windows;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditAudios.xaml</summary>
    public partial class ListAudios : UserControl {
        public ListAudios() {
            InitializeComponent();
            TypeDescriptor.GetProperties(AudiosList)["ItemsSource"].AddValueChanged(AudiosList, SubtitlesListItemSourceChanged); 
        }

        private void SubtitlesListItemSourceChanged(object sender, EventArgs e) {
            CollectionView view = (CollectionView) CollectionViewSource.GetDefaultView(AudiosList.ItemsSource);

            if (view == null) {
                return;
            }

            PropertyGroupDescription groupDescription = new PropertyGroupDescription("File");
            if (view.GroupDescriptions != null) {
                view.GroupDescriptions.Add(groupDescription);
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
        }

        private void OnRemoveClicked(object sender, RoutedEventArgs e) {
            
        }
    }
}
