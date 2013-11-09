using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.XBMC {

    /// <summary>This table stores the URLs for movie art metadata.</summary>
    [Table("art")]
    public class XbmcArt : IEquatable<XbmcArt> {
        #region Constants

        /// <summary>The value of the <see cref="XbmcArt.Type"/> when this art is a thumbnail</summary>
        public const string THUMBNAIL = "thumb";

        /// <summary>The value of the <see cref="XbmcArt.Type"/> when this art is a fanart</summary>
        public const string FANART = "fanart";

        /// <summary>The value of the <see cref="XbmcArt.Type"/> when this art is a poster</summary>
        public const string POSTER = "poster";

        /// <summary>The value of the <see cref="XbmcArt.MediaType"/> when this art is for an actor</summary>
        public const string ACTOR = "actor";

        /// <summary>The value of the <see cref="XbmcArt.MediaType"/> when this art is for a movie</summary>
        public const string MOVIE = "movie";

        /// <summary>The value of the <see cref="XbmcArt.MediaType"/> when this art is for a set</summary>
        public const string SET = "set";

        #endregion

        /// <summary>Gets or sets the database Art Id.</summary>
        /// <value>The database Art Id</value>
        [Key]
        [Column("art_id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the Id of the Actor/Movie/Set referenced</summary>
        /// <value>Id of the Actor/Movie/Set referenced</value>
        [Column("media_id")]
        public long MediaID { get; set; }

        /// <summary>Gets or sets the Media type (Actor/Movie/Set)</summary>
        /// <value>Media type (Actor/Movie/Set)</value>
        /// <example>\eg{''<c>actor</c>''}</example>
        /// <seealso cref="ACTOR"/>
        /// <seealso cref="MOVIE"/>
        /// <seealso cref="SET"/>
        [Column("media_type")]
        public string MediaType { get; set; }

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

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcArt other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return MediaID == other.MediaID &&
                   MediaType == other.MediaType &&
                   Type == other.Type &&
                   Url == other.Url;
        }

    }

}
