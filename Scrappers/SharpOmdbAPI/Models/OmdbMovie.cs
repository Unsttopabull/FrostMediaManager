﻿using Newtonsoft.Json;

namespace SharpOmdbAPI.Models {

    public class OmdbMovie {
        public string Title { get; set; }

        public string Year { get; set; }

        public string Rated { get; set; }

        public string Released { get; set; }

        public string Runtime { get; set; }

        public string Genre { get; set; }

        public string Director { get; set; }

        public string Writer { get; set; }

        public string Actors { get; set; }

        public string Plot { get; set; }

        public string Language { get; set; }

        public string Country { get; set; }

        public string Awards { get; set; }

        public string Poster { get; set; }

        public string Metascore { get; set; }

        [JsonProperty("imdbRating")]
        public string ImdbRating { get; set; }

        [JsonProperty("imdbVotes")]
        public string ImdbVotes { get; set; }

        [JsonProperty("imdbID")]
        public string ImdbId { get; set; }

        public string Type { get; set; }

        [JsonProperty("tomatoMeter")]
        public string TomatoMeter { get; set; }

        [JsonProperty("tomatoImage")]
        public string TomatoImage { get; set; }

        [JsonProperty("tomatoRating")]
        public string TomatoRating { get; set; }

        [JsonProperty("tomatoReviews")]
        public string TomatoReviews { get; set; }

        [JsonProperty("tomatoFresh")]
        public string TomatoFresh { get; set; }

        [JsonProperty("tomatoRotten")]
        public string TomatoRotten { get; set; }

        [JsonProperty("tomatoConsensus")]
        public string TomatoConsensus { get; set; }

        [JsonProperty("tomatoUserMeter")]
        public string TomatoUserMeter { get; set; }

        [JsonProperty("tomatoUserRating")]
        public string TomatoUserRating { get; set; }

        [JsonProperty("tomatoUserReviews")]
        public string TomatoUserReviews { get; set; }

        public string DVD { get; set; }

        public string BoxOffice { get; set; }

        public string Production { get; set; }

        public string Website { get; set; }

        public string Response { get; set; }
    }

}
