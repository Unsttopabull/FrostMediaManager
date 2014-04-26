using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util;
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

            //StreamWriter sw = new StreamWriter(System.IO.File.Create("frost.sql")) { AutoFlush = true };
            //Database.Log = s => {
            //    lock (sw) {
            //        sw.WriteLine(DateTime.Now);
            //        sw.WriteLine("----------------------");
            //        sw.WriteLine(s);
            //        sw.WriteLine();
            //    }
            //};
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

        private static SQLiteConnection GetSQLiteConnection(string filePath) {
            SQLiteConnection sqliteConn = new SQLiteConnection("data source=" + filePath);
            sqliteConn.Trace += sqliteConn_Trace;
            return sqliteConn;
        }

        private static void sqliteConn_Trace(object sender, TraceEventArgs e) {
            Debug.WriteLine(e.Statement, "SQL");
        }

        #region Find methods

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.ComplexType<ISO3166>()
                        .Ignore(iso => iso.EnglishName);

            modelBuilder.ComplexType<ISO639>()
                        .Ignore(iso => iso.EnglishName);

            modelBuilder.Configurations.Add(new Art.Configuration());
            //modelBuilder.Configurations.Add(new Plot.Configuration());
            modelBuilder.Configurations.Add(new Actor.Configuration());
            modelBuilder.Configurations.Add(new File.Configuration());
            modelBuilder.Configurations.Add(new Language.Configuration());
            modelBuilder.Configurations.Add(new Audio.Configuration());
            modelBuilder.Configurations.Add(new Video.Configuration());
            modelBuilder.Configurations.Add(new Certification.Configuration());
            modelBuilder.Configurations.Add(new Movie.Configuration());
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