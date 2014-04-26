using System;
using System.Collections.Generic;
using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {

    [Serializable]
    public class ParsedMovieInfo {
        public ParsedMovieInfo() {
            Videos = new List<IParsedVideo>();
        }

        public string Distribution { get; set; }
        public string Duration { get; set; }
        public string ReleaseYear { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public IEnumerable<string> Writers { get; set; }
        public IEnumerable<string> Directors { get; set; }
        public IEnumerable<string> Actors { get; set; }
        public string OfficialSite { get; set; }
        public string ImdbLink { get; set; }
        public string ImdbRating { get; set; }
        public string Plot { get; set; }
        public string TrailerUrl { get; set; }
        public ICollection<IParsedVideo> Videos { get; set; }
        public IEnumerable<ParsedAward> Awards { get; set; }
        public IEnumerable<string> Genres { get; set; }

        public string Cover { get; set; }

        public string Fanart { get; set; }
    }

}