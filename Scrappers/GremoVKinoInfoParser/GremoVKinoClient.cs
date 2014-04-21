using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers.GremoVKino {

    public class GremoVKinoClient : ParsingClient {
        private const string URL = "http://www.kolosej.si{0}";

        public override List<IParsedMovie> Parse() {
            throw new NotImplementedException();
        }

        public List<IParsedMovie> Parse(string movieTitle) {
            string list;
            using (WebClient webCl = new WebClient { Encoding = Encoding.UTF8 }) {
                webCl.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                webCl.Headers.Add(HttpRequestHeader.Referer, "http://www.gremovkino.si/filmi/iskanje/seznam-filmi");
                webCl.Headers.Add("Origin", "http://www.gremovkino.si");
                webCl.Headers.Add("Host", "www.gremovkino.si");
                webCl.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36 OPR/18.0.1284.68");

                byte[] uploadData = Encoding.UTF8.GetBytes(string.Format("mainsearch={0}", movieTitle));
                byte[] response = webCl.UploadData("http://www.gremovkino.si/filmi/iskanje/seznam-filmi", uploadData);
                list = Encoding.UTF8.GetString(response);
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(list));

            List<IParsedMovie> movies = new List<IParsedMovie>();

            HtmlNode searchResults = hd.GetElementbyId("trailers_list");
            searchResults = searchResults.SelectSingleNode("ul[1]");

            foreach (HtmlNode movie in searchResults.SelectNodes("li[@title]")) {
                string sloName = null;
                string url = null;
                string origName = movie.Attributes["title"].Value;

                HtmlNode movieLink = movie.SelectSingleNode("a[@href]");
                if (movieLink != null) {
                    url = movieLink.Attributes["href"].Value;

                    HtmlNode sloNameNode = movieLink.SelectSingleNode("img[@alt]");
                    if (sloNameNode != null) {
                        sloName = sloNameNode.Attributes["alt"].Value;

                        if (origName != sloName) {
                            origName = origName.Replace(sloName, "").Trim(' ', '(', ')');
                        }
                    }
                }
                movies.Add(new GremoVKinoMovie(origName, sloName, url));
            }

            AvailableMovies = movies;
            return movies;            
        }
    }

}