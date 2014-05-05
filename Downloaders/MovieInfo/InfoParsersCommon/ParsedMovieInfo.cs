using System;
using System.Collections.Generic;
using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers {

    [Serializable]
    public class ParsedMovieInfo {
        public ParsedMovieInfo() {
            Videos = new List<IParsedVideo>();
        }

        public string Distribution { get; set; }
        public string MPAA { get; set; }
        public string Duration { get; set; }
        public int? ReleaseYear { get; set; }
        public IEnumerable<string> Countries { get; set; }
        public string Language { get; set; }
        public IEnumerable<IParsedPerson> Writers { get; set; }
        public IEnumerable<IParsedPerson> Directors { get; set; }
        public IEnumerable<IParsedActor> Actors { get; set; }
        public string OfficialSite { get; set; }
        public string ImdbLink { get; set; }

        /// <summary>Gets or sets the movie average rating on a scale from 1 to 10 (can have decimals).</summary>
        /// <value>The average movie rating.</value>
        public string Rating { get; set; }
        public string TmdbId { get; set; }
        public string Plot { get; set; }
        public string Tagline { get; set; }
        public string TrailerUrl { get; set; }
        public ICollection<IParsedVideo> Videos { get; set; }
        public IEnumerable<IParsedAward> Awards { get; set; }

        public IEnumerable<IParsedArt> Art { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public string Cover { get; set; }
        public string CoverPreview { get; set; }

        public string Fanart { get; set; }
        public string FanartPreview { get; set; }
    }

}