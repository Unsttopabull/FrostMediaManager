using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class BaseInfo {

        /// <summary>Movie's IMDb ID.</summary>
        [XmlRpcMember("idmovieimdb")]
        public string IdMovieIMDB;

        /// <summary>Subtitle language ISO639-3 code.</summary>
        [XmlRpcMember("sublanguageid")]
        public string SubLanguageId;

        /// <summary>Release name.</summary>
        [XmlRpcMember("moviereleasename")]
        public string MovieReleaseName;

        /// <summary>Optional alias movie title.</summary>
        [XmlRpcMember("movieaka")]
        public string MovieAKA;

        /// <summary>Optional uploader's comments (insert translator credits here, etc.).</summary>
        [XmlRpcMember("subauthorcomment")]
        public string SubAuthorComment;
    }
}