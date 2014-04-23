using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Frost.InfoParsers.Models;
using HtmlAgilityPack;

namespace Frost.InfoParsers {

    [Serializable]
    public class ParsedMovieInfo {
        public ParsedMovieInfo() {
            Videos = new List<IParsedVideo>();
        }

        public bool IsFinished { get; set; }
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
        public string Summary { get; set; }
        public string TrailerUrl { get; set; }
        public ICollection<IParsedVideo> Videos { get; set; }
        public ParsedAward Awards { get; set; }
        public IEnumerable<string> Genres { get; set; }

        public static HtmlDocument DownloadWebPage(string url, Encoding enc = null) {
            string html;
            using (WebClient webCl = new WebClient { Encoding = enc ?? Encoding.UTF8 }) {
                try {
                    html = webCl.DownloadString(url);
                }
                catch (WebException e) {
                    Console.Error.WriteLine(e.Message);
                    return null;
                }
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(html));
            return hd;
        }
    }

}