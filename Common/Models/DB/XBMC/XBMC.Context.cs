using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Common.Models.DB.XBMC.Actor;
using Common.Models.DB.XBMC.StreamDetails;
using Common.Models.DB.XBMC.Tag;

namespace Common.Models.DB.XBMC {
    public class XbmcContainer : DbContext {
        public XbmcContainer()
            : base("name=XbmcEntities") {
        }

        public XbmcContainer(string connString)
            : base(connString) {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            const string FK_MOVIE = "idMovie";

            modelBuilder.Entity<XbmcStreamDetails>()
                        .Map<XbmcVideoDetails>(s => s.Requires("iStreamType").HasValue(0))
                        .Map<XbmcAudioDetails>(s => s.Requires("iStreamType").HasValue(1))
                        .Map<XbmcSubtitleDetails>(s => s.Requires("iStreamType").HasValue(2))
                        .HasRequired(sd => sd.File)
                        .WithMany(f => f.StreamDetails)
                        .Map(m => m.MapKey("idFile"));

            //-----------------------------------------------------------------//

            //modelBuilder.Entity<XbmcSet>()
            //            .HasMany(s => s.Movies)
            //            .WithOptional(m => m.Set)
            //            .HasForeignKey(m => m.SetId);

            //---------------------------------------------------------------------------//

            //modelBuilder.Entity<XbmcMovie>()
            //            .HasRequired(m => m.File)
            //            .WithRequiredPrincipal(f => f.Movie)
            //            .Map(m => m.MapKey("idFile"));

            //Join table Movie <--> Actors
            modelBuilder.Entity<XbmcMovie>()
                        .HasMany(p => p.Actors)
                        .WithMany(m => m.Movies)
                        .Map(m => {
                            m.ToTable("actorlinkmovie");
                            m.MapLeftKey(FK_MOVIE);
                            m.MapRightKey("idActor");
                        });

            //Join table Movie <--> Genre
            modelBuilder.Entity<XbmcMovie>()
                        .HasMany(m => m.Genres)
                        .WithMany(g => g.Movies)
                        .Map(m => {
                            m.ToTable("genrelinkmovie");
                            m.MapLeftKey(FK_MOVIE);
                            m.MapRightKey("idGenre");
                        });

            //Join table Movie <--> Country
            modelBuilder.Entity<XbmcMovie>()
                        .HasMany(m => m.Countries)
                        .WithMany(c => c.Movies)
                        .Map(m => {
                            m.ToTable("countrylinkmovie");
                            m.MapLeftKey(FK_MOVIE);
                            m.MapRightKey("idCountry");
                        });

            //Join table Movie <--> Country
            modelBuilder.Entity<XbmcMovie>()
                        .HasMany(m => m.Studios)
                        .WithMany(s => s.Movies)
                        .Map(m => {
                            m.ToTable("studiolinkmovie");
                            m.MapLeftKey(FK_MOVIE);
                            m.MapRightKey("idStudio");
                        });

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<XbmcMovie> Movies { get; set; }

        public DbSet<XbmcPerson> Persons { get; set; }

        public DbSet<XbmcFile> Files { get; set; }
        public DbSet<XbmcPath> Paths { get; set; }
        //public DbSet<XbmcStreamDetails> StreamDetails { get; set; }

        public DbSet<XbmcSet> Sets { get; set; }

        public DbSet<XbmcGenre> Genres { get; set; }
        public DbSet<XbmcStudio> Studios { get; set; }
        public DbSet<XbmcCountry> Countries { get; set; }
        public DbSet<XbmcTag> Tags { get; set; }

        public DbSet<XbmcArt> Art { get; set; }

    }
}
