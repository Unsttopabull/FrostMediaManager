﻿using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common.Util.ISO;
using Frost.XamlControls.Commands;
using RibbonUI.Design.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.Add {

    /// <summary>Interaction logic for SelectLanguages.xaml</summary>
    public partial class SelectLanguages : Window {
        private readonly List<MovieLanguage> _languages;
        private IEnumerable<MovieLanguage> _availableLanguages;
        private ICommand _addLanguagesCommand;
        private ICommand _removeLanguagesCommand;
        private ICollectionView _collectionView;

        public SelectLanguages() {
            _languages = new List<MovieLanguage>();

            InitializeComponent();
        }

        public IEnumerable<MovieLanguage> AvailableLanguages {
            get {
                if (_availableLanguages == null) {
                    if (Directory.Exists("Images/Languages")) {
                        var langs = UIHelper.GetLanguagesWithImages();

                        _availableLanguages = langs.Select(iso => new MovieLanguage(new DesignLanguage(iso)));
                    }
                    else {

                        _availableLanguages = UIHelper.Languages;
                    }

                    _collectionView = CollectionViewSource.GetDefaultView(_availableLanguages);
                    _collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
                return _availableLanguages;
            }
            set {
                _availableLanguages = value;
                if (_availableLanguages != null) {
                    _collectionView = CollectionViewSource.GetDefaultView(_availableLanguages);
                    _collectionView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                }
            }
        }

        public IEnumerable<MovieLanguage> Languages {
            get { return _languages; }
        }

        public ICommand AddLanguageCommand {
            get {
                if (_addLanguagesCommand == null) {
                    _addLanguagesCommand = new RelayCommand<MovieLanguage>(AddLanguage, language => language != null);
                }
                return _addLanguagesCommand;
            }
            set { _addLanguagesCommand = value; }
        }

        public ICommand RemoveLanguageCommand {
            get {
                if (_removeLanguagesCommand == null) {
                    _removeLanguagesCommand = new RelayCommand<MovieLanguage>(RemoveLanguage, language => language != null);
                }
                return _removeLanguagesCommand;
            }
            set { _removeLanguagesCommand = value; }
        }

        private void RemoveLanguage(MovieLanguage lang) {
            _languages.Remove(lang);
        }

        private void AddLanguage(MovieLanguage lang) {
            _languages.Add(lang);
        }

        private void OnCloseClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
