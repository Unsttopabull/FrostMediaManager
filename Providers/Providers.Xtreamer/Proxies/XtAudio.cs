using System.Collections.Generic;
using System.Globalization;
using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtAudio : ChangeTrackingProxy<XjbPhpMovie>, IAudio {
        private readonly string _xtPath;
        private IFile _file;

        public XtAudio(XjbPhpMovie movie, string xtPath) : base(movie) {
            _xtPath = xtPath;
            OriginalValues = new Dictionary<string, object> {
                {"Source", Entity.AudioSource},
                {"Type", Entity.AudioType},
                {"NumberOfChannels", Entity.AudioChannels},
                {"CodecId", Entity.AudioCodec},
            };
        }

        public long Id {
            get { return 0; }
        }

        /// <summary>Gets or sets the source of the audio</summary>
        /// <value>the source of the audio</value>
        /// <example>\eg{<c>LD MD LINE MIC</c>}</example>
        public string Source {
            get { return Entity.AudioSource; }
            set {
                Entity.AudioSource = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the type of the audio</summary>
        /// <summary>The type of the audio</summary>
        /// <example>\eg{<c>AC3, DTS</c>}</example>
        public string Type {
            get { return Entity.AudioType; }
            set {
                Entity.AudioType = value;
                TrackChanges(value);
            }
        }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        public int? NumberOfChannels {
            get {
                int num;
                if (int.TryParse(Entity.AudioChannels, out num)) {
                    return num;
                }
                return null;
            }
            set {
                Entity.AudioChannels = value.HasValue
                                           ? value.Value.ToString(CultureInfo.InvariantCulture)
                                           : null;

                TrackChanges(Entity.AudioChannels);
            }
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string Codec {
            get { return Entity.AudioCodec; }
            set {
                //TrackChanges(value);
                //Entity.AudioCodec = value;
            }
        }

        /// <summary>Gets or sets the codec id this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MPAL3, aac_hd, dtshd</c>}</example>
        public string CodecId {
            get { return Entity.AudioCodec; }
            set {
                TrackChanges(value);
                Entity.AudioCodec = value;
            }
        }

        #region Not Implemented

        /// <summary>Gets or sets the channel setup.</summary>
        /// <value>The audio channels setting.</value>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        public string ChannelSetup {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the audio channel positions.</summary>
        /// <value>The audio channel positions.</value>
        /// <example>\eg{ <c>Front: L C R, Side: L R, LFE</c>}</example>
        public string ChannelPositions {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return null; }
            set { }
        }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>\eg{ ''<c>Constant</c>'' or ''<c>Variable</c>''}</example>
        public FrameOrBitRateMode BitRateMode {
            get { return default(FrameOrBitRateMode); }
            set { }
        }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        public long? SamplingRate {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        public long? BitDepth {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the compression mode of this audio.</summary>
        /// <value>The compression mode of this audio.</value>
        public CompressionMode CompressionMode {
            get { return default(CompressionMode); }
            set { }
        }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        public long? Duration {
            get { return null; }
            set { }
        }

        #endregion

        #region Releations

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        public IFile File {
            get { return _file ?? (_file = new XtFile(Entity, _xtPath)); }
        }

        public ILanguage Language {
            get { return null; }
            set { }
        }

        #endregion

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Source":
                    case "Type":
                    case "NumberOfChannels":
                    case "Codec":
                    case "CodecId":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
