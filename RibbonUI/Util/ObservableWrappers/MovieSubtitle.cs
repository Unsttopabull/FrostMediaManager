using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {

    public class MovieSubtitle : MovieHasLanguageBase<ISubtitle> {

        public MovieSubtitle(ISubtitle subtitle) : base(subtitle) {
        }

        public long Id {
            get { return _observedEntity.Id; }
        }

        public long? PodnapisiId {
            get { return _observedEntity.PodnapisiId; }
            set {
                _observedEntity.PodnapisiId = value;
                OnPropertyChanged();
            }
        }

        public long? OpenSubtitlesId {
            get { return _observedEntity.OpenSubtitlesId; }
            set {
                _observedEntity.OpenSubtitlesId = value;
                OnPropertyChanged();
            }
        }

        public string MD5 {
            get { return _observedEntity.MD5; }
            set {
                _observedEntity.MD5 = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format {
            get { return _observedEntity.Format; }
            set {
                _observedEntity.Format = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding {
            get { return _observedEntity.Encoding; }
            set {
                _observedEntity.Encoding = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo {
            get { return _observedEntity.EmbededInVideo; }
            set {
                _observedEntity.EmbededInVideo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired {
            get { return _observedEntity.ForHearingImpaired; }
            set {
                _observedEntity.ForHearingImpaired = value;
                OnPropertyChanged();
            }
        }

        public IFile File {
            get { return _observedEntity.File; }
        }

        public override ILanguage Language {
            get { return _observedEntity.Language; }
            set {
                _observedEntity.Language = value;
                OnPropertyChanged();
            }
        }
    }

}