using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.XBMC.StreamDetails {

    /// <summary>Represents information about a subtitle in a file.</summary>
    public class XbmcSubtitleDetails : XbmcStreamDetails {

        /// <summary>Initializes a new instance of the <see cref="XbmcSubtitleDetails"/> class.</summary>
        public XbmcSubtitleDetails() {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcSubtitleDetails"/> class.</summary>
        /// <param name="file">The file this subtitle is contained in.</param>
        /// <param name="language">The language of this subtitle.</param>
        public XbmcSubtitleDetails(XbmcFile file, string language) : base(file) {
            Language = language;
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcSubtitleDetails"/> class.</summary>
        /// <param name="language">The language of this subtitle.</param>
        public XbmcSubtitleDetails(string language) {
            Language = language;
        }

        /// <summary>Gets or sets the language of this subtitle.</summary>
        /// <value>The language of this subtitle.</value>
        [Column("strSubtitleLanguage")]
        public string Language { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcSubtitleDetails other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Language == other.Language;
        }

    }

}
