using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.XPath;
using HtmlAgilityPack;

namespace Frost.CinemaInfoParsers.Kolosej {

    public class KolosejClient : CinemaClient<KolosejMovie> {
        private const string URL = "http://www.kolosej.si{0}";

        public override List<KolosejMovie> Parse() {

            string list;
            using (WebClient webCl = new WebClient { Encoding = Encoding.UTF8 }) {
                 list = webCl.DownloadString(@"http://www.kolosej.si/filmi/A-Z/original/");
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(list));

            List<KolosejMovie> movies = new List<KolosejMovie>();

            HtmlNode mainContent = hd.GetElementbyId("main-content-one-column");
            XPathNavigator xPathNavigator = mainContent.CreateNavigator();
            if (xPathNavigator != null) {
                XPathNodeIterator movieList = xPathNavigator.Select("table[@class='movie-list']/tbody/tr[position() > 1]");
                if (movieList.Count == 0) {
                    movieList = xPathNavigator.Select("table[@class='movie-list']/tr[position() > 1]");
                }

                foreach (HtmlNodeNavigator node in movieList) {
                    HtmlNodeNavigator origNode = (HtmlNodeNavigator) node.SelectSingleNode("td/a");

                    string origName = null;
                    string sloName = null;
                    string link = null;

                    if (origNode != null) {
                        origName = origNode.Value;

                        if (origNode.HasAttributes) {
                            link = string.Format(URL, origNode.CurrentNode.Attributes[0].Value);
                        }
                    }

                    XPathNavigator xpn = node.SelectSingleNode("td[2]/text()");
                    if (xpn != null) {
                        sloName = xpn.Value;
                    }

                    movies.Add(new KolosejMovie(origName, sloName, link));
                }
            }
            else {
                throw new Exception("Expected content not found.");
            }

            AvailableMovies = movies;
            return movies;
        }
    }

}