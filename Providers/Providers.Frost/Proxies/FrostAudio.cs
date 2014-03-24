using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Providers.Frost.DB.Files;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {
    class FrostAudio : ProxyBase<Audio>, IAudio {

        public FrostAudio(Audio audio, FrostMoviesDataDataService service) : base(audio, service) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public bool this[string propertyName] {
            get { throw new System.NotImplementedException(); }
        }

        /// <summary>Gets or sets the source of the audio</summary>
        /// <value>the source of the audio</value>
        /// <example>\eg{<c>LD MD LINE MIC</c>}</example>
        public string Source {
            get { return Entity.Source; }
            set { Entity.Source = value; }
        }

        /// <summary>Gets or sets the type of the audio</summary>
        /// <summary>The type of the audio</summary>
        /// <example>\eg{<c>AC3 DTS</c>}</example>
        public string Type {
            get { return Entity.Type; }
            set { Entity.Type = value; }
        }

        /// <summary>Gets or sets the channel setup.</summary>
        /// <value>The audio channels setting.</value>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        public string ChannelSetup {
            get { return Entity.ChannelSetup; }
            set { Entity.ChannelSetup = value; }
        }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        public int? NumberOfChannels {
            get { return Entity.NumberOfChannels; }
            set { Entity.NumberOfChannels = value; }
        }

        /// <summary>Gets or sets the audio channel positions.</summary>
        /// <value>The audio channel positions.</value>
        /// <example>\eg{ <c>Front: L C R, Side: L R, LFE</c>}</example>
        public string ChannelPositions {
            get { return Entity.ChannelPositions; }
            set { Entity.ChannelPositions = value; }
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string Codec {
            get { return Entity.Codec; }
            set { Entity.Codec = value; }
        }

        /// <summary>Gets or sets the codec id this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MPAL3, aac_hd, dtshd</c>}</example>
        public string CodecId {
            get { return Entity.CodecId; }
            set { Entity.CodecId = value; }
        }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate {
            get { return Entity.BitRate; }
            set { Entity.BitRate = value; }
        }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>\eg{ ''<c>Constant</c>'' or ''<c>Variable</c>''}</example>
        public FrameOrBitRateMode BitRateMode {
            get { return Entity.BitRateMode; }
            set { Entity.BitRateMode = value; }
        }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        public long? SamplingRate {
            get { return Entity.SamplingRate; }
            set { Entity.SamplingRate = value; }
        }

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        public long? BitDepth {
            get { return Entity.BitDepth; }
            set { Entity.BitDepth = value; }
        }

        /// <summary>Gets or sets the compression mode of this audio.</summary>
        /// <value>The compression mode of this audio.</value>
        public CompressionMode CompressionMode {
            get { return Entity.CompressionMode; }
            set { Entity.CompressionMode = value; }
        }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        public long? Duration {
            get { return Entity.Duration; }
            set { Entity.Duration = value; }
        }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        public IFile File {
            get { return Entity.File; }
        }

        public ILanguage Language {
            get { return Entity.Language; }
            set { Entity.Language = Service.FindLanguage(value, true); }
        }
    }
}
