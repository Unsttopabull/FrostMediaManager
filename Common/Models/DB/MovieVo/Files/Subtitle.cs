using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo.Files {

    /// <summary>Represents information about a subtitle stream in a file.</summary>
    public class Subtitle : IEquatable<Subtitle> {

        /// <summary>Initializes a new instance of the <see cref="Subtitle"/> class.</summary>
        public Subtitle() {
            Movie = new Movie();
            File = new File();
        }

        /// <summary>Initializes a new instance of the <see cref="Subtitle"/> class.</summary>
        /// <param name="language">The language of this subtitle.</param>
        /// <param name="embededInVideo">If the subtitle is embeded in the video set to <c>true</c></param>
        /// <param name="forHearingImpaired"></param>
        public Subtitle(string language, bool embededInVideo = false, bool forHearingImpaired = false) {
            Language = language;
            EmbededInVideo = embededInVideo;
            ForHearingImpaired = forHearingImpaired;
        }

        #region Properties/Columns

        /// <summary>Gets or sets the Id of this subtitle in the database.</summary>
        /// <value>The Id of this subtitle in the database</value>
        [Key]
        public long Id { get; set; }

        /// <summary>Gets or sets the language this subtitle is in.</summary>
        /// <value>The language of this subtitle.</value>
        public string Language { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        public bool EmbededInVideo { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        public bool ForHearingImpaired { get; set; }

        #endregion

        #region Foreign Keys

        /// <summary>Gets or sets the movie foreign key.</summary>
        /// <value>The movie foreign key.</value>
        public long MovieId { get; set; }

        /// <summary>Gets or sets the file foreign key.</summary>
        /// <value>The file foreign key.</value>
        public long FileId { get; set; }

        #endregion

        #region Associations/Related tables

        /// <summary>Gets or sets the file this subtitle is contained in.</summary>
        /// <value>The file this subtitle is contained in.</value>
        [ForeignKey("FileId")]
        public virtual File File { get; set; }

        /// <summary>Gets or sets the movie this subtitle if for.</summary>
        /// <value>The movie this subtitle if for.</value>
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Subtitle other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Language == other.Language &&
                   EmbededInVideo == other.EmbededInVideo &&
                   ForHearingImpaired == other.ForHearingImpaired &&
                   MovieId == other.MovieId &&
                   FileId == other.FileId;
        }

    }

}
