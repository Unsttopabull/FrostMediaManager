using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {

    public class FrostSubtitle : ProxyWithService<Subtitle, FrostMoviesDataDataService>, ISubtitle {

        public FrostSubtitle(Subtitle subtitle, FrostMoviesDataDataService service) : base(subtitle, service) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public long? PodnapisiId {
            get { return Entity.PodnapisiId; }
            set { Entity.PodnapisiId = value; }
        }

        public long? OpenSubtitlesId {
            get { return Entity.OpenSubtitlesId; }
            set { Entity.OpenSubtitlesId = value; }
        }

        public string MD5 {
            get { return Entity.MD5; }
            set { Entity.MD5 = value; }
        }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format {
            get { return Entity.Format; }
            set { Entity.Format = value; }
        }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding {
            get { return Entity.Encoding; }
            set { Entity.Encoding = value; }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo {
            get { return Entity.EmbededInVideo; }
            set { Entity.EmbededInVideo = value; }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired {
            get { return Entity.ForHearingImpaired; }
            set { Entity.ForHearingImpaired = value; }
        }

        #region M to 1

        public IFile File {
            get { return Entity.File; }
            set {
                if (value == null) {
                    Entity.File = null;
                    return;
                }

                Entity.File = Service.FindFile(value, true);
            }
        }

        public ILanguage Language {
            get { return Entity.Language; }
            set {
                if (value == null) {
                    Entity.Language = null;
                    return;
                }

                Entity.Language = Service.FindLanguage(value, true);
            }
        }

        #endregion

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "PodnapisiId":
                    case "OpenSubtitlesId":
                    case "MD5":
                    case "Format":
                    case "Encoding":
                    case "EmbededInVideo":
                    case "ForHearingImpaired":
                    case "Language":
                    case "File":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
