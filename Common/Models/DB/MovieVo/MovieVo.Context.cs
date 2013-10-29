using System.Data.Entity;
using Common.Models.DB.MovieVo.Arts;

namespace Common.Models.DB.MovieVo {

    public class MovieVoContainer : DbContext {
        public MovieVoContainer() : base("name=MovieVoContainer") {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();


            modelBuilder.Entity<Art>()
                        .Map<Cover>(m => m.Requires("Type").HasValue(1))
                        .Map<Poster>(m => m.Requires("Type").HasValue(2))
                        .Map<Fanart>(m => m.Requires("Type").HasValue(3));

            //---------------------------------------------------------------//

            modelBuilder.Entity<Special>()
                        .HasMany(s => s.Movies)
                        .WithMany(m => m.Specials)
                        .Map(m => {
                            m.ToTable("MovieSpecials");
                            m.MapLeftKey("SpecialId");
                            m.MapRightKey("MovieId");
                        });

            //-----------------------------------------------------------------//

            // Movie <--> Director link
            modelBuilder.Entity<Person>()
                        .HasMany(p => p.MoviesAsDirector)
                        .WithMany(m => m.Directors)
                        .Map(m => {
                            m.ToTable("MovieDirectors");
                            m.MapLeftKey("DirectorId");
                            m.MapRightKey("MovieId");
                        });

            // Movie <--> Writer link
            modelBuilder.Entity<Person>()
                        .HasMany(p => p.MoviesAsWriter)
                        .WithMany(m => m.Writers)
                        .Map(m => {
                            m.ToTable("MovieWriters");
                            m.MapLeftKey("WriterId");
                            m.MapRightKey("MovieId");
                        });

            // Movie <--> Actor link
            modelBuilder.Entity<Person>()
                        .HasMany(p => p.MoviesAsWriter)
                        .WithMany(m => m.Writers)
                        .Map(m => {
                            m.ToTable("MovieWriters");
                            m.MapLeftKey("WriterId");
                            m.MapRightKey("MovieId");
                        });

            //----------------------------------------------------//

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

            //Database.SetInitializer(new SeedInitializer());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Audio> AudioDetails { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Video> VideoDetails { get; set; }
        public DbSet<Art> Art { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Plot> Plots { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Person> People { get; set; }
    }
}
