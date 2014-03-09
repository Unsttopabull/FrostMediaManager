﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common;
using Frost.Common.Annotations;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost.DB;
using Frost.XamlControls.Commands;
using GalaSoft.MvvmLight;
using Microsoft.Win32;
using RibbonUI.Messages.Country;
using RibbonUI.Messages.Genre;
using RibbonUI.Messages.People;
using RibbonUI.Messages.Plot;
using RibbonUI.Messages.Studio;
using RibbonUI.Util.ObservableWrappers;
using RibbonUI.Windows;

namespace RibbonUI.ViewModels.UserControls {
    public class EditMovieViewModel : ViewModelBase {
        private IMovie _selectedMovie;
        private ObservableCollection<MoviePlot> _moviePlots;
        private ObservableCollection<ObservableActor> _actors;
        private ObservableCollection<ICountry> _countries;
        private ObservableCollection<MovieStudio> _studios;
        private ObservableCollection<IGenre> _genres;
        private ObservableCollection<MoviePerson> _directors;

        public EditMovieViewModel() {
            MoviePlots = new ObservableCollection<MoviePlot>();

            #region Commands

            AddStudioCommand = new RelayCommand(AddStudioOnClick);
            RemoveStudioCommand = new RelayCommand<MovieStudio>(
                RemoveStudio,
                studio => studio != null
            );
            EditStudioCommand = new RelayCommand<MovieStudio>(EditStudioOnClick, studio => studio != null);

            AddMovieGenreCommand = new RelayCommand<IGenre>(
                AddGenre,
                genre => genre != null
            );
            RemoveMovieGenreCommand = new RelayCommand<IGenre>(
                RemoveGenre,
                genre => genre != null
            );

            AddGenreCommand = new RelayCommand<ListCollectionView>(AddGenreClick);
            RemoveGenreCommand = new RelayCommand<IGenre>(RemoveGenreClick, genre => genre != null);
            EditGenreCommand = new RelayCommand<IGenre>(EditGenreOnClick, genre => genre != null);

            AddActorCommand = new RelayCommand(AddActorOnClick);
            RemoveActorCommand = new RelayCommand<ObservableActor>(
                RemoveActor,
                actor => actor != null
            );
            EditActorCommand = new RelayCommand<ObservableActor>(
                actor => new EditPerson { Owner = ParentWindow, SelectedPerson = actor }.ShowDialog(),
                actor => actor != null
            );

            SetPlotLanguageCommand = new RelayCommand<MoviePlot>(SetPlotLanguageClick, plot => plot != null);
            AddPlotCommand = new RelayCommand(AddPlot);
            RemovePlotCommand = new RelayCommand<MoviePlot>(RemovePotOnClick, plot => plot != null);

            AddCountryCommand = new RelayCommand(AddCountryOnClick);
            RemoveCountryCommand = new RelayCommand<ICountry>(
                RemoveCountry,
                country => country != null
            );

            AddDirectorCommand = new RelayCommand(AddDirectorOnClick);
            RemoveDirectorCommand = new RelayCommand<MoviePerson>(
                RemoveDirector,
                director => director != null
            );

            EditDirectorCommand = new RelayCommand<MoviePerson>(
                director => new EditPerson { Owner = ParentWindow, SelectedPerson = director }.ShowDialog(),
                director => director != null
            );

            TrailerSearchCommand = new RelayCommand(TrailerSearchOnClick);

            #endregion
        }

        public Window ParentWindow { get; set; }

        public IMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                if (_selectedMovie != null) {
                    MoviePlots = new ObservableCollection<MoviePlot>(_selectedMovie.Plots.Select(p => new MoviePlot(p)));
                    Actors = new ObservableCollection<ObservableActor>(_selectedMovie.Actors.Select(a => new ObservableActor(a)));
                    Countries = new ObservableCollection<ICountry>(_selectedMovie.Countries);
                    Studios = new ObservableCollection<MovieStudio>(_selectedMovie.Studios.Select(s => new MovieStudio(s)));
                    Genres = new ObservableCollection<IGenre>(_selectedMovie.Genres);
                    Directors = new ObservableCollection<MoviePerson>(_selectedMovie.Directors.Select(p => new MoviePerson(p)));
                }

