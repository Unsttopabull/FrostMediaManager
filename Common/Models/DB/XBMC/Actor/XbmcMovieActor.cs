using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.Actor {
    [NotMapped]
    public class XbmcMovieActor {

        public XbmcMovieActor(long id, string name, string thumbXml, string role) {
            Id = id;
            Name = name;
            ThumbXml = thumbXml;
            Role = role;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public string ThumbXml { get; set; }

        public string ThumbURL {
            get {
                return ThumbXml != null
                               ? ThumbXml.Replace("<thumb>", "").Replace("</thumb>", "")
                               : null;
            }
            set { ThumbXml = "<thumb>" + value + "</thumb>"; }
        }       
    }
}