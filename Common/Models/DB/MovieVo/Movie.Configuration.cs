﻿using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.MovieVo {

    public partial class Movie {
        internal class Configuration : EntityTypeConfiguration<Movie> {
            public Configuration() {
                ToTable("Movies");

				//Movie <--> Set
                HasOptional(m => m.Set)
                    .WithMany(s => s.Movies)
                    .HasForeignKey(movie => movie.SetId);

				//Movie <--> Ratings
                HasMany(m => m.Ratings)
                    .WithRequired(r => r.Movie)
                    .HasForeignKey(r => r.MovieId)
                    .WillCascadeOnDelete();

                //Movie <--> Plots
                HasMany(m => m.Plots)
                    .WithRequired(p => p.Movie)
                    .HasForeignKey(p => p.MovieId)
                    .WillCascadeOnDelete();
            }
        }
    }

}