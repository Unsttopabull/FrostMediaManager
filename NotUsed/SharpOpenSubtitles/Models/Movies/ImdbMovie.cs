using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Movies {

    public class ImdbMovie {

        /// <summary>Movie's ID on IMDb.</summary>
        [XmlRpcMember("id")]
        public string ID;

        /// <summary>The movie title.</summary>
        [XmlRpcMember("title")]
        public string Title;
    }

}