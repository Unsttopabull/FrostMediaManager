using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Movies {

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class ImdbMovieDetails {

        [XmlRpcMember("imdb_status")]
        public string IMDBStatus;

        /// <summary></summary>
        [XmlRpcMember("id")]
        [XmlRpcMissingMapping(MappingAction.Error)]
        public string ID;

        /// <summary></summary>
        [XmlRpcMember("title")]
        [XmlRpcMissingMapping(MappingAction.Error)]
        public string Title;

        [XmlRpcMember("aka")]
        public string[] AlternateTitles;

        /// <summary></summary>
        [XmlRpcMember("year")]
        public string Year;

        /// <summary></summary>
        [XmlRpcMember("cover")]
        public string Cover;

        /// <summary></summary>
        [XmlRpcMember("cast")]
        public ImdbPeople Cast;

        /// <summary></summary>
        [XmlRpcMember("directors")]
        public ImdbPeople Directors;

        /// <summary></summary>
        [XmlRpcMember("writers")]
        public ImdbPeople Writers;

        /// <summary></summary>
        [XmlRpcMember("awards")]
        public string Awards;

        //float
        [XmlRpcMember("rating")]
        public string Rating;

        //int
        [XmlRpcMember("votes")]
        public string Votes;

        /// <summary></summary>
        [XmlRpcMember("genres")]
        public string[] Genres;

        /// <summary></summary>
        [XmlRpcMember("country")]
        public string[] Countries;

        /// <summary></summary>
        [XmlRpcMember("language")]
        public string[] Languages;

        /// <summary></summary>
        [XmlRpcMember("duration")]
        public string Duration;

        /// <summary></summary>
        [XmlRpcMember("certification")]
        public string[] Certification;

        /// <summary></summary>
        [XmlRpcMember("tagline")]
        public string Tagline;

        /// <summary></summary>
        [XmlRpcMember("plot")]
        public string Plot;

        /// <summary></summary>
        [XmlRpcMember("goofs")]
        public string Goofs;

        /// <summary></summary>
        [XmlRpcMember("trivia")]
        public string Trivia;

        /// <summary></summary>
        [XmlRpcMember("request_from")]
        public string RequestFrom;

        //bool
        [XmlRpcMember("from_redis")]
        public string FromRedis;

        [XmlRpcMember("kind")]
        public string Kind;
    }

}