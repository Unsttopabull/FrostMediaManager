using System;
using System.Data.Entity;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using Frost.Common;
using Frost.Common.Util;
using Frost.Models.Frost.DB.Arts;
using Frost.Models.Frost.DB.Files;
using Frost.Models.Frost.DB.People;
using Frost.Models.Frost.Properties;

namespace Frost.Models.Frost.DB {

    /// <summary>
    /// Represents a context used for manipulation of the database.
    /// </summary>
    public class MovieVoContainer : DbContext {

        /// <summary>Initializes a new instance of the <see cref="MovieVoContainer"/> class.</summary>
        public MovieVoContainer(bool dropCreate, string filePath) : base(GetSQLiteConnection(filePath), true) {
            if (dropCreate) {
                Database.SetInitializer(new SQLiteInitializer<MovieVoContainer>(Resources.MovieVoSQL));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="MovieVoContainer"/> class.</summary>
        public MovieVoContainer(string connectionString, bool dropCreate = true) : base(connectionString) {
            if (dropCreate) {
                Database.SetInitializer(new SQLiteInitializer<MovieVoContainer>(Resources.MovieVoSQL));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="MovieVoContainer"/> class.</summary>
        public MovieVoContainer(bool dropCreate = true) : this("name=MovieVoContainer", dropCreate) {
        }

        public MovieVoContainer() : this("name=MovieVoContainer", false) {
        }

        /// <summary>Gets or sets the information about the movies in the library.</summary>
        /// <value>The information about the movies in library.</value>
        public DbSet<Movie> Movies { get; set; }

        /// <summary>Gets or sets the information about files that contain the movies their subtitles in the library.</summary>
        /// <value>The information about files that contain the movies their subtitles in the library</value>
        public DbSet<File> Files { get; set; }

        /// <summary>Gets or sets the information about video streams in the library.</summary>
        /// <value>The information about video streams in the library.</value>
        public DbSet<Video> VideoDetails { get; set; }

        /// <summary>Gets or sets the information about audio streams in the library.</summary>
        /// <value>The information about audio streams in the library.</value>
        public DbSet<Audio> AudioDetails { get; set; }

        /// <summary>Gets or sets the information about subtitles of the movies in the library.</summary>
        /// <value>The information about subtitles of the movies in the library</value>
        public DbSet<Subtitle> Subtitles { get; set; }

        /// <summary>Gets or sets the information about promotional images in the library.</summary>
        /// <value>The information about promotional images in the library.</value>
        public DbSet<ArtBase> Art { get; set; }

        /// <summary>Gets or sets the information about contries the movies in the library were shot and/or produced in.</summary>
        /// <value>The information about contries the movies in the library were shot and/or produced in.</value>
        public DbSet<Country> Countries { get; set; }

        /// <summary>Gets or sets the information about studios that procuced the movies in the library.</summary>
        /// <value>The information about studios that procuced the movies in the library.</value>
        public DbSet<Studio> Studios { get; set; }

        /// <summary>Gets or sets the information the ratings of the movies in the library.</summary>
        /// <value>The information the ratings of the movies in the library</value>
        public DbSet<Rating> Ratings { get; set; }

        /// <summary>Gets or sets the story and plot information with summary and a tagline of the movies in the library.</summary>
        /// <value>The story and plot information with summary and a tagline of the movies in the library.</value>
        public DbSet<Plot> Plots { get; set; }

        /// <summary>Gets or sets the information about genres of the movies in the library</summary>
        /// <value>The information about genres of the movies in the library</value>
        public DbSet<Genre> Genres { get; set; }

        /// <summary>Gets or sets the information about movie awards</summary>
        /// <value>The information about the awards of movies in the library</value>
        public DbSet<Award> Awards { get; set; }

        /// <summary>Gets or sets the promotional videos of movies in the library.</summary>
        /// <value>The promotional videos of movies in the library.</value>
        public DbSet<PromotionalVideo> PromotionalVideos { get; set; }

        /// <summary>Gets or sets the certifications and/or restrictions in certain countries on the movies in the library.</summary>
        /// <value>The certifications and/or restrictions in certain countries on the movies in the library.</value>
        public DbSet<Certification> Certifications { get; set; }

        /// <summary>Gets or sets the information about movie collections and sets in the library.</summary>
        /// <value>The information about movie collections and sets in the library.</value>
        public DbSet<Set> Sets { get; set; }

        /// <summary>Gets or sets the information about a language.</summary>
        public DbSet<Language> Languages { get; set; }

        /// <summary>Gets or sets the scene release tags of a movie.</summary>
        /// <value>The information about scene release tags of a movie.</value>
        public DbSet<Special> Specials { get; set; }

        /// <summary>Gets or sets the information about people that participated in the movies in the library.</summary>
        /// <value>The information about people that participated in the movies in the library</value>
        public DbSet<Person> People { get; set; }

        /// <summary>Saves the movie in the database making sure we're not creating duplicates of the already existing entites.</summary>
        /// <param name="movie">The movie to save.</param>
        /// <exception cref="ArgumentNullException">Throws if the <paramref name="movie"/> is <c>null</c>.</exception>
        public void Save(Movie movie) {
            if (movie == null) {
                throw new ArgumentNullException("movie");
            }

            Movie mv = Movies.Create();
            CopyProperties(movie, mv);

            mv.Set = CheckSet(movie.Set);
            AddSubtitles(movie, mv.Subtitles);

            //Console.WriteLine(movie.Title);

            //CheckSet(movie);
            //CheckCountries(movie);
            //CheckStudios(movie);
            //CheckPeople(movie);
            ////CheckSpecials(movie);
            //CheckGenres(movie);
            //CheckAwards(movie);
            //CheckLanguages(movie);

            //Movies.Add(movie);
        }

        private void AddSubtitles(Movie movie, ObservableHashSet<Subtitle> subtitles) {
            foreach (Subtitle subtitle in movie.Subtitles) {
                if (subtitle.Language != null) {
                    subtitles.Add(subtitle);
                }

                Language lang = Languages.Local.FirstOrDefault(l => l.Name.Equals(subtitle.Language.Name, StringComparison.InvariantCultureIgnoreCase));
                if (lang == null) {
                    //subtitles.Add(new Subtitle())
                }
            }
        }

        private void CopyProperties(Movie mv, Movie copy) {
            copy.Title = mv.Title;
            copy.OriginalTitle = mv.OriginalTitle;
            copy.SortTitle = mv.SortTitle;
            copy.Type = mv.Type;
            copy.Goofs = mv.Goofs;
            copy.Trivia = mv.Trivia;
            copy.ReleaseYear = mv.ReleaseYear;
            copy.ReleaseDate = mv.ReleaseDate;
            copy.Edithion = mv.Edithion;
            copy.DvdRegion = mv.DvdRegion;
            copy.LastPlayed = mv.LastPlayed;
            copy.Premiered = mv.Premiered;
            copy.Aired = mv.Aired;
            copy.Trailer = mv.Trailer;
            copy.Top250 = mv.Top250;
            copy.Runtime = mv.Runtime;
            copy.Watched = mv.Watched;
            copy.PlayCount = mv.PlayCount;
            copy.RatingAverage = mv.RatingAverage;
            copy.ImdbID = mv.ImdbID;
            copy.TmdbID = mv.TmdbID;
            copy.ReleaseGroup = mv.ReleaseGroup;
            copy.IsMultipart = mv.IsMultipart;
            copy.PartTypes = mv.PartTypes;
            copy.DirectoryPath = mv.DirectoryPath;
            copy.NumberOfAudioChannels = mv.NumberOfAudioChannels;
            copy.AudioCodec = mv.AudioCodec;
            copy.VideoResolution = mv.VideoResolution;
            copy.VideoCodec = mv.VideoCodec;
        }

        #region Check methods

        private void CheckLanguages(Movie movie) {
            for (int i = 0; i < movie.Videos.Count; i++) {
                CheckLanguage(movie.Videos.ElementAt(i));
            }

            for (int i = 0; i < movie.Audios.Count; i++) {
                CheckLanguage(movie.Audios.ElementAt(i));
            }

            for (int i = 0; i < movie.Subtitles.Count; i++) {
                CheckLanguage(movie.Subtitles.ElementAt(i));
            }
        }

        private void CheckLanguage(IHasLanguage hasLanguage) {
            if (hasLanguage.Language == null) {
                return;
            }

            Language dbLanguage = Languages.Local.FirstOrDefault(l => l.Name == hasLanguage.Language.Name ||
                                                                      l.ISO639.Alpha3 == hasLanguage.Language.ISO639.Alpha3);
            if (dbLanguage == null) {
                Languages.Local.Add(hasLanguage.Language);
                return;
            }

            hasLanguage.Language = dbLanguage;
        }

        private void CheckAwards(Movie movie) {
            for (int i = 0; i < movie.Awards.Count; i++) {
                Award award = movie.Awards.ElementAt(i);
                Award dbAward = Awards.Local.FirstOrDefault(aw => aw.Organization == award.Organization &&
                                                                  aw.IsNomination == award.IsNomination &&
                                                                  aw.AwardType == award.AwardType);
                if (dbAward == null) {
                    continue;
                }

                movie.Awards.Remove(award);
                movie.Awards.Add(dbAward);
            }
        }

        private void CheckSpecials(Movie movie) {
            for (int i = 0; i < movie.Specials.Count; i++) {
                Special special = movie.Specials.ElementAt(i);

                if (Specials.Local.FirstOrDefault(s => s.Value == special.Value) == null) {
                    Specials.Local.Add(special);
                }
            }
        }

        private void CheckPeople(Movie movie) {
            for (int i = 0; i < movie.ActorsLink.Count; i++) {
                MovieActor movieActor = movie.ActorsLink.ElementAt(i);

                Person dbPerson = People.Local.FirstOrDefault(p => p.Name == movieActor.Person.Name || p.ImdbID == movieActor.Person.ImdbID);
                if (dbPerson == null) {
                    People.Local.Add(movieActor.Person);
                    continue;
                }

                movieActor.Person = dbPerson;
            }

            for (int i = 0; i < movie.Writers.Count; i++) {
                Person writer = movie.Writers.ElementAt(i);

                Person dbPerson = People.Local.FirstOrDefault(p => p.Name == writer.Name || p.ImdbID == writer.ImdbID);
                if (dbPerson == null) {
                    People.Local.Add(writer);
                    continue;
                }

                movie.Writers.Remove(writer);
                movie.Writers.Add(dbPerson);
            }

            for (int i = 0; i < movie.Directors.Count; i++) {
                Person director = movie.Directors.ElementAt(i);

                Person dbPerson = People.Local.FirstOrDefault(p => p.Name == director.Name || p.ImdbID == director.ImdbID);
                if (dbPerson == null) {
                    People.Local.Add(director);
                    continue;
                }

                movie.Directors.Remove(director);
                movie.Directors.Add(dbPerson);
            }
        }

        private void CheckGenres(Movie movie) {
            for (int i = 0; i < movie.Genres.Count; i++) {
                //Genre genre = movie.Genres.ElementAt(i);
                Genre genre = movie.Genres[i];

                Genre dbGenre = Genres.Local.FirstOrDefault(s => s.Name == genre.Name);
                if (dbGenre == null) {
                    Genres.Local.Add(genre);
                    continue;
                }

                //movie.Genres.Remove(genre);
                //movie.Genres.Add(dbGenre);
                movie.Genres[i] = dbGenre;
            }
        }

        private void CheckStudios(Movie movie) {

            for (int i = 0; i < movie.Studios.Count; i++) {
                //Studio studio = movie.Studios.ElementAt(i);
                Studio studio = movie.Studios[i];

                Studio dbStudio = Studios.Local.FirstOrDefault(s => s.Name == studio.Name);
                if (dbStudio == null) {
                    Studios.Local.Add(studio);
                    continue;
                }

                //movie.Studios.Remove(studio);
                //movie.Studios.Add(dbStudio);
                movie.Studios[i] = dbStudio;
            }
        }

        private Set CheckSet(Set set) {
            if (set == null) {
                return null;
            }

            Set dbSet = Sets.Local.FirstOrDefault(s => s.Name.Equals(set.Name, StringComparison.InvariantCultureIgnoreCase));
            return dbSet ?? set;
        }

        private void CheckCountries(Movie movie) {
            for (int i = 0; i < movie.Countries.Count; i++) {
                //Country country = movie.Countries.ElementAt(i);
                Country country = movie.Countries[i];

                Country dbCountry = Countries.Local.FirstOrDefault(c => c.Name.Equals(country.Name, StringComparison.OrdinalIgnoreCase));
                if (dbCountry == null) {
                    Countries.Local.Add(country);
                    continue;
                }

                movie.Countries[i] = dbCountry;
                //movie.Countries.Remove(country);
                //movie.Countries.Add(dbCountry);
            }
        }

        #endregion

        /// <summary>Checks if the context has unsaved changed (added, modified or deleted entites)</summary>
        /// <returns>Returns <c>true</c> if unsaved changes exist; otherwise <c>false</c>.</returns>
        public bool HasUnsavedChanges() {
            ChangeTracker.DetectChanges();
            return ChangeTracker.Entries().Any(e => e.State == EntityState.Added
                                                         || e.State == EntityState.Modified
                                                         || e.State == EntityState.Deleted);
        }

        private static SQLiteConnection GetSQLiteConnection(string filePath) {
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + filePath);
            sqliteConn.Trace += sqliteConn_Trace;
            return sqliteConn;
        }

        private static void sqliteConn_Trace(object sender, TraceEventArgs e) {
            Debug.WriteLine(e.Statement, "SQL");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new MovieActor.Configuration());
            modelBuilder.Configurations.Add(new File.Configuration());
            modelBuilder.Configurations.Add(new Language.Configuration());
            modelBuilder.Configurations.Add(new Audio.Configuration());
            modelBuilder.Configurations.Add(new Video.Configuration());
            modelBuilder.Configurations.Add(new Certification.Configuration());
            modelBuilder.Configurations.Add(new Movie.Configuration());
            modelBuilder.Configurations.Add(new ArtBase.Configuration());
            modelBuilder.Configurations.Add(new Special.Configuration());
            modelBuilder.Configurations.Add(new Person.Configuration());
            modelBuilder.Configurations.Add(new Country.CountryConfiguration());
            modelBuilder.Configurations.Add(new Studio.Configuration());
            modelBuilder.Configurations.Add(new Genre.Configuration());
            modelBuilder.Configurations.Add(new Award.Configuration());
            modelBuilder.Configurations.Add(new PromotionalVideo.Configuration());
            modelBuilder.Configurations.Add(new Subtitle.Configuration());

            base.OnModelCreating(modelBuilder);
        }
    }

}