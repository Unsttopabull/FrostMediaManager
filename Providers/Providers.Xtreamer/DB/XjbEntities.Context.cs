using System.Data.Entity;
using System.Data.SQLite;
using Frost.Providers.Xtreamer.Properties;

namespace Frost.Providers.Xtreamer.DB {

    public class XjbEntities : DbContext {

        /// <summary>Initializes a new instance of the <see cref="XjbEntities"/> class.</summary>
        public XjbEntities() : base("name=xjbEntities") {
            Database.SetInitializer(new SQLiteInitializer<XjbEntities>(Resources.FixXjbSQL));
        }

        /// <summary>Initializes a new instance of the <see cref="XjbEntities"/> class.</summary>
        /// <param name="filePath">The path to the SQLite database file.</param>
        public XjbEntities(string filePath) : base(new SQLiteConnection("data source="+filePath), true) {
            Database.SetInitializer(new SQLiteInitializer<XjbEntities>(Resources.FixXjbSQL));
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
