﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Frost.Models.Frost.DB;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {
        internal readonly MovieVoContainer Container;
        private readonly TextWriterTraceListener _logger;

        public MainWindow() {
            InitializeComponent();

            _logger = new TextWriterTraceListener(File.Create("dbLog.txt"));

            Container = new MovieVoContainer();
            Container.Database.Log = s => _logger.WriteLine(s);
        }

        private void RibbonWindowLoaded(object sender, RoutedEventArgs e) {
            Container.Movies.Load();
                     //.Include("Studios")
                     //.Include("Art")
                     //.Include("Genres")
                     //.Include("Awards")
                     //.Include("ActorsLink")
                     //.Include("Plots")
                     //.Include("Directors")
                     //.Include("Countries")
                     //.Include("Audios")
                     //.Include("Videos").Load();

            Container.Genres.Load();
            Container.Countries.Load();
            Container.Languages.Load();
            Container.People.Load();
            Container.Studios.Load();
            
            CollectionViewSource movieSource = (CollectionViewSource) (FindResource("MoviesSource"));
            if (movieSource != null) {
                movieSource.Source = Container.Movies.Local;
            }

            CollectionViewSource genreSurce = (CollectionViewSource) (FindResource("GenreSource"));
            if (genreSurce != null) {
                genreSurce.Source = Container.Genres.Local;
            }

            CollectionViewSource countriesSource = (CollectionViewSource) (FindResource("CountriesSource"));
            if (countriesSource != null) {
                countriesSource.Source = Container.Countries.Local;
            }

            CollectionViewSource languageSource = (CollectionViewSource) (FindResource("LanguagesSource"));
            if (languageSource != null) {
                languageSource.Source = Container.Languages.Local;
            }

            CollectionViewSource peopleSource = (CollectionViewSource) (FindResource("PeopleSource"));
            if (peopleSource != null) {
                peopleSource.Source = Container.People.Local;
            }

            CollectionViewSource studiosSource = (CollectionViewSource) (FindResource("StudiosSource"));
            if (studiosSource != null) {
                studiosSource.Source = Container.Studios.Local;
            }

            _logger.WriteLine("End startup load");
        }

        /// <summary>Raises the <see cref="E:System.Windows.Window.Closing"/> event.</summary>
        /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);


            IEnumerable<DbEntityEntry> unsaved = Container.GetUnsavedEntites();
            DbEntityEntry first = unsaved.FirstOrDefault();

            foreach (string propertyName in first.CurrentValues.PropertyNames) {
                object currentValue = first.CurrentValues[propertyName];
                object originalValue = first.OriginalValues[propertyName];
            }

            if (!Container.HasUnsavedChanges()) {
                return;
            }

            if (MessageBox.Show("There are unsaved changes, save?", "Unsaved changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                Container.SaveChanges();
            }

            Container.Dispose();
            _logger.Close();
        }
    }

}