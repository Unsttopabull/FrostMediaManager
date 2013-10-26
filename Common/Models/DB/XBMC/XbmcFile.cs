using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.DB.XBMC.StreamDetails;

namespace Common.Models.DB.XBMC {

    [Table("files")]
    public class XbmcFile {

        public XbmcFile() {
            Path = new XbmcPath();
            //Movie = new XbmcMovie();
            StreamDetails = new HashSet<XbmcStreamDetails>();
        }

        public XbmcFile(string dateAdded, string lastPlayed, long? playCount) : this() {
            DateAdded = dateAdded;
            LastPlayed = lastPlayed;
            PlayCount = playCount;
        }

        // [ForeignKey("Movie")]
        [Column("idFile")]
        public long Id { get; set; }

        [Column("idPath")]
        [ForeignKey("Path")]
        public long PathId { get; set; }

        [Column("strFilename")]
        public string Filename { get; set; }

        [Column("playCount")]
        public long? PlayCount { get; set; }

        [Column("lastPlayed")]
        public string LastPlayed { get; set; }

        [Column("dateAdded")]
        public string DateAdded { get; set; }

        //[InverseProperty("File")]
        public virtual ICollection<XbmcStreamDetails> StreamDetails { get; set; }

        public XbmcPath Path { get; set; }

        //public virtual XbmcBookmark Bookmark { get; set; }

        //[Required]
        //public virtual XbmcMovie Movie { get; set; }
    }
}