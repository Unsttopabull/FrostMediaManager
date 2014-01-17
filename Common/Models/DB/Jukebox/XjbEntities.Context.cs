using System.Data.Entity;
using System.Data.SQLite;

namespace Frost.Common.Models.DB.Jukebox {

    public class XjbEntities : DbContext {

        /// <summary>Initializes a new instance of the <see cref="XjbEntities"/> class.</summary>
        public XjbEntities() : base("name=xjbEntities") {
        }

        /// <summary>Initializes a new instance of the <see cref="XjbEntities"/> class.</summary>
        /// <param name="filePath">The path to the SQLite database file.</param>
        public XjbEntities(string filePath) : base(new SQLiteConnection("data source="+filePath), true) {
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
