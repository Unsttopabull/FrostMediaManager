using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.DB.Art {

    /// <summary>This table stores the URLs for movie art metadata.</summary>
    [Table("art")]
    public abstract class XbmcArt : IArt {
        #region Constants

        /// <summary>The value of the <see cref="XbmcArt.Type"/> when this art is a thumbnail</summary>
        public const string THUMBNAIL = "thumb";

        /// <summary>The value of the <see cref="XbmcArt.Type"/> when this art is a fanart</summary>
        public const string FANART = "fanart";

        /// <summary>The value of the <see cref="XbmcArt.Type"/> when this art is a poster</summary>
        public const string POSTER = "poster";

        ///// <summary>The value of the <see cref="XbmcArt.MediaType"/> when this art is for an actor</summary>
        //public const string ACTOR = "actor";

        ///// <summary>The value of the <see cref="XbmcArt.MediaType"/> when this art is for a movie</summary>
        //public const string MOVIE = "movie";

        ///// <summary>The value of the <see cref="XbmcArt.MediaType"/> when this art is for a set</summary>
        //public const string SET = "set";

        #endregion

        /// <summary>Gets or sets the database Art Id.</summary>
        /// <value>The database Art Id</value>
        [Key]
        [Column("art_id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the type of the art thumb/fanart/poster</summary>
        /// <value>The type of the art thumb/fanart/poster</value>
        /// <example>\eg{''<c>thumb</c>''}</example>
        /// <seealso cref="THUMBNAIL"/>
        /// <seealso cref="FANART"/>
        /// <seealso cref="POSTER"/>
        [Column("type")]
        public string Type { get; set; }

        /// <summary>Gets or sets the path to the art (URL or path on drive/network or an image grab from a file)</summary>
        /// <value>Path to the art (URL or path on drive/network or an image grab from a file)</value>
        /// <example>\egb{
        /// 	<list type="bullet">
        /// 		<item><description>''<c>image://video@smb%3a%2f%2fMYXTREAMER%2fXtreamer_PRO%2fsda1%2fFilmi%2fCloud%20Atlas%20(2012)%20-%20Atlas%20oblakov%2fhdt.cloud.atlas.2012.1080p.bluray.x264.mkv/</c>''</description></item>
        /// 		<item><description>''<c>E:/Movies/The Great Gatsby/.actors/Leonardo_DiCaprio.jpg</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/True Grit/.actors/Matt_Damon.jpg</c>''</description></item>
        /// 		<item><description>''<c>http://cf2.imgobject.com/t/p/original/lovvVgGJ142gs5Qh7QLVyq7u0Hy.jpg</c>''</description></item>
        /// 	</list>}
        /// </example>
        [Column("url")]
        public string Url { get; set; }

        #region IArt

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Path":
                    case "Type":
                    case "PreviewOrPath":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        string IArt.Path {
            get { return Url; }
            set { Url = value; }
        }

        ArtType IArt.Type {
            get {
                switch (Type) {
                    case "fanart":
                        return ArtType.Fanart;
                    case "poster":
                        return ArtType.Poster;
                    case "thumb":
                        return ArtType.Unknown;
                    default:
                        return ArtType.Unknown;
                }
            }
        }

        string IArt.PreviewOrPath {
            get { return Url; }
        }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        string IArt.Preview {
            get { return default(string); }
            set { }
        }

        #endregion

        internal class Configuration : EntityTypeConfiguration<XbmcArt> {

            public Configuration() {
                Map<XbmcActorArt>(m => m.Requires("media_type").HasValue("actor"));
                Map<XbmcMovieArt>(m => m.Requires("media_type").HasValue("movie"));
                Map<XbmcSetArt>(m => m.Requires("media_type").HasValue("set"));
            }
        }
    }

}
