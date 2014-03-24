using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Common.Util.ISO;
using Frost.Providers.Xbmc.DB.Proxy;

namespace Frost.Providers.Xbmc.DB.StreamDetails {

    /// <summary>Represents information about an audio stream in a file.</summary>
    public class XbmcAudioDetails : XbmcDbStreamDetails, IAudio, IXbmcHasLanguage {
        private ILanguage _language;
        private string _languageName;

        public XbmcAudioDetails() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcAudioDetails"/> class.</summary>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The number of audio channels.</param>
        /// <param name="language">The language of this audio.</param>
        public XbmcAudioDetails(string codec, long? channels, string language = null) : this(new XbmcFile(), codec, channels, language) {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcAudioDetails"/> class.</summary>
        /// <param name="file">The file that contains this audio stream.</param>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The number of audio channels.</param>
        /// <param name="language">The language of this audio.</param>
        public XbmcAudioDetails(XbmcFile file, string codec, long? channels, string language = null) {
            File = file;

            Codec = codec;
            Channels = channels;
            Language = language;
        }

        internal XbmcAudioDetails(IAudio audio) {
            Id = audio.Id;
            Codec = audio.CodecId;
            Channels = audio.NumberOfChannels;
            if (audio.Language != null) {
                Language = audio.Language.ISO639.Alpha3;
            }
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        [Column("strAudioCodec")]
        public string Codec { get; set; }

        /// <summary>Gets or sets the number of audio channels.</summary>
        /// <value>The number of audio channels.</value>
        [Column("iAudioChannels")]
        public long? Channels { get; set; }

        /// <summary>Gets or sets the language of this audio.</summary>
        /// <value>The language of this audio.</value>
        [Column("strAudioLanguage")]
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

        #region IAudio 

        bool IMovieEntity.this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Codec":
                    case "CodecId":
                    case "NumberOfChannels":
                    case "Language":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the codec id this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MPAL3, aac_hd, dtshd</c>}</example>
        string IAudio.CodecId {
            get { return Codec; }
            set { Codec = value; }
        }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        int? IAudio.NumberOfChannels {
            get { return (int?) Channels; }
            set { Channels = value; }
        }

        ILanguage IHasLanguage.Language {
            get { return _language; }
            set { 
                Language = (value != null && value.ISO639 != null)
                    ? value.ISO639.Alpha3
                    : null; }
        }

        #region Not Implemented

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        long? IAudio.BitDepth {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        float? IAudio.BitRate {
            get { return default(float?); }
            set { }
        }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>\eg{ ''<c>Constant</c>'' or ''<c>Variable</c>''}</example>
        FrameOrBitRateMode IAudio.BitRateMode {
            get { return default(FrameOrBitRateMode); }
            set { }
        }

        /// <summary>Gets or sets the audio channel positions.</summary>
        /// <value>The audio channel positions.</value>
        /// <example>\eg{ <c>Front: L C R, Side: L R, LFE</c>}</example>
        string IAudio.ChannelPositions {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the channel setup.</summary>
        /// <value>The audio channels setting.</value>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        string IAudio.ChannelSetup {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the compression mode of this audio.</summary>
        /// <value>The compression mode of this audio.</value>
        CompressionMode IAudio.CompressionMode {
            get { return default(CompressionMode); }
            set { }
        }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        long? IAudio.Duration {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        IFile IAudio.File {
            get { return File; }
            //set { File = new XbmcFile(value); }
        }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        long? IAudio.SamplingRate {
            get { return default(long?); }
            set { }
        }

        /// <summary>Gets or sets the source of the audio</summary>
        /// <value>the source of the audio</value>
        /// <example>\eg{<c>LD MD LINE MIC</c>}</example>
        string IAudio.Source {
            get { return default(string); }
            set { }
        }

        /// <summary>Gets or sets the type of the audio</summary>
        /// <summary>The type of the audio</summary>
        /// <example>\eg{<c>AC3 DTS</c>}</example>
        string IAudio.Type {
            get { return default(string); }
            set { }
        }

        #endregion

        #endregion
    }

}
