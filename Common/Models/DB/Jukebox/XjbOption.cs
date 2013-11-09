using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    /// <summary>Represent a list of options in a dictionary-like fashion.</summary>
    [Table("options")]
    public class XjbOption {

        /// <summary>Initializes a new instance of the <see cref="XjbOption"/> class.</summary>
        /// <param name="key">The key (what this option represents)</param>
        /// <param name="value">The value.</param>
        public XjbOption(string key, string value) {
            Key = key;
            Value = value;
        }

        /// <summary>Gets or sets the Id of this option in the database.</summary>
        /// <value>The Id of this option in the database</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the key (what this option represents).</summary>
        /// <value>The key (what this option represents)</value>
        [Column("key")]
        public string Key { get; set; }

        /// <summary>Gets or sets the value of this option.</summary>
        /// <value>The value of this option.</value>
        [Column("value")]
        public string Value { get; set; }

    }

}
