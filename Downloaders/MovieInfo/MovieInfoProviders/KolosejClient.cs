using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml.XPath;
using Frost.InfoParsers;
using HtmlAgilityPack;

namespace Frost.MovieInfoProviders {

    public class KolosejClient : ParsingClient {
        private const string URL = "http://www.kolosej.si{0}";
        public const string CLIENT_NAME = "Kolosej.si";

        public KolosejClient() : base(CLIENT_NAME, true, false, false) {
            string directoryName;
            try {
                 directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch {
                return;
            }

            if (directoryName != null) {
                Icon = new Uri(directoryName+"/kolosej.png");
            }
        }

        public override IEnumerable<ParsedMovie> GetByImdbId(string imdbId) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByMovieHash(IEnumerable<string> movieHashes) {
            throw new NotImplementedException();
        }

        public override IEnumerable<ParsedMovie> GetByTitle(string title, int releaseYear) {
            throw new NotImplementedException();
        }

        public override void Index() {
            string list;
            using (WebClient webCl = new WebClient { Encoding = Encoding.UTF8 }) {
                list = webCl.DownloadString(@"http://www.kolosej.si/filmi/A-Z/original/");
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(list));

            List<ParsedMovie> movies = new List<ParsedMovie>();

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

                    movies.Add(new ParsedMovie(origName, sloName, link));
                }
            }
            else {
                throw new Exception("Expected content not found.");
            }

            AvailableMovies = movies;
        }

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            HtmlDocument hd = DownloadWebPage(movie.Url);

            if (hd == null) {
                return null;
            }

            HtmlNode mainContent = hd.GetElementbyId("main-content-one-column");
            HtmlNode movieInfo = mainContent.SelectSingleNode("//div[@class='movie-info']");
            ParsedMovieInfo info = ParseMovieInfo(movieInfo);

            if (info == null) {
                return null;
            }

            info.Plot = mainContent.SelectSingleNode("//div[@class='summary']").InnerTextOrNull();

            HtmlNode trailer = mainContent.SelectSingleNode("//div[@class='inline-trailer']/iframe[@src]");
            if (trailer != null) {
                info.TrailerUrl = trailer.Attributes["src"].Value.Trim();
            }
            return info;
        }

        private ParsedMovieInfo ParseMovieInfo(HtmlNode movieInfo) {
            if (movieInfo == null) {
                return null;
            }

            ParsedMovieInfo info = new ParsedMovieInfo();

            string distrib = movieInfo.SelectSingleNode("div[@class='distribution']").InnerTextOrNull();
            if (distrib != null) {
                info.Distribution = distrib.Replace("Film distribucije ", "");
            }

            HtmlNode officialSite = movieInfo.SelectSingleNode("span[@class='title-orig']/a[@href]");
            if (officialSite != null) {
                info.OfficialSite = officialSite.Attributes["href"].Value;
            }

            HtmlNode duration = movieInfo.SelectSingleNode("span[@class='duration']/text()");
            if (duration != null) {
                info.Duration = duration.InnerText.Replace('\t', ' ').Replace('\n', ' ').Trim();
            }

            info.ReleaseYear = movieInfo.SelectSingleNode("span[@class='year']/text()").InnerTextAsIntOrNull();
            info.Country = movieInfo.SelectSingleNode("span[@class='country']/a/text()").InnerTextOrNull();
            info.Language = movieInfo.SelectSingleNode("span[@class='language']/text()").InnerTextOrNull();
            info.Writers = movieInfo.SelectSingleNode("span[@class='screenplay']/text()")
                                    .InnerTextSplitOrNull(true, ",", " in ")
                                    .Where(d => d != null)
                                    .Select(d => new ParsedPerson(d));

            info.Directors = movieInfo.SelectNodes("span[@class='director']/a/text()")
                                      .InnerTextOrNull()
                                      .Where(d => d != null)
                                      .Select(d => new ParsedPerson(d));

            info.Actors = movieInfo.SelectNodes("span[@class='actors']/a/text()")
                                   .InnerTextOrNull()
                                   .Where(d => d != null)
                                   .Select(d => new ParsedActor(d));

            HtmlNode imdb = movieInfo.SelectSingleNode("//a[text()='IMDB']");
            if (imdb != null && imdb.HasAttributes) {
                info.ImdbLink = imdb.Attributes[0].Value;
            }

            return info;
        }
    }

}