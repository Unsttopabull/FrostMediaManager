using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models.Provider;
using Frost.Common.Util.ISO;
using Frost.Providers.Xbmc.DB.Proxy;

namespace Frost.Providers.Xbmc.DB.StreamDetails {

    /// <summary>Represents information about a subtitle in a file.</summary>
    public class XbmcSubtitleDetails : XbmcDbStreamDetails, ISubtitle, IXbmcHasLanguage {
        private ILanguage _language;
        private string _languageName;

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

        internal XbmcSubtitleDetails(ISubtitle subtitle, XbmcFile file) {
            Id = subtitle.Id;
            if (subtitle.Language != null) {
                Language = subtitle.Language.ISO639.Alpha3;
            }
            File = file;
        }

        /// <summary>Gets or sets the language of this subtitle.</summary>
        /// <value>The language of this subtitle.</value>
        [Column("strSubtitleLanguage")]
        public string Language {
            get { return _languageName; }
            set {
                _languageName = value;

                if (_languageName != null) {
                    ISOLanguageCode isoCode = ISOLanguageCodes.Instance.GetByISOCode(_languageName);
                    if (isoCode != null) {
                        _language = new XbmcLanguage(isoCode);
                    }
                }
            }
        }

        #region ISubtitle

        ILanguage IHasLanguage.Language {
            get { return _language; }
            set { 
                Language = (value != null && value.ISO639 != null)
                    ? value.ISO639.Alpha3
                    : null; }
        }

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Language":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        bool ISubtitle.EmbededInVideo {
            get { return default(bool); }
            set { }
        }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        string ISubtitle.Encoding {
            get { return default(string); }
            set { }
        }

        IFile ISubtitle.File {
            get { return File; }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        bool ISubtitle.ForHearingImpaired {
            get { return default(bool); }
            set { }
        }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        string ISubtitle.Format {
            get { return default(string); }
            set { }
        }

        string ISubtitle.MD5 {
            get { return default(string); }
            set { }
        }

        long? ISubtitle.OpenSubtitlesId {
            get { return default(long?); }
            set { }
        }

        long? ISubtitle.PodnapisiId {
            get { return default(long?); }
            set { }
        }

        #endregion
    }

}
