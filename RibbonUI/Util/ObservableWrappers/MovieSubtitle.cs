using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Models;
using Frost.Common.Properties;

namespace RibbonUI.Util.ObservableWrappers {

    public class MovieSubtitle : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ISubtitle _subtitle;

        public MovieSubtitle(ISubtitle subtitle) {
            _subtitle = subtitle;
        }

        public long Id {
            get { return _subtitle.Id; }
        }

        public long? PodnapisiId {
            get { return _subtitle.PodnapisiId; }
            set {
                _subtitle.PodnapisiId = value;
                OnPropertyChanged();
            }
        }

        public long? OpenSubtitlesId {
            get { return _subtitle.OpenSubtitlesId; }
            set {
                _subtitle.OpenSubtitlesId = value;
                OnPropertyChanged();
            }
        }

        public string MD5 {
            get { return _subtitle.MD5; }
            set {
                _subtitle.MD5 = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format {
            get { return _subtitle.Format; }
            set {
                _subtitle.Format = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding {
            get { return _subtitle.Encoding; }
            set {
                _subtitle.Encoding = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo {
            get { return _subtitle.EmbededInVideo; }
            set {
                _subtitle.EmbededInVideo = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired {
            get { return _subtitle.ForHearingImpaired; }
            set {
                _subtitle.ForHearingImpaired = value;
                OnPropertyChanged();
            }
        }

        public IFile File {
            get { return _subtitle.File; }
        }

        public ILanguage Language {
            get { return _subtitle.Language; }
            set {
                _subtitle.Language = value;
                OnPropertyChanged();
            }
        }

        public ISubtitle ObservedSubtitle {
            get { return _subtitle; }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}