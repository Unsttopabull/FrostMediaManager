using System.Data.Entity;

namespace Frost.Common.Models.DB.Jukebox {

    public class XjbEntities : DbContext {

        public XjbEntities() : base("name=xjbEntities") {
        }

        public XjbEntities(string connectionString) : base(connectionString) {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Configurations.Add(new XjbGenre.Configuration());
        }

        public DbSet<XjbDrive> Drives { get; set; }
        public DbSet<XjbGenre> Genres { get; set; }
        public DbSet<XjbMovie> Movies { get; set; }
        public DbSet<XjbMoviePerson> MoviesPersons { get; set; }
        public DbSet<XjbOption> Options { get; set; }
        public DbSet<XjbPerson> Persons { get; set; }

    }

}