                OnPropertyChanged();
            }
        }

        public ObservableCollection<MoviePlot> MoviePlots {
            get { return _moviePlots; }
            private set {
                if (Equals(value, _moviePlots)) {
                    return;
                }
                _moviePlots = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ObservableActor> Actors {
            get { return _actors; }
            set {
                if (Equals(value, _actors)) {
                    return;
                }
                _actors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ICountry> Countries {
            get { return _countries; }
            set {
                if (Equals(value, _countries)) {
                    return;
                }
                _countries = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MovieStudio> Studios {
            get { return _studios; }
            set {
                if (Equals(value, _studios)) {
                    return;
                }
                _studios = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IGenre> Genres {
            get { return _genres; }
            set {
                if (Equals(value, _genres)) {
                    return;
                }
                _genres = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MoviePerson> Directors {
            get { return _directors; }
            set {
                if (Equals(value, _directors)) {
                    return;
                }
                _directors = value;
                OnPropertyChanged();
            }
        }

        #region ICommands

        public ICommand AddStudioCommand { get; private set; }
        public RelayCommand<MovieStudio> RemoveStudioCommand { get; private set; }
        public RelayCommand<MovieStudio> EditStudioCommand { get; private set; }

        public RelayCommand<IGenre> AddMovieGenreCommand { get; private set; }
        public RelayCommand<IGenre> RemoveMovieGenreCommand { get; private set; }

        public ICommand<ListCollectionView> AddGenreCommand { get; private set; }
        public RelayCommand<IGenre> RemoveGenreCommand { get; private set; }
        public RelayCommand<IGenre> EditGenreCommand { get; private set; }

        public ICommand AddActorCommand { get; private set; }
        public ICommand<ObservableActor> RemoveActorCommand { get; private set; }
        public ICommand<ObservableActor> EditActorCommand { get; private set; }

        public ICommand<MoviePlot> SetPlotLanguageCommand { get; private set; }
        public ICommand AddPlotCommand { get; private set; }
        public ICommand<MoviePlot> RemovePlotCommand { get; private set; }

        public ICommand AddCountryCommand { get; private set; }
        public RelayCommand<ICountry> RemoveCountryCommand { get; private set; }

        public ICommand AddDirectorCommand { get; private set; }
        public RelayCommand<MoviePerson> RemoveDirectorCommand { get; private set; }
        public RelayCommand<MoviePerson> EditDirectorCommand { get; private set; }

        public ICommand TrailerSearchCommand { get; private set; }

        #endregion

        #region Country Handlers

        private void AddCountryOnClick() {
            AddCountries sc = new AddCountries {
                Owner = ParentWindow,
                Countries = (CollectionViewSource) ParentWindow.Resources["CountriesSource"]
            };

            if (sc.ShowDialog() != true) {
                return;
            }

            foreach (ICountry country in sc.SelectedCountry.SelectedItems) {
                if (!Countries.Contains(country)) {
                    Countries.Add(country);

                    MessengerInstance.Send(new AddCountryMessage(country));
                }
            }
        }

        private void RemoveCountry(ICountry country) {
            Countries.Remove(country);

            MessengerInstance.Send(new RemoveCountryMessage(country));
        }

        #endregion

        #region Directors Handlers

        private void AddDirectorOnClick() {
            AddPerson addPerson = new AddPerson { Owner = ParentWindow };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            IPerson director;
            if (addPerson.PersonName.Text != ((IPerson) addPerson.PeopleList.SelectedItem).Name) {
                director = ModelCreator.Create<IPerson>();
                director.Name = addPerson.PersonName.Text;
                director.Thumb = addPerson.PersonThumb.Text;
            }
            else {
                director = (IPerson) addPerson.PeopleList.SelectedItem;
            }

            Directors.Add(new MoviePerson(director));
            MessengerInstance.Send(new AddDirectorMessage(director));
        }

        private void RemoveDirector(MoviePerson director) {
            Directors.Remove(director);
            MessengerInstance.Send(new RemoveDirectorMessage(director.Person));
        }

        #endregion

        #region Genre Handlers

        private void AddGenreClick(ListCollectionView genres) {
            string genreName = InputBox.Show(ParentWindow, "Genre name:", "Add genre", "Add");

            if (!string.IsNullOrEmpty(genreName)) {
                if (genres.Cast<IGenre>().All(genre => !genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase))) {
                    IGenre newGenre = ModelCreator.Create<IGenre>();
                    newGenre = (Genre) genres.AddNewItem(new Genre(newGenre));
                    genres.CommitNew();

                    Genres.Add(newGenre);
                    MessengerInstance.Send(new AddGenreMessage(newGenre));

                    genres.Refresh();
                }
                else {
                    MessageBox.Show(ParentWindow, "Genre with specified name already exists.");
                }
            }
        }

        private void RemoveGenreClick(IGenre genre) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will remove this genre from all the movies in the collection.\n" +
                "To remove the genre from this movie only click \"No\" and just uncheck it\nin the list.\n\n" +
                "Do you really want to remove this genre?",
                "Genre remove", MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                CollectionViewSource itemsSource = (CollectionViewSource) ParentWindow.Resources["GenreSource"];
                ListCollectionView view = (ListCollectionView) itemsSource.View;

                view.Remove(genre);
            }
        }

        private void EditGenreOnClick(IGenre genre) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will edit this genre name in all the movies in the collection.\n" +
                "If you want to edit this genre only for this movie ucheck it and add a new genre.\n\n" +
                "Do you really want to edit this genre?",
                "Genre edit", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string genreName = InputBox.Show(ParentWindow, TranslationManager.T("Genre name:"), TranslationManager.T("Edit genre"), TranslationManager.T("Edit"), genre.Name);

            if (!genre.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase)) {
                genre.Name = genreName;
            }
        }

        private void RemoveGenre(IGenre genre) {
            Genres.Remove(genre);
            MessengerInstance.Send(new RemoveGenreMessage(genre));
        }

        private void AddGenre(IGenre genre) {
            Genres.Add(genre);
            MessengerInstance.Send(new AddGenreMessage(genre));
        }
        #endregion

        #region Plot Handlers

        private void SetPlotLanguageClick(MoviePlot moviePlot) {
            SelectLanguage sc = new SelectLanguage { Owner = ParentWindow, DataContext = ParentWindow.Resources["LanguagesSource"] };

            if (sc.ShowDialog() == true) {
                ILanguage selectedLang = (ILanguage) sc.SelectedLanguage.SelectedItem;
                moviePlot.Language = selectedLang.Name;
            }
        }

        private void RemovePotOnClick(MoviePlot moviePlot) {
            if (MessageBox.Show(ParentWindow, TranslationManager.T("Do you really want to remove plot: \"{0}\"?", moviePlot), TranslationManager.T("Confrim remove"), MessageBoxButton.YesNo) == MessageBoxResult.Yes) {
                MoviePlots.Remove(moviePlot);
                MessengerInstance.Send(new RemovePlotMessage(moviePlot.Plot));
            }
        }

        private void AddPlot() {
            IPlot plot = ModelCreator.Create<IPlot>();
            plot.Full = TranslationManager.T("Enter full description");

            MoviePlots.Add(new MoviePlot(plot));
            MessengerInstance.Send(new AddPlotMessage(plot));
        }

        #endregion

        #region Actors Handlers

        private void AddActorOnClick() {
            AddPerson addPerson = new AddPerson(true) { Owner = ParentWindow };

            if (addPerson.ShowDialog() != true) {
                return;
            }

            IPerson person = (IPerson) addPerson.PeopleList.SelectedItem;

            string addActorCharacter = addPerson.ActorCharacter.Text;

            IActor movieActor;
            if (addPerson.PersonName.Text != person.Name) {
                //the person/actor is not in the database yet

                movieActor = ModelCreator.Create<IActor>();
                movieActor.Name = addPerson.PersonName.Text;
                movieActor.Thumb = addPerson.PersonThumb.Text;
            }
            else {
                //the person/actor is already in the database

                if (string.IsNullOrEmpty(addActorCharacter)) {
                    //check if they have already been added as an actor without a character
                    movieActor = Actors.FirstOrDefault(ma => ma.Name == person.Name && ma.Character == null);
                    if (movieActor != null) {
                        MessageBox.Show(ParentWindow, "This actor with unspecified character already exists in the list. To add another role specify a character.");
                        return;
                    }

                    //else add it as actor with unspecified character
                    movieActor = ModelCreator.Create<IActor>();
                    movieActor.Name = person.Name;
                    movieActor.Thumb = person.Thumb;
                    movieActor.ImdbID = person.ImdbID;
                }
                else {
                    //check if they have already been added as an actor with this character
                    movieActor = Actors.FirstOrDefault(ma => ma.Name == person.Name && ma.Character == addActorCharacter);
                    if (movieActor != null) {
                        MessageBox.Show(ParentWindow, "This actor already exists in the list. To add another actor's role specify a diffrent character.");
                        return;
                    }

                    //else add them as an actor with the specified character
                    movieActor = ModelCreator.Create<IActor>();//new MovieActor(SelectedMovie, person, addActorCharacter);
                    movieActor.Name = person.Name;
                    movieActor.Thumb = person.Thumb;
                    movieActor.ImdbID = person.ImdbID;
                    movieActor.Character = addActorCharacter;
                }
            }

            Actors.Add(new ObservableActor(movieActor));
            MessengerInstance.Send(new AddActorMessage(movieActor));
        }

        private void RemoveActor(ObservableActor actor) {
            Actors.Remove(actor);
            MessengerInstance.Send(new RemoveActorMessage(actor.Actor));
        }

        #endregion

        #region Studio Handlers

        private void AddStudioOnClick() {
            AddStudios addStudios = new AddStudios { Owner = ParentWindow, DataContext = ParentWindow.Resources["StudiosSource"] };

            if (addStudios.ShowDialog() != true) {
                return;
            }

            if (addStudios.StudiosList.SelectedIndex == -1 && !string.IsNullOrEmpty(addStudios.NewStudioName.Text)) {
                IStudio studio = ModelCreator.Create<IStudio>();
                studio.Name = addStudios.NewStudioName.Text;

                Studios.Add(new MovieStudio(studio));
                MessengerInstance.Send(new AddStudioMessage(studio));
                return;
            }

            foreach (IStudio studio in addStudios.StudiosList.SelectedItems) {
                MovieStudio studio2 = Studios.FirstOrDefault(ms => ms.Studio == studio);
                if (studio2 == null) {
                    Studios.Add(new MovieStudio(studio));
                    MessengerInstance.Send(new AddStudioMessage(studio));
                }
            }
        }

        private void EditStudioOnClick(MovieStudio studio) {
            if (MessageBox.Show(ParentWindow,
                "Please note this will edit this studio name in all the movies in the collection.\n" +
                "If you want to edit studio name only for this movie remove this studio add a new one with desired name.\n\n" +
                "Do you really want to edit this studio?",
                "Studio edit", MessageBoxButton.YesNo) != MessageBoxResult.Yes) {
                return;
            }

            string studioName = InputBox.Show(ParentWindow, TranslationManager.T("Studio name:"), TranslationManager.T("Edit studio"), TranslationManager.T("Edit"), studio.Name);

            if (!studio.Name.Equals(studioName, StringComparison.CurrentCultureIgnoreCase)) {
                studio.Name = studioName;
            }
        }

        private void RemoveStudio(MovieStudio studio) {
            Studios.Remove(studio);

            MessengerInstance.Send(new RemoveStudioMessage(studio.Studio));
        }

        #endregion

        private void TrailerSearchOnClick() {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                SelectedMovie.Trailer = ofd.FileName;
            }      
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChangedHandler != null) {
                PropertyChangedHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
