using System.Data.Entity.ModelConfiguration;

namespace Frost.Providers.Frost.DB {

    public partial class Movie {
        internal class Configuration : EntityTypeConfiguration<Movie> {
            public Configuration() {
                ToTable("Movies");

                //HasMany(m => m.Art)
                //    .WithRequired()
                //    .Map(fk => fk.MapKey("MovieId"));

				//Movie <--> Set
                HasOptional(m => m.Set)
                    .WithMany(s => s.Movies)
                    .HasForeignKey(movie => movie.SetId);

                HasOptional(m => m.MainPlot)
                    .WithOptionalDependent()
                    //.WithRequired()
                    .Map(fk => fk.MapKey("PlotId"));

                HasOptional(m => m.DefaultCover)
                    .WithOptionalDependent()
                    //.WithRequired()
                    .Map(fk => fk.MapKey("CoverId"));

                HasOptional(m => m.DefaultFanart)
                    .WithOptionalDependent()
                    //.WithRequired()
                    .Map(fk => fk.MapKey("FanartId"));

				//Movie <--> Ratings
                HasMany(m => m.Ratings)
                    .WithRequired(r => r.Movie)
                    .HasForeignKey(r => r.MovieId)
                    .WillCascadeOnDelete();

                //Movie <--> Plots
                //HasMany(m => m.Plots)
                //    //.WithRequired(p => p.Movie)
                //    .WithRequired()
                //    .Map(fk => fk.MapKey("MovieId"))
                //    //.HasForeignKey(p => p.MovieId)
                //    .WillCascadeOnDelete();
            }
        }
    }

}