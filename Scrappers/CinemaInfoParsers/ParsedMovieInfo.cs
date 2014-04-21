using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Frost.InfoParsers.Models;
using HtmlAgilityPack;

namespace Frost.InfoParsers {

    [Serializable]
    public abstract class ParsedMovieInfo : IParsedMovieInfo {

        public ParsedMovieInfo() {
            Videos = new List<IParsedVideo>();
        }

        public bool IsFinished { get; protected set; }
        public string Distribution { get; protected set; }
        public string Duration { get; protected set; }
        public string ReleaseYear { get; protected set; }
        public string Country { get; protected set; }
        public string Language { get; protected set; }
        public IEnumerable<string> Writers { get; protected set; }
        public IEnumerable<string> Directors { get; protected set; }
        public IEnumerable<string> Actors { get; protected set; }
        public string OfficialSite { get; protected set; }
        public string ImdbLink { get; protected set; }
        public string ImdbRating { get; protected set; }
        public string Summary { get; protected set; }
        public string TrailerUrl { get; protected set; }
        public ICollection<IParsedVideo> Videos { get; protected set; }
        public ParsedAward Awards { get; protected set; }
        public IEnumerable<string> Genres { get; protected set; }

        protected HtmlDocument DownloadWebPage(string url, Encoding enc = null) {
            string html;
            using (WebClient webCl = new WebClient { Encoding = enc ?? Encoding.UTF8 }) {
                try {
                    html = webCl.DownloadString(url);
                }
                catch (WebException e) {
                    Console.Error.WriteLine(e.Message);

                    IsFinished = false;
                    return null;
                }
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(html));
            return hd;
        }
    }

}