using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Movies {

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public class ImdbMovieDetails {

        /// <summary></summary>
        [XmlRpcMember("cast")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public ImdbPeople Actors;

        [XmlRpcMember("imdb_status")]
        public string ImdbStatus;

        /// <summary></summary>
        [XmlRpcMember("goofs")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Goofs;

        /// <summary></summary>
        [XmlRpcMember("request_from")]
        public string RequestFrom;

        //float
        [XmlRpcMember("rating")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Rating;

        //bool
        [XmlRpcMember("from_redis")]
        public string FromRedis;

        [XmlRpcMember("kind")]
        public string Kind;

        /// <summary></summary>
        [XmlRpcMember("trivia")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Trivia;

        /// <summary></summary>
        [XmlRpcMember("cover")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Cover;

        /// <summary></summary>
        [XmlRpcMember("awards")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string[] Awards;

        /// <summary></summary>
        [XmlRpcMember("genres")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string[] Genres;

        /// <summary></summary>
        [XmlRpcMember("id")]
        [XmlRpcMissingMapping(MappingAction.Error)]
        public string Id;

        //int
        [XmlRpcMember("votes")]
        public string Votes;

        /// <summary></summary>
        [XmlRpcMember("country")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string[] Countries;

        /// <summary></summary>
        [XmlRpcMember("language")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string[] Languages;

        /// <summary></summary>
        [XmlRpcMember("directors")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public ImdbPeople Directors;

        /// <summary></summary>
        [XmlRpcMember("duration")]
        public string Duration;

        /// <summary></summary>
        [XmlRpcMember("tagline")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Tagline;

        /// <summary></summary>
        [XmlRpcMember("plot")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Plot;

        /// <summary></summary>
        [XmlRpcMember("title")]
        [XmlRpcMissingMapping(MappingAction.Error)]
        public string Title;

        [XmlRpcMember("aka")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string[] AlternateTitles;

        /// <summary></summary>
        [XmlRpcMember("year")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Year;

        /// <summary></summary>
        [XmlRpcMember("writers")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public ImdbPeople Writers;

        /// <summary></summary>
        [XmlRpcMember("certification")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string[] Certification;
    }

}