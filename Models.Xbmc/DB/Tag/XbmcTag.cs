using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Providers.Xbmc.DB.Tag {

    /// <summary>Represents a table that lists tags.</summary>
    [Table("tag")]
    public class XbmcTag {

        /// <summary>Gets or sets the Id of the Tag in the database.</summary>
        /// <value>The Id of the Tag in the database.</value>
        [Key]
        [Column("idTag")]
        public long Id { get; set; }

        /// <summary>Gets or sets the name of the tag.</summary>
        /// <value>The name of the tag.</value>
        [Column("strTag")]
        public string Name { get; set; }

    }

}
