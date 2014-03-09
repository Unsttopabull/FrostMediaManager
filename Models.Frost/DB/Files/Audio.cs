using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common;
using Frost.Common.Models;
using Frost.Model.Xbmc.NFO;

namespace Frost.Models.Frost.DB.Files {

    /// <summary>Represents information about an audio stream in a file.</summary>
    public class Audio : IAudio {
        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="Audio"/> class.</summary>
        public Audio() {
        }

        public Audio(IAudio audio) {
            Source = audio.Source;
            Type = audio.Type;
            ChannelPositions = audio.ChannelPositions;
            NumberOfChannels = audio.NumberOfChannels;
            ChannelPositions = audio.ChannelPositions;
            Codec = audio.Codec;
            CodecId = audio.CodecId;
            BitRate = audio.BitRate;
            BitRateMode = audio.BitRateMode;
            SamplingRate = audio.SamplingRate;
            BitRate = audio.BitRate;
            CompressionMode = audio.CompressionMode;
            Duration = audio.Duration;

            if (audio.Language != null) {
                Language = new Language(audio.Language);
            }

            if (audio.File != null) {
                File = new File(audio.File);
            }

            //if (audio.Movie != null) {
            //    Movie = new Movie(audio.Movie);
            //}
        }

        /// <summary>Initializes a new instance of the <see cref="Audio"/> class.</summary>
        /// <param name="file">The file.</param>
        public Audio(File file) {
            File = file;
        }

        /// <summary>Initializes a new instance of the <see cref="Audio"/> class.</summary>
        /// <param name="source">The source of the audio (LD, MIC, LINE MIC, ...)</param>
        /// <param name="type">The type of the audio (AC3, DTS, Sorund, ...)</param>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channelSetup">The audio channels setting. (2, 6, 5.1, ...)</param>
        /// <param name="language">The language of this audio.</param>
        public Audio(string source, string type, string codec, string channelSetup, string language) {
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

        #endregion

        #region Properties/Columns

        /// <summary>Gets or sets the database audio Id.</summary>
        /// <value>The database audio Id</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        public string CodecId { get; set; }

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

        /// <summary>Gets or sets the language of this audio.</summary>
        /// <value>The language of this audio.</value>
        ILanguage IHasLanguage.Language {
            get { return Language; }
            set { Language = new Language(value); }
        }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        public virtual File File { get; set; }

        /// <summary>Gets or sets the file this audio is contained in.</summary>
        /// <value>The file this audio is contained in.</value>
        IFile IAudio.File {
            get { return File; }
            set {
                File = new File(value);
            }
        }

        /// <summary>Gets or sets the movie this audio is from.</summary>
        /// <value>The movie this audio is from.</value>
        public virtual Movie Movie { get; set; }

        #endregion

        /// <summary>Converts and instance of <see cref="Audio"/> to an instance of <see cref="XbmcXmlAudioInfo">XbmcXmlAudioInfo</see></summary>
        /// <param name="audio">The audio to convert</param>
        /// <returns>An instance of <see cref="XbmcXmlAudioInfo">XbmcXmlAudioInfo</see> converted from <see cref="Audio"/></returns>
        public static explicit operator XbmcXmlAudioInfo(Audio audio) {
            return new XbmcXmlAudioInfo(audio.Codec, audio.NumberOfChannels ?? 0, audio.Language.ISO639.Alpha3);
        }

        /// <summary>Converts <see cref="XbmcXmlAudioInfo"/> to an instance of <see cref="Audio">Audio</see></summary>
        /// <param name="audio">The instance of <see cref="XbmcXmlAudioInfo"/> to convert</param>
        /// <returns>An instance of <see cref="Audio">Audio</see> converted from <see cref="XbmcXmlAudioInfo"/></returns>
        public static explicit operator Audio(XbmcXmlAudioInfo audio) {
            return new Audio(audio.Codec, audio.Channels.ToICString(), audio.Language);
        }

        internal class Configuration : EntityTypeConfiguration<Audio> {
            public Configuration() {
                ToTable("Audios");

                HasRequired(a => a.Movie)
                    .WithMany(m => m.Audios)
                    .HasForeignKey(a => a.MovieId)
                    .WillCascadeOnDelete();

                HasRequired(a => a.File)
                    .WithMany(f => f.AudioDetails)
                    .HasForeignKey(a => a.FileId)
                    .WillCascadeOnDelete();

                HasOptional(a => a.Language);
            }
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return MemberwiseClone();
        }
    }

}