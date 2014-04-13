using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Common.Util.ISO;
using Frost.Providers.Xbmc.DB.Proxy;
using Frost.Providers.Xbmc.DB.StreamDetails;

namespace Frost.Providers.Xbmc.Proxies {

    /// <summary>Represents information about a subtitle in a file.</summary>
    public class XbmcSubtitleDetails : Proxy<XbmcDbStreamDetails>, ISubtitle {
        private ILanguage _language;

        /// <summary>Initializes a new instance of the <see cref="XbmcSubtitleDetails"/> class.</summary>
        public XbmcSubtitleDetails(XbmcDbStreamDetails stream) : base(stream) {
        }

        public XbmcSubtitleDetails(ISubtitle sub) : base(new XbmcDbStreamDetails()){
            if (sub != null && sub.Language != null) {
                Language = sub.Language;
            }
        }

        public long Id {
            get { return Entity.Id; }
        }

        public ILanguage Language {
            get {
                if (_language != null) {
                    return _language;
                }

                ISOLanguageCode isoCode = ISOLanguageCodes.Instance.GetByISOCode(Entity.SubtitleLanguage);
                _language = isoCode != null 
                    ? new XbmcLanguage(isoCode)
                    : null;

                return _language;
            }
            set {
                if (value != null && value.ISO639 != null) {
                    _language = new XbmcLanguage(value);
                }
                else {
                    _language = null;
                }

                Entity.SubtitleLanguage = (value != null && value.ISO639 != null)
                                      ? value.ISO639.Alpha3
                                      : null;
            }
        }

        public bool this[string propertyName] {
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

        public IFile File {
            get { return Entity.File; }
        }

        #region Not Implemented

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
