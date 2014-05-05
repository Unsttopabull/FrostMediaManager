using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class SubFile {

        /// <summary>MD5 hash of subtitle file contents.</summary>
        [XmlRpcMember("subhash")]
        public string SubHash;

        /// <summary>Subtitle filename.</summary>
        [XmlRpcMember("subfilename")]
        public string SubFilename;

        /// <summary>Hash calculated for the video file contents, see <a href="http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes">Hash Source Codes</a> for various implementations</summary>
        [XmlRpcMember("moviehash")]
        public string MovieHash;

        /// <summary>Size of the video file in bytes.</summary>
        [XmlRpcMember("moviebytesize")]
        public double MovieSizeInBytes;

        /// <summary>Length of video in miliseconds.</summary>
        [XmlRpcMember("movietimems")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int? MovieTimeMS;

        /// <summary>Length of video in frames.</summary>
        [XmlRpcMember("movieframes")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int? MovieFrames;

        /// <summary>Frame rate used in video file, e.g. 23.976.</summary>
        [XmlRpcMember("moviefps")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double? MovieFPS;

        /// <summary>Video filename.</summary>
        [XmlRpcMember("moviefilename")]
        public string MovieFilename;

        /// <summary>GZipped and then Base64-encoded subtitle file contents.</summary>
        [XmlRpcMember("subcontent")]
        public string SubContent;
    }
}