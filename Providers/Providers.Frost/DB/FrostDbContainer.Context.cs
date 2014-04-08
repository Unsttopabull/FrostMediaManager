using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.DB.People;
using Frost.Providers.Frost.Properties;

namespace Frost.Providers.Frost.DB {

    /// <summary>Represents a context used for manipulation of the database.</summary>
    public class FrostDbContainer : DbContext {

        /// <summary>Initializes a new instance of the <see cref="FrostDbContainer"/> class.</summary>
        public FrostDbContainer(bool dropCreate, string filePath) : base(GetSQLiteConnection(filePath), true) {
            Database.SetInitializer(new SQLiteInitializer<FrostDbContainer>(Resources.MovieVoSQL, dropCreate));
        }

        /// <summary>Initializes a new instance of the <see cref="FrostDbContainer"/> class.</summary>
        public FrostDbContainer(string connectionString, bool dropCreate = true) : base(connectionString) {
            Database.SetInitializer(new SQLiteInitializer<FrostDbContainer>(Resources.MovieVoSQL, dropCreate));
        }

        public FrostDbContainer(SQLiteConnection conn) : base(conn, false) {
            Database.SetInitializer(new SQLiteInitializer<FrostDbContainer>(Resources.MovieVoSQL, true));
        }

        /// <summary>Initializes a new instance of the <see cref="FrostDbContainer"/> class.</summary>
        public FrostDbContainer(bool dropCreate = true) : this("name=MovieVoContainer", dropCreate) {
        }

        public FrostDbContainer() : this("name=MovieVoContainer", false) {
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
        public DbSet<Art> Art { get; set; }

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

        /// <summary>Checks if the context has unsaved changed (added, modified or deleted entites)</summary>
        /// <returns>Returns <c>true</c> if unsaved changes exist; otherwise <c>false</c>.</returns>
        public bool HasUnsavedChanges() {
            ChangeTracker.DetectChanges();

            return ChangeTracker.Entries().Any(e => e.State == EntityState.Added
                                                    || e.State == EntityState.Modified
                                                    || e.State == EntityState.Deleted);
        }

        /// <summary>Checks if the context has unsaved changed (added, modified or deleted entites)</summary>
        /// <returns>Returns <c>true</c> if unsaved changes exist; otherwise <c>false</c>.</returns>
        public IEnumerable<DbEntityEntry> GetUnsavedEntites() {
            ChangeTracker.DetectChanges();
            return ChangeTracker.Entries().Where(e => e.State == EntityState.Added
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

        #region Find methods

        internal Subtitle FindOrCreateSubtitle(ISubtitle subtitle, bool createIfNotFound) {
            Subtitle p;
            if (subtitle.Id > 0) {
                p = Subtitles.Find(subtitle.Id);
                if (p == null || (subtitle.MD5 != null && p.MD5 != subtitle.MD5)) {
                    if (createIfNotFound) {
                        p = Subtitles.Add(new Subtitle(subtitle));
                    }
                    else {
                        return null;
                    }
                }
                return p;
            }

            p = Subtitles
                .FirstOrDefault(pr =>
                                (subtitle.MD5 != null && pr.MD5 == subtitle.MD5) ||
                                (subtitle.OpenSubtitlesId != null && pr.OpenSubtitlesId == subtitle.OpenSubtitlesId) ||
                                (subtitle.PodnapisiId != null && pr.PodnapisiId == subtitle.PodnapisiId) ||
                                (subtitle.EmbededInVideo == pr.EmbededInVideo &&
                                 subtitle.ForHearingImpaired == pr.ForHearingImpaired &&
                                 subtitle.Encoding == pr.Encoding));

            if (p == null && createIfNotFound) {
                Subtitles.Add(new Subtitle(subtitle));
            }
            return p;
        }


        internal Plot FindPlot(IPlot plot, bool createIfNotFound) {
            Plot p;
            if (plot.Id > 0) {
                p = Plots.Find(plot.Id);
                if (p == null || (p.Full != plot.Full)) {
                    if (createIfNotFound) {
                        p = Plots.Add(new Plot(plot));
                    }
                    else {
                        return null;
                    }
                }
                return p;
            }

            p = Plots.FirstOrDefault(pr => plot.Full == pr.Full) ?? Plots.Add(new Plot(plot));
            return p;
        }

        internal Person FindPerson(IPerson person, bool createIfNotFound) {
            if (person == null) {
                return null;
            }

            Person p;
            if (person.Id > 0) {
                p = People.Find(person.Id);
                if (p != null && p.Name == person.Name) {
                    return p;
                }

                return createIfNotFound
                           ? People.Add(new Person(person))
                           : null;
            }

            p = People.FirstOrDefault(pr => (person.ImdbID != null && pr.ImdbID == person.ImdbID) || pr.Name == person.Name);
            if (p == null && createIfNotFound) {
                p = People.Add(new Person(person));
            }
            return p;
        }

        internal Country FindCountry(ICountry country, bool createIfNotFound) {
            Country c;
            if (country.Id > 0) {
                c = Countries.Find(country.Id);
                if (c == null || (c.Name != country.Name)) {
                    if (createIfNotFound) {
                        c = Countries.Add(new Country(country));
                    }
                    else {
                        return null;
                    }
                }
                return c;
            }

            c = Countries.FirstOrDefault(pr => (country.ISO3166 != null && pr.ISO3166.Alpha3 == country.ISO3166.Alpha3) || pr.Name == country.Name);
            if (c == null && createIfNotFound) {
                Countries.Add(new Country(country));
            }
            return c;
        }

        internal Language FindLanguage(ILanguage language, bool createIfNotFound) {
            Language c;
            if (language.Id > 0) {
                c = Languages.Find(language.Id);
                if (c == null || (c.Name != language.Name)) {
                    if (createIfNotFound) {
                        c = Languages.Add(new Language(language));
                    }
                    else {
                        return null;
                    }
                }
                return c;
            }

            c = Languages.FirstOrDefault(pr => (language.ISO639 != null && pr.ISO639.Alpha3 == language.ISO639.Alpha3) || pr.Name == language.Name);
            if (c == null) {
                return null;
            }
            return Languages.Add(new Language(language));
        }

        internal TSet FindHasName<TEntity, TSet>(TEntity hasName, bool createIfNotFound) where TEntity : class, IHasName, IMovieEntity
            where TSet : class, IHasName, IMovieEntity {
            DbSet<TSet> set = Set<TSet>();
            if (hasName.Id > 0) {
                //check if the entity is already in the context otherwise retreive it
                TSet find = set.Find(hasName.Id);
                if ((find == null || find.Name != hasName.Name)) {
                    if (createIfNotFound) {
                        find = set.Create();
                        find.Name = hasName.Name;

                        find = set.Add(find);
                    }
                    else {
                        return null;
                    }
                }
                return find;
            }

            //primary key not available so search in the DB by Name
            TSet hn = set.FirstOrDefault(n => n.Name == hasName.Name);
            if (hn == null && createIfNotFound) {
                hn = set.Create();
                hn.Name = hasName.Name;

                hn = set.Add(hn);
            }
            return hn;
        }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.ComplexType<ISO3166>()
                        .Ignore(iso => iso.EnglishName);

            modelBuilder.ComplexType<ISO639>()
                        .Ignore(iso => iso.EnglishName);

            modelBuilder.Configurations.Add(new Plot.Configuration());
            modelBuilder.Configurations.Add(new Actor.Configuration());
            modelBuilder.Configurations.Add(new File.Configuration());
            modelBuilder.Configurations.Add(new Language.Configuration());
            modelBuilder.Configurations.Add(new Audio.Configuration());
            modelBuilder.Configurations.Add(new Video.Configuration());
            modelBuilder.Configurations.Add(new Certification.Configuration());
            modelBuilder.Configurations.Add(new Movie.Configuration());
            modelBuilder.Configurations.Add(new Art.Configuration());
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

        public override int SaveChanges() {
            try {
                EfLogger.LogChanges(this, "frost.log");
            }
            catch (Exception e) {
            }

            List<DbEntityValidationResult> errors = GetValidationErrors().ToList();

            return base.SaveChanges();
        }
    }

}