using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using Frost.Common.Models;
using Frost.Model.Xbmc.NFO;

namespace Frost.Models.Frost.DB.Files {

    /// <summary>Represents information about a subtitle stream in a file.</summary>
    public class Subtitle : ISubtitle /*, IEquatable<ISubtitle>*/ {

        /// <summary>Initializes a new instance of the <see cref="Subtitle" /> class.</summary>
        public Subtitle() {
            
        }

        public Subtitle(ISubtitle subtitle) {
            PodnapisiId = subtitle.PodnapisiId;
            OpenSubtitlesId = subtitle.OpenSubtitlesId;
            MD5 = subtitle.MD5;
            Format = subtitle.Format;
            Encoding = subtitle.Encoding;
            EmbededInVideo = subtitle.EmbededInVideo;
            ForHearingImpaired = subtitle.ForHearingImpaired;

            if (subtitle.Language != null) {
                Language = new Language(subtitle.Language);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="Subtitle" /> class.</summary>
        public Subtitle(File file = null) {
            File = file;
        }

        /// <summary>Initializes a new instance of the <see cref="Subtitle" /> class.</summary>
        /// <param name="file">The file this subtitle is contained in.</param>
        /// <param name="lang">The language info of this subtitle.</param>
        public Subtitle(File file = null, Language lang = null) : this(file) {
            Language = lang;
        }

        /// <summary>Initializes a new instance of the <see cref="Subtitle"/> class.</summary>
        /// <param name="file">The file this subtitle is contained in.</param>
        /// <param name="language">The language of this subtitle.</param>
        /// <param name="typeFormat">The type or format of the subtitle.</param>
        /// <param name="embededInVideo">If the subtitle is embeded in the video set to <c>true</c>.</param>
        /// <param name="forHearingImpaired">If the subtitle is for people that are hearing impaired.</param>
        public Subtitle(File file, Language language, string typeFormat = null, bool embededInVideo = false, bool forHearingImpaired = false) : this(file, language) {
            Format = typeFormat;
            EmbededInVideo = embededInVideo;
            ForHearingImpaired = forHearingImpaired;
        }

        /// <summary>Initializes a new instance of the <see cref="Subtitle"/> class.</summary>
        /// <param name="file">The file this subtitle is contained in.</param>
        /// <param name="language">The language of this subtitle.</param>
        /// <param name="typeFormat">The type or format of the subtitle.</param>
        /// <param name="embededInVideo">If the subtitle is embeded in the video set to <c>true</c>.</param>
        /// <param name="forHearingImpaired">If the subtitle is for people that are hearing impaired.</param>
        public Subtitle(File file, string language, string typeFormat = null, bool embededInVideo = false, bool forHearingImpaired = false) : this(file, new Language(language), typeFormat, embededInVideo, forHearingImpaired) {
        }

        #region Properties/Columns

        /// <summary>Gets or sets the Id of this subtitle in the database.</summary>
        /// <value>The Id of this subtitle in the database</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long? PodnapisiId { get; set; }

        public long? OpenSubtitlesId { get; set; }

        public string MD5 { get; set; }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        public string Format { get; set; }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        public string Encoding { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired { get; set; }

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

        #region Associations/Related tables

        /// <summary>Gets or sets the language this subtitle is in.</summary>
        /// <value>The language of this subtitle.</value>
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }

        /// <summary>Gets or sets the language this subtitle is in.</summary>
        /// <value>The language of this subtitle.</value>
        ILanguage IHasLanguage.Language {
            get { return Language; }
            set {
                if (value == null) {
                    Language = null;
                    return;
                }
                Language = new Language(value);
            }
        }

        /// <summary>Gets or sets the file this subtitle is contained in.</summary>
        /// <value>The file this subtitle is contained in.</value>
        public virtual File File { get; set; }

        IFile ISubtitle.File {
            get { return File; }
        }

        /// <summary>Gets or sets the movie this subtitle if for.</summary>
        /// <value>The movie this subtitle if for.</value>
        public virtual Movie Movie { get; set; }

        #endregion

        /// <summary>Converts <see cref="XbmcXmlSubtitleInfo"/> to an instance of <see cref="Subtitle">Subtitle</see></summary>
        /// <param name="subtitle">The instance of <see cref="XbmcXmlAudioInfo"/> to convert</param>
        /// <returns>An instance of <see cref="Subtitle">Subtitle</see> converted from <see cref="XbmcXmlSubtitleInfo"/></returns>
        public static explicit operator Subtitle(XbmcXmlSubtitleInfo subtitle) {
            return new Subtitle(null, subtitle.LongLanguage ?? subtitle.Language, null);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ISubtitle other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(other, this)) {
                return true;
            }

            if (other.Id != 0 && other.Id == Id) {
                return true;
            }

            if(other.MD5 == MD5 &&
               other.OpenSubtitlesId == OpenSubtitlesId &&
               other.PodnapisiId == PodnapisiId &&
               other.EmbededInVideo == EmbededInVideo &&
               other.ForHearingImpaired == ForHearingImpaired &&
               other.Encoding == Encoding &&
               other.Format == Format
            )
            {
                if (other.File != null && File != null) {
                    return other.File.Equals(File);
                }
                return true;
            }
            return false;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder(100);

            string extension = File != null ? "*." + File.Extension : "";
            sb.Append(string.Format("Format: {0} ", Format ?? (EmbededInVideo ? "Embeded" : extension)));

            if (!string.IsNullOrEmpty(Encoding)) {
                sb.Append("(" + Encoding + ")");
            }

            if (Language != null) {
                sb.Append(" - " + Language.Name);
            }

            return sb.ToString();
        }

        /// <summary>Creates a new object that is a copy of the current instance.</summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        public object Clone() {
            return MemberwiseClone();
        }

        internal class Configuration : EntityTypeConfiguration<Subtitle> {
            public Configuration() {
                ToTable("Subtitles");
                
                //Movie <--> Subtitles
                HasRequired(s => s.Movie)
                    .WithMany(m => m.Subtitles)
                    .HasForeignKey(fk => fk.MovieId)
                    .WillCascadeOnDelete();

                HasRequired(s => s.File)
                    .WithMany(f => f.Subtitles)
                    .HasForeignKey(fk => fk.FileId)
                    .WillCascadeOnDelete();

                HasOptional(s => s.Language);
            }
        }
    }

}