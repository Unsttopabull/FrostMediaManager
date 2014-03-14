using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Frost.Common;
using Frost.Common.Models;
using Frost.DetectFeatures;
using RibbonUI.Annotations;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieAudio : MovieHasLanguageBase, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IAudio _audio;

        /// <summary>Initializes a new instance of the <see cref="MovieAudio"/> class.</summary>
        /// <param name="audio">The audio.</param>
        public MovieAudio(IAudio audio) {
            _audio = audio;
        }

        public override ILanguage Language {
            get { return _audio.Language; }
            set {
                _audio.Language = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the source of the audio</summary>
        /// <value>the source of the audio</value>
        /// <example>\eg{<c>LD MD LINE MIC</c>}</example>
        public string Source {
            get { return _audio.Source; }
            set {
                _audio.Source = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the type of the audio</summary>
        /// <summary>The type of the audio</summary>
        /// <example>\eg{<c>AC3 DTS</c>}</example>
        public string Type {
            get { return _audio.Type; }
            set {
                _audio.Type = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the channel setup.</summary>
        /// <value>The audio channels setting.</value>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        public string ChannelSetup {
            get { return _audio.ChannelSetup; }
            set {
                _audio.ChannelSetup = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        public int? NumberOfChannels {
            get { return _audio.NumberOfChannels; }
            set {
                _audio.NumberOfChannels = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio channel positions.</summary>
        /// <value>The audio channel positions.</value>
        /// <example>\eg{ <c>Front: L C R, Side: L R, LFE</c>}</example>
        public string ChannelPositions {
            get { return _audio.ChannelPositions; }
            set {
                _audio.ChannelPositions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string Codec {
            get { return _audio.Codec; }
            set {
                _audio.Codec = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the codec id this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MPAL3, aac_hd, dtshd</c>}</example>
        public string CodecId {
            get { return _audio.CodecId; }
            set {
                _audio.CodecId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return _audio.BitRate; }
            set {
                _audio.BitRate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>\eg{ ''<c>Constant</c>'' or ''<c>Variable</c>''}</example>
        public FrameOrBitRateMode BitRateMode {
            get { return _audio.BitRateMode; }
            set {
                _audio.BitRateMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        public long? SamplingRate {
            get { return _audio.SamplingRate; }
            set {
                _audio.SamplingRate = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        public long? BitDepth {
            get { return _audio.BitDepth; }
            set {
                _audio.BitDepth = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the compression mode of this audio.</summary>
        /// <value>The compression mode of this audio.</value>
        public CompressionMode CompressionMode {
            get { return _audio.CompressionMode; }
            set {
                _audio.CompressionMode = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        public long? Duration {
            get { return _audio.Duration; }
            set {
                _audio.Duration = value;

                OnPropertyChanged("DurationTimeSpan");
                OnPropertyChanged();
            }
        }

        public TimeSpan DurationTimeSpan {
            get {
                return Duration.HasValue
                    ? TimeSpan.FromMilliseconds((double) Duration)
                    : new TimeSpan();
            }
            set {
                Duration = Convert.ToInt64(value.TotalMilliseconds);
            }
        }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        public IFile File {
            get { return _audio.File; }
            set {
                _audio.File = value;
                OnPropertyChanged();
            }
        }

        public IAudio ObservedAudio {get { return _audio; }}

        #region Images

        public ImageSource CodecImage {
            get {
                string mapping;
                FileFeatures.AudioCodecIdMappings.TryGetValue(CodecId, out mapping);
                return GetImageSourceFromPath("Images/FlagsE/acodec_" + (mapping ?? CodecId) + ".png");
            }
        }

        public ImageSource AudioChannelsImage {
            get { return GetImageSourceFromPath("Images/FlagsE/achan_" + NumberOfChannels + ".png"); }
        }
        #endregion 

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
