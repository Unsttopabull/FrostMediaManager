using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Report {

    public class SubtitleComment {

        /// <summary>Subtitle identifier [BEWARE! this is not the ID of subtitle file but of the whole subtitle (a subtitle can contain multiple subtitle files)].</summary>
        [XmlRpcMember("idsubtitle")]
        public int SubtitleID;

        /// <summary>User's comment.</summary>
        [XmlRpcMember("comment")]
        public string Comment;

        /// <summary>Optional parameter. If set to 1, subtitles are marked as bad.</summary>
        [XmlRpcMember("badsubtitle")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int? BadSubtitle;

        public SubtitleComment(int subtitleID, string comment, bool badSubtitle) {
            SubtitleID = subtitleID;
            Comment = comment;

            if (badSubtitle) {
                BadSubtitle = 1;
            }
        }
    }

}