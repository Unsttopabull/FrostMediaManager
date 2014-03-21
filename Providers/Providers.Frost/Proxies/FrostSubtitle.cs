using Frost.Common.Models;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {

    public class FrostSubtitle : ProxyBase<Subtitle>, ISubtitle {

        public FrostSubtitle(Subtitle subtitle, FrostMoviesDataDataService service) : base(subtitle, service) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public bool this[string propertyName] {
            get { throw new System.NotImplementedException(); }
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

        public IFile File {
            get { return Entity.File; }
        }

        public ILanguage Language {
            get { return Entity.Language; }
            set { Entity.Language = Service.FindOrCreate(value, true); }
        }
    }
}
