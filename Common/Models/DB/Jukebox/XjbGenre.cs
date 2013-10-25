using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    [Table("genres")]
    public class XjbGenre {

        public XjbGenre(string name) {
            Name = name;
        }

        [Key]
        [Column("id")]
        public long ID { get; set; }

        [Column("name")]
        public string Name { get; set; }
    }
}
