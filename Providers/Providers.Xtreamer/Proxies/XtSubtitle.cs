using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtSubtitle : ISubtitle {
        private readonly XjbPhpMovie _movie;
        private readonly int _index;
        private XtSubtitleFile _subtitleFile;
        private ILanguage _language;

        public XtSubtitle(XjbPhpMovie movie, int index) {
            _movie = movie;
            _index = index;
        }

        public long Id {
            get { return 0; }
        }

        public long? PodnapisiId {
            get { return 0; }
            set { }
        }

        public long? OpenSubtitlesId {
            get { return 0; }
            set { }
        }

        public string MD5 {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo {
            get { return false; }
            set { }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired {
            get { return false; }
            set { }
        }

        public IFile File {
            get { return _subtitleFile ?? (_subtitleFile = new XtSubtitleFile(_movie, _index)); }
        }

        public ILanguage Language {
            get { return _language ?? (_language = XtLanguage.FromEnglishNameCode(_movie.SubtitleLanguage)); }
            set { _language = value; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "File":
                    case "Language":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}