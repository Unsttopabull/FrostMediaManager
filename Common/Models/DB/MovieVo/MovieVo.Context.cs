using System.Data.Entity;
using Common.Models.DB.MovieVo.Arts;
using Common.Models.DB.MovieVo.People;

namespace Common.Models.DB.MovieVo {

    public class MovieVoContainer : DbContext {
        public MovieVoContainer() : base("name=MovieVoContainer") {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new Art.ArtConfiguration());
            modelBuilder.Configurations.Add(new Special.SpecialConfiguration());
            modelBuilder.Configurations.Add(new Person.PersonConfiguration());
            modelBuilder.Configurations.Add(new Country.CountryConfiguration());
            modelBuilder.Configurations.Add(new Studio.StudioConfiguration());
            modelBuilder.Configurations.Add(new Genre.GenreConfiguration());

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
