using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class UploadSubtitleInfo {

        /// <summary>Basic upload information structure.</summary>
        [XmlRpcMember("baseinfo")]
        public BaseInfo BaseInfo;

        /// <summary>Structure containing information about a subtitle file for the first or only part.</summary>
        [XmlRpcMember("cd1")]
        public ISubtitlePart CD1;

        /// <summary>Structure containing information about a subtitle file for the second part.</summary>
        [XmlRpcMember("cd2")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public ISubtitlePart CD2;
    }
}