using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC {

    [Table("path")]
    public class XbmcPath {

        public XbmcPath() {
            Movies = new HashSet<XbmcMovie>();
        }

        [Key]
        [Column("idPath")]
        public long Id { get; set; }

        [Column("strPath")]
        public string PathName { get; set; }

        [Column("strContent")]
        public string Content { get; set; }

        [Column("strScraper")]
        public string Scraper { get; set; }

        [Column("strHash")]
        public string Hash { get; set; }

        [Column("scanRecursive")]
        public long ScanRecursive { get; set; }

        [Column("useFolderNames")]
        public bool UseFolderNames { get; set; }

        [Column("strSettings")]
        public string Settings { get; set; }

        [Column("noUpdate")]
        public bool NoUpdate { get; set; }

        [Column("exclude")]
        public bool Exclude { get; set; }

        [Column("dateAdded")]
        public string DateAdded { get; set; }

        public virtual ICollection<XbmcMovie> Movies { get; set; }
    }
}