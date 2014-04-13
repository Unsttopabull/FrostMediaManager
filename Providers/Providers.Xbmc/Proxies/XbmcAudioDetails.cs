using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Common.Util.ISO;
using Frost.Providers.Xbmc.DB.Proxy;
using Frost.Providers.Xbmc.DB.StreamDetails;

namespace Frost.Providers.Xbmc.Proxies {

    /// <summary>Represents information about an audio stream in a file.</summary>
    public class XbmcAudioDetails : Proxy<XbmcDbStreamDetails>, IAudio {
        private ILanguage _language;

        public XbmcAudioDetails(XbmcDbStreamDetails stream) : base(stream) {
        }

        #region IAudio

        /// <summary>Unique identifier.</summary>
        public long Id { get; private set; }

        public bool this[string propertyName] {
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

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string Codec {
            get { return Entity.AudioCodec; }
            set { } 
        }

        /// <summary>Gets or sets the codec id this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MPAL3, aac_hd, dtshd</c>}</example>
        public string CodecId {
            get { return Entity.AudioCodec; }
            set { Entity.AudioCodec = value; }
        }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        public int? NumberOfChannels {
            get { return (int?) Entity.AudioChannels; }
            set { Entity.AudioChannels = value; }
        }

        public ILanguage Language {
            get {
                if (_language != null) {
                    return _language;
                }

                ISOLanguageCode isoCode = ISOLanguageCodes.Instance.GetByISOCode(Entity.AudioLanguage);
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

                Entity.AudioLanguage = (value != null && value.ISO639 != null)
                                      ? value.ISO639.Alpha3
                                      : null;
            }
        }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        public IFile File {
            get { return Entity.File; }
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