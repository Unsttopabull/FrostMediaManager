using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.StreamDetails {

    [Table("streamdetails")]
    public abstract class XbmcStreamDetails {

        [Column("idStream")]
        public long Id { get; set; }

        public virtual XbmcFile File { get; set; }
    }
}