﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Frost.Common.Models;

namespace Frost.Models.Frost.DB {



    /// <summary>Represents a studio that prodcuced a movie.</summary>
    public class Studio : IStudio {

        /// <summary>Initializes a new instance of the <see cref="Studio"/> class.</summary>
        public Studio() {
            Movies = new HashSet<Movie>();
        }

        /// <summary>Initializes a new instance of the <see cref="Studio"/> class with the specified studio name.</summary>
        /// <param name="name">The name of the studio.</param>
        public Studio(string name) : this() {
            Name = name;
        }

        public Studio(IStudio studio) {
            //Contract.Requires<ArgumentNullException>(studio != null);
            //Contract.Requires<ArgumentNullException>(studio.Movies != null);

            Name = studio.Name;
        }

        /// <summary>Gets or sets the Id of this studio in the database.</summary>
        /// <value>The Id of this studio in the database</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the studio.</summary>
        /// <value>The name of the studio.</value>
        [Required]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies this studio has produced.</summary>
        /// <value>The movies this studio has produced.</value>
        public virtual ICollection<Movie> Movies { get; set; }

        /// <summary>Converts studio names to an <see cref="IEnumerable{T}"/> with elements of type <see cref="Studio"/></summary>
        /// <param name="studioNames">The studio names.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Studio"/> instances with specified studio names</returns>
        public static IEnumerable<Studio> GetFromNames(IEnumerable<string> studioNames) {
            return studioNames.Select(studioName => new Studio(studioName));
        }

        /// <summary>Converts the studio name to a <see cref="Studio"/> instance</summary>
        /// <param name="studioName">Name of the studio.</param>
        /// <returns>An instance of <see cref="Studio"/> with string as a studio name</returns>
        public static implicit operator Studio(string studioName) {
            return new Studio(studioName);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Name ?? base.ToString();
        }

        internal class Configuration : EntityTypeConfiguration<Studio> {

            public Configuration() {
                ToTable("Studios");

                //Join tabela za Movie <--> Studio
                HasMany(m => m.Movies)
                    .WithMany(g => g.Studios)
                    .Map(m => {
                        m.ToTable("MovieStudios");
                        m.MapLeftKey("StudioId");
                        m.MapRightKey("MovieId");
                    });
            }

        }
    }

}
