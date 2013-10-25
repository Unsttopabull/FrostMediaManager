using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.Jukebox {

    [Table("drives")]
    public class XjbDrive
    {
        public XjbDrive(string size, string label, long? lastSeenTs) {
            Size = size;
            Label = label;
            LastSeenTs = lastSeenTs;
        }

        [Key]
        [Column("id")]
        public string ID { get; set; }

        [Column("size")]
        public string Size { get; set; }

        [Column("label")]
        public string Label { get; set; }

        [Column("last_seen_ts")]
        public long? LastSeenTs { get; set; }
    }
}
