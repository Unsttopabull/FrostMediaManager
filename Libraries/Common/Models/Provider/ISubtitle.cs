namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about a subtitle that is accessible by the UI.</summary>
    public interface ISubtitle : IHasLanguage, IMovieEntity {

        /// <summary>Gets or sets the Subtitle Id on Podnapisi.NET.</summary>
        long? PodnapisiId { get; set; }

        /// <summary>Gets or sets the Subtitle Id on OpenSubtitle.org.</summary>
        long? OpenSubtitlesId { get; set; }

        /// <summary>Gets or sets the MD5 Hash of the file.</summary>
        /// <value>The MD5 hash of the file.</value>
        string MD5 { get; set; }

        /// <summary>Gets or sets the type or format of the subtitle.</summary>
        /// <value>The type or format of the subtitle.</value>
        string Format { get; set; }

        /// <summary>Gets or sets the character set this subtitle is encoded in.</summary>
        /// <value>The character set this subtitle is encoded in</value>
        string Encoding { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is embeded in the movie video.</summary>
        /// <value>Is <c>true</c> if this subtitle is embeded in the movie video; otherwise, <c>false</c>.</value>
        bool EmbededInVideo { get; set; }

        /// <summary>Gets or sets a value indicating whether this subtitle is for people that are hearing impaired.</summary>
        /// <value>Is <c>true</c> if this subtitle is for people that are hearing impaired; otherwise, <c>false</c>.</value>
        bool ForHearingImpaired { get; set; }

        /// <summary>Gets or sets the file this subtitle is contained in.</summary>
        /// <value>The file this subtitle is contained in.</value>
        IFile File { get; }
    }
}