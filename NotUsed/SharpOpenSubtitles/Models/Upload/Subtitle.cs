using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class Subtitle : ISubtitlePart {

        /// <summary>Structure containing information about a subtitle file for the first or only part.</summary>
        [XmlRpcMember("cd1")]
        public SubFile CD1;

        /// <summary>Structure containing information about a subtitle file for the second part.</summary>
        [XmlRpcMember("cd2")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public SubFile CD2;
    }
}