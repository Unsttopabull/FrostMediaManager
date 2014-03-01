﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Frost.Models.Frost.DB;
using RibbonUI.Windows;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {

        public Ribbon() {
            InitializeComponent();
        }

        private void OpenInFolder_Click(object sender, RoutedEventArgs e) {
            Movie movie = DataContext as Movie;

            if (movie != null) {
                string directory = movie.DirectoryPath;
                if (!string.IsNullOrEmpty(directory)) {
                    Process.Start(directory);
                }
            }
        }

        private void SearchClick(object sender, RoutedEventArgs e) {
            //using (MovieVoContainer mvc = new MovieVoContainer(true, "movieVo.db3")) {
            //    int count = mvc.Movies.Count();
            //}

            TestWindow tw = new TestWindow();
            Debug.Listeners.Add(tw.Listener);

            tw.Owner = Window.GetWindow(this);
            tw.ShowDialog();

            //((MainWindow)((Grid)Parent).Parent).ContentGrid.;
        }

        private void MenuItemOptionsOnClick(object sender, RoutedEventArgs e) {
            SettingsWindow sw = new SettingsWindow { Owner = Window.GetWindow(this) };
            sw.ShowDialog();
        }
    }

}