using System;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Frost.CinemaInfoParsers {
    
    [Serializable]
    public abstract class ParsedMovieInfo {
        public bool IsFinished { get; protected set; }
        public string Distribution { get; protected set; }
        public string Duration { get; protected set; }
        public string ReleaseYear { get; protected set; }
        public string Country { get; protected set; }
        public string Language { get; protected set; }
        public string[] Writers { get; protected set; }
        public string[] Directors { get; protected set; }
        public string[] Actors { get; protected set; }
        public string OfficialSite { get; protected set; }
        public string ImdbLink { get; protected set; }
        public string ImdbRating { get; protected set; }
        public string Summary { get; protected set; }
        public string TrailerUrl { get; protected set; }
        public string[] Genres { get; protected set; }

        protected HtmlDocument DownloadWebPage(string url) {
            string html;
            using (WebClient webCl = new WebClient { Encoding = Encoding.UTF8 }) {
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