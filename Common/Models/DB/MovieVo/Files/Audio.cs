using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Frost.Common.Models.XML.XBMC;

namespace Frost.Common.Models.DB.MovieVo.Files {

    /// <summary>Represents information about an audio stream in a file.</summary>
    [Table("Audios")]
    public class Audio : IEquatable<Audio> {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Audio"/> class.</summary>
        /// <param name="source">The source of the audio (LD, MIC, LINE MIC, ...)</param>
        /// <param name="type">The type of the audio (AC3, DTS, Sorund, ...)</param>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channelSetup">The audio channels setting. (2, 6, 5.1, ...)</param>
        /// <param name="language">The language of this audio.</param>
        public Audio(string source, string type, string codec, string channelSetup, string language) {
            File = new File();
            Movie = new Movie();
            Language = new Language(language);

            Source = source;
            Type = type;
            ChannelSetup = channelSetup;
            Codec = codec;
        }

        /// <summary>Initializes a new instance of the <see cref="Audio"/> class.</summary>
        /// <param name="source">The source of the audio (LD, MIC, LINE MIC, ...)</param>
        /// <param name="type">The type of the audio (AC3, DTS, Sorund, ...)</param>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channelSetup">The audio channels setting. (2, 6, 5.1, ...)</param>
        public Audio(string source, string type, string codec, string channelSetup) : this(source, type, codec, channelSetup, null) {
        }

        /// <summary>Initializes a new instance of the <see cref="Audio"/> class.</summary>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The audio channels setting. (2, 6, 5.1, ...)</param>
        /// <param name="language">The language of this audio.</param>
        public Audio(string codec, string channels, string language) : this(null, null, codec, channels, language) {
        }

        public Audio() {
            Movie = new Movie();
            File = new File();
            Language = new Language();
        }

        #endregion

        #region Propreties/Columns

        /// <summary>Gets or sets the database audio Id.</summary>
        /// <value>The database audio Id</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the source of the audio</summary>
        /// <value>the source of the audio</value>
        /// <example>\eg{<c>LD MD LINE MIC</c>}</example>
        public string Source { get; set; }

        /// <summary>Gets or sets the type of the audio</summary>
        /// <summary>The type of the audio</summary>
        /// <example>\eg{<c>AC3 DTS</c>}</example>
        public string Type { get; set; }

        /// <summary>Gets or sets the channel setup.</summary>
        /// <value>The audio channels setting.</value>
        /// <example>\eg{ <c>Stereo, 2, 5.1, 6</c>}</example>
        public string ChannelSetup { get; set; }

        /// <summary>Gets or sets the number of chanells in the audio (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio (5.1 has 6 chanels)</value>
        public int? NumberOfChannels { get; set; }

        /// <summary>Gets or sets the audio channel positions.</summary>
        /// <value>The audio channel positions.</value>
        /// <example>\eg{ <c>Front: L C R, Side: L R, LFE</c>}</example>
        public string ChannelPositions { get; set; }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        public string Codec { get; set; }

        /// <summary>Gets or sets the audio bit rate.</summary>
        /// <value>The bit rate in Kbps.</value>
        public float? BitRate { get; set; }

        /// <summary>Gets or sets the audio bit rate mode.</summary>
        /// <value>The bit rate mode</value>
        /// <example>\eg{ ''<c>Constant</c>'' or ''<c>Variable</c>''}</example>
        public FrameOrBitRateMode BitRateMode { get; set; }

        /// <summary>Gets or sets the audio sampling rate.</summary>
        /// <value>The sampling rate in KHz.</value>
        public long? SamplingRate { get; set; }

        /// <summary>Gets or sets the audio bit depth.</summary>
        /// <value>The audio depth in bits.</value>
        public long? BitDepth { get; set; }

        /// <summary>Gets or sets the compression mode of this audio.</summary>
        /// <value>The compression mode of this audio.</value>
        public CompressionMode CompressionMode { get; set; }

        /// <summary>Gets or sets the audio duration.</summary>
        /// <value>The audio duration in miliseconds.</value>
        public long? Duration { get; set; }

        #endregion

        #region Foreign Keys
        /// <summary>Gets or sets the language foreign key.</summary>
        /// <value>The language foreign key.</value>
        public long? LanguageId { get; set; }

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the file foreign key.</summary>
        /// <value>The file foreign key.</value>
        public long FileId { get; set; }

        #endregion

        #region Relation tables

        /// <summary>Gets or sets the language of this audio.</summary>
        /// <value>The language of this audio.</value>
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        [ForeignKey("FileId")]
        public virtual File File { get; set; }

        /// <summary>Gets or sets the movie this audio is from.</summary>
        /// <value>The movie this audio is from.</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Audio other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Source == other.Source &&
                   Type == other.Type &&
                   ChannelSetup == other.ChannelSetup &&
                   NumberOfChannels == other.NumberOfChannels &&
                   ChannelPositions == other.ChannelPositions &&
                   Codec == other.Codec &&
                   BitRate == other.BitRate &&
                   BitRateMode == other.BitRateMode &&
                   SamplingRate == other.SamplingRate &&
                   BitDepth == other.BitDepth &&
                   CompressionMode == other.CompressionMode &&
                   Duration == other.Duration &&
                   Language == other.Language;
        }

        /// <summary>Converts and instance of <see cref="Audio"/> to an instance of <see cref="Common.Models.XML.XBMC.XbmcXmlAudioInfo">XbmcXmlAudioInfo</see></summary>
        /// <param name="audio">The audio to convert</param>
        /// <returns>An instance of <see cref="Common.Models.XML.XBMC.XbmcXmlAudioInfo">XbmcXmlAudioInfo</see> converted from <see cref="Audio"/></returns>
        public static explicit operator XbmcXmlAudioInfo(Audio audio) {
            return new XbmcXmlAudioInfo(audio.Codec, audio.NumberOfChannels ?? 0, audio.Language.ISO639.Alpha3);
        }

    }

}
