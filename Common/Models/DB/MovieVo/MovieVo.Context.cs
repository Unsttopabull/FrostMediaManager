using System;
using System.Data.Entity;
using System.Reflection;
using Frost.Common.Models.DB.MovieVo.Arts;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Models.DB.MovieVo.People;

namespace Frost.Common.Models.DB.MovieVo {

    /// <summary>Represents a context used for manipulation of the database.</summary>
    public class MovieVoContainer : DbContext {

        static MovieVoContainer() {
            if (Environment.Is64BitProcess) {
                Assembly.LoadFrom("x64/System.Data.SQLite.dll");
                Assembly.LoadFrom("x64/System.Data.SQLite.Linq.dll");
            }
            else {
                Assembly.LoadFrom("x86/System.Data.SQLite.dll");
                Assembly.LoadFrom("x86/System.Data.SQLite.Linq.dll");                
            }            
        }

        /// <summary>Initializes a new instance of the <see cref="MovieVoContainer"/> class.</summary>
        public MovieVoContainer() : base("name=MovieVoContainer") {
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
            modelBuilder.Configurations.Add(new Genre.GenreConfiguration());
            modelBuilder.Configurations.Add(new Subtitle.Configuration());

            Database.SetInitializer(new SeedInitializer());

            base.OnModelCreating(modelBuilder);
        }

    }

}
