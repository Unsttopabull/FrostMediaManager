namespace Frost.SharpOpenSubtitles.Models.Report {

    public class SubtitleRating {

        /// <summary>Average subtitle rating for given subtitles.</summary>
        public string SubRating;

        /// <summary>Number of times these subtitles were rated.</summary>
        public string SubSumVotes;

        /// <summary>ID of subtitle in the database (BEWARE this is not the ID of subtitle file but the whole subtitle).</summary>
        public string IDSubtitle;
    }
}