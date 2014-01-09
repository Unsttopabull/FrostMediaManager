using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Frost.Common.Models.DB.MovieVo.Files {

    /// <summary>Represents information about a subtitle stream in a file.</summary>
    public class Subtitle : IEquatable<Subtitle> {

        public Subtitle() {
            
        }

        public Subtitle(File file = null) {
            File = file;
            Language = new Language();
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
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
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

        /// <summary>Gets or sets the file this subtitle is contained in.</summary>
        /// <value>The file this subtitle is contained in.</value>
        public virtual File File { get; set; }

        /// <summary>Gets or sets the movie this subtitle if for.</summary>
        /// <value>The movie this subtitle if for.</value>
        public virtual Movie Movie { get; set; }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Subtitle other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Id == other.Id && PodnapisiId == other.PodnapisiId && OpenSubtitlesId == other.OpenSubtitlesId && string.Equals(MD5, other.MD5) && string.Equals(Format, other.Format) && string.Equals(Encoding, other.Encoding) && ForHearingImpaired.Equals(other.ForHearingImpaired) && EmbededInVideo.Equals(other.EmbededInVideo) && LanguageId == other.LanguageId && MovieId == other.MovieId && FileId == other.FileId;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((Subtitle) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode() {
            unchecked {
                int hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ PodnapisiId.GetHashCode();
                hashCode = (hashCode * 397) ^ OpenSubtitlesId.GetHashCode();
                hashCode = (hashCode * 397) ^ (MD5 != null ? MD5.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Format != null ? Format.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Encoding != null ? Encoding.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ForHearingImpaired.GetHashCode();
                hashCode = (hashCode * 397) ^ EmbededInVideo.GetHashCode();
                hashCode = (hashCode * 397) ^ LanguageId.GetHashCode();
                hashCode = (hashCode * 397) ^ MovieId.GetHashCode();
                hashCode = (hashCode * 397) ^ FileId.GetHashCode();
                return hashCode;
            }
        }
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder(100);

            sb.Append(string.Format("Format: {0} ", Format ?? "*."+File.Extension));

            if (!string.IsNullOrEmpty(Encoding)) {
                sb.Append("(" + Encoding + ")");
            }

            if (Language != null) {
                sb.Append(" - " + Language.Name);
            }

            return sb.ToString();
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

                HasOptional(s => s.Language).WithMany();
            }
        }
    }

}