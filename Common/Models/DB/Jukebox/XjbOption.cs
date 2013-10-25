using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    [Table("options")]
    public class XjbOption {

        public XjbOption(string key, string value) {
            Key = key;
            Value = value;
        }

        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("key")]
        public string Key { get; set; }

        [Column("value")]
        public string Value { get; set; }
    }

}
