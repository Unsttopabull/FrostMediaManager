using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Frost.InfoParsers;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers.PlanetTus {

    public class PlanetTusClient : ParsingClient, IDisposable {
        private const string URL = "http://maribor.planet-tus.si/sl/kino?l={0}&page={1}";
        private const string MOVIE_URL = "http://maribor.planet-tus.si{0}";
        private static readonly string[] SubLists = {
            "num", "a", "b", "c", "č", "d", "e", "f", "g", "j", "k", "l", "m", "n",
            "o", "p", "q", "r", "s", "š", "t", "u", "v", "w", "x", "y", "z", "ž"
        };

        private readonly WebClient _webCl;

        public PlanetTusClient() : base("Tus") {
            _webCl = new WebClient { Encoding = Encoding.UTF8 };
        }

        #region Index building

        public override List<ParsedMovie> Parse() {
            List<ParsedMovie> list = new List<ParsedMovie>();
            foreach (string subList in SubLists) {
                list.AddRange(ParsePage(subList));
            }

            AvailableMovies = list;
            return list;
        }

        private IEnumerable<ParsedMovie> ParsePage(string subList, int page = 1) {
            Console.WriteLine(@"{0} page: {1}", subList, page);

            string list = _webCl.DownloadString(string.Format(URL, subList, page));

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(list));


            HtmlNode center = hd.GetElementbyId("center");
            HtmlNodeCollection movieList = center.SelectNodes("//ul[@class='movie_list']/li");

            if (movieList == null) {
                return new List<ParsedMovie>();
            }

            List<ParsedMovie> movies = new List<ParsedMovie>(movieList.Count);
            //List<Task> tsk = new List<Task>();

            foreach (HtmlNode node in movieList) {
                string url = null;
                string sloName = null;

                HtmlNode tusUrl = node.SelectSingleNode("div[@class='inside']/h2[@class='ntb']/a[@href]");
                if (tusUrl != null) {
                    url = string.Format(MOVIE_URL, tusUrl.Attributes["href"].Value);
                    sloName = tusUrl.InnerTextOrNull();
                }
                string origName = node.SelectSingleNode("div[@class='inside']/h3[@class='ntb']/text()").InnerTextOrNull();

                ParsedMovie movie = new ParsedMovie(origName, sloName, url);
                movies.Add(movie);
            }

            if (page == 1) {
                HtmlNodeCollection pagination = center.SelectNodes("//div[@class='pagination clearfix']/a");

                if (pagination != null) {
                    for (int i = 2; i < pagination.Count; i++) {
                        movies.AddRange(ParsePage(subList, i));
                    }
                }
            }

            //Task.WaitAll(tsk.ToArray());
            return movies;
        }

        #endregion

        #region Movie Parsing

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            HtmlDocument hd = ParsedMovieInfo.DownloadWebPage(movie.Url);

            if (hd == null) {
                return null;
            }

            ParsedMovieInfo movieInfo = new ParsedMovieInfo();

            HtmlNode center = hd.GetElementbyId("center");
            movieInfo.Summary = GetSummary(center);
            GetInfo(movieInfo, center);

            return movieInfo;
        }

        private static void GetInfo(ParsedMovieInfo mi, HtmlNode center) {
            HtmlNode movieData = center.SelectSingleNode("//div[@class='movie_data']/dl");

            if (movieData == null) {
                return;
            }

            HtmlNode imdb = movieData.SelectSingleNode("dt/a[@href and text()='IMDB']");
            if (imdb != null) {
                mi.ImdbLink = imdb.Attributes["href"].Value;

                HtmlNode imdbRating = imdb.ParentNode.SelectSingleNode("following-sibling::dd[1]");
                if (imdbRating != null) {
                    mi.ImdbRating = imdbRating.InnerText.Trim();
                }
            }

            mi.Genres = movieData.SelectNodes("dt[text()='Zvrst:']/following-sibling::dd[1]/a").InnerTextOrNull();
            mi.Duration = movieData.SelectSingleNode("dt[text()='Trajanje:']/following-sibling::dd[1]").InnerTextOrNull();
            mi.Actors = movieData.SelectNodes("dt[text()='Igrajo:']/following-sibling::dd[1]/a").InnerTextOrNull();
            mi.Directors = movieData.SelectNodes("dt[text()='Režija:']/following-sibling::dd[1]/a").InnerTextOrNull();
            mi.Writers = movieData.SelectNodes("dt[text()='Scenarij:']/following-sibling::dd[1]/a").InnerTextOrNull();
            mi.Distribution = movieData.SelectSingleNode("dt[text()='Distributer:']/following-sibling::dd[1]/a").InnerTextOrNull();
            mi.OfficialSite = movieData.SelectSingleNode("dt[text()='Spletna stran:']/following-sibling::dd[1]/a").InnerTextOrNull(false);
            mi.ReleaseYear = movieData.SelectSingleNode("dt[text()='Letnik:']/following-sibling::dd[1]/a").InnerTextOrNull();
        }

        private string GetSummary(HtmlNode center) {
            HtmlNodeCollection summaryParagraphs = center.SelectNodes("//div[@class='gen_panes']/div[1]/p[position() < 3]");

            if (summaryParagraphs != null) {
                StringBuilder sb = new StringBuilder();
                foreach (HtmlNode paragraph in summaryParagraphs) {
                    sb.AppendLine(paragraph.InnerText.Trim());
                }
                return sb.ToString();
            }
            return null;
        }

        #endregion

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (AvailableMovies != null) {
                AvailableMovies.Clear();
            }

            if (_webCl != null) {
                _webCl.Dispose();
            }
        }
    }

}