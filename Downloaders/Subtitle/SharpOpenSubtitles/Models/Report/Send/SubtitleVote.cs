using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Report {

    public class SubtitleVote {

        /// <summary>ID of subtitle (NOT subtitle file) user wants to rate.</summary>
        [XmlRpcMember("idsubtitle")]
        public string SubtitleID;

        /// <summary>Subtitle rating, must be in interval 1 (worst) to 10 (best).</summary>
        [XmlRpcMember("score")]
        public double Score;

        public SubtitleVote(string subtitleID, double score) {
            SubtitleID = subtitleID;
            Score = score;
        }
    }

}