using System.Data.Entity;
using System.Data.SQLite;
using Frost.Common.Util;
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
            modelBuilder.Configurations.Add(new XjbMoviePerson.Configuration());
        }

        public DbSet<XjbDrive> Drives { get; set; }
        public DbSet<XjbGenre> Genres { get; set; }
        public DbSet<XjbMovie> Movies { get; set; }
        public DbSet<XjbMoviePerson> MoviesPersons { get; set; }
        public DbSet<XjbOption> Options { get; set; }
        public DbSet<XjbPerson> People { get; set; }

        /// <summary>Saves all changes made in this context to the underlying database.</summary>
        /// <returns>The number of objects written to the underlying database. </returns>
        /// <exception cref="T:System.InvalidOperationException">Thrown if the context has been disposed.</exception>
        public override int SaveChanges() {
            EfLogger.LogChanges(this, "xt.log");

            return base.SaveChanges();
        }
    }

}
