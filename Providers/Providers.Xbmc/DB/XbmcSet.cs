using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.DB {

    /// <summary>Represents a table that lists movie sets and collections.</summary>
    [Table("sets")]
    public class XbmcSet : IMovieSet {

        /// <summary>Initializes a new instance of the <see cref="XbmcSet"/> class.</summary>
        public XbmcSet() {
            Movies = new HashSet<XbmcDbMovie>();
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcSet"/> class.</summary>
        /// <param name="name">The name.</param>
        public XbmcSet(string name) : this() {
            Name = name;
        }

        internal XbmcSet(IMovieSet set) {
            Name = set.Name;
        }

        /// <summary>Gets or sets the id of the set or collection in the database.</summary>
        /// <value>The id of the set or collection in the database</value>
        [Key]
        [Column("idSet")]
        public long Id { get; set; }

        /// <summary>Gets or sets the title of the set or collection</summary>
        /// <value>The title of the set or collection</value>
        [Column("strSet")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies contained in this set or collection.</summary>
        /// <value>The movies contained in this set or collection</value>
        public virtual HashSet<XbmcDbMovie> Movies { get; set; }

        public bool this[string propertyName] {
            get { return true; }
        }

        internal class Configuration : EntityTypeConfiguration<XbmcSet> {

            public Configuration() {
                HasMany(s => s.Movies)
                    .WithOptional(m => m.Set)
                    .HasForeignKey(s => s.SetId);
            }

        }

    }

}
