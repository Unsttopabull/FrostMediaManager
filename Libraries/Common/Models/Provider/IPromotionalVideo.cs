namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about a promotional video clip that is accessible by the UI</summary>
    public interface IPromotionalVideo : IMovieEntity {

        /// <summary>Gets or sets the clip type.</summary>
        /// <value>The clip type.</value>
        PromotionalVideoType Type { get; set; }

        /// <summary>Gets or sets the clip title.</summary>
        /// <value>The clip title.</value>
        string Title { get; set; }

        /// <summary>Gets or sets the clip URL.</summary>
        /// <value>The clip URL.</value>
        string Url { get; set; }

        /// <summary>Gets or sets the clip duration (eg. 5:30 meaning 5 minutes 30 seconds).</summary>
        /// <value>The clip duration.</value>
        string Duration { get; set; }

        /// <summary>Gets or sets the name of the language used in the clip.</summary>
        /// <value>The language used in the clip.</value>
        string Language { get; set; }

        /// <summary>Gets or sets the langauge of the subtitles used in the clip.</summary>
        /// <value>The langauge of the subtitles used in the clip</value>
        string SubtitleLanguage { get; set; }
    }
}