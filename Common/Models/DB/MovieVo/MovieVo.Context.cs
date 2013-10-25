using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Common.Models.DB.MovieVo {

    public class MovieVoContainer : DbContext {
        public MovieVoContainer() : base("name=MovieVoContainer") {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Join tabela za Movie <--> Country
            modelBuilder.Entity<Movie>()
                        .HasMany(m => m.Countries)
                        .WithMany(c => c.Movies)
                        .Map(m => {
                            m.ToTable("MovieCountries");
                            m.MapLeftKey("MovieId");
                            m.MapRightKey("CountryId");
                        });

            //Join tabela za Movie <--> Genre
            modelBuilder.Entity<Movie>()
                        .HasMany(m => m.Genres)
                        .WithMany(g => g.Movies)
                        .Map(m => {
                            m.ToTable("MovieGenres");
                            m.MapLeftKey("MovieId");
                            m.MapRightKey("GenreId");
                        });

            //Join tabela za Movie <--> Studio
            modelBuilder.Entity<Movie>()
                        .HasMany(m => m.Studios)
                        .WithMany(g => g.Movies)
                        .Map(m => {
                            m.ToTable("MovieStudios");
                            m.MapLeftKey("MovieId");
                            m.MapRightKey("StudioId");
                        });

            Database.SetInitializer(new SeedInitializer());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<Audio> Audio { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<Video> Video { get; set; }
        public DbSet<Art> Art { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Plot> Plot { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Certification> Certification { get; set; }
        public DbSet<MoviePerson> MoviePerson { get; set; }
    }
}
