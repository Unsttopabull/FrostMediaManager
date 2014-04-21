using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
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

        public PlanetTusClient() {
            _webCl = new WebClient { Encoding = Encoding.UTF8 };
        }

        public override List<IParsedMovie> Parse() {
            List<IParsedMovie> list = new List<IParsedMovie>();
            foreach (string subList in SubLists) {
                list.AddRange(ParsePage(subList));
            }

            AvailableMovies = list;
            return list;
        }

        private IEnumerable<TusMovie> ParsePage(string subList, int page = 1) {
            Console.WriteLine(@"{0} page: {1}", subList, page);

            string list = _webCl.DownloadString(string.Format(URL, subList, page));
            //string list = _webCl.DownloadString(@"C:\Users\Martin\Desktop\FMM\Tus\tus.html");

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(list));
           

            HtmlNode center = hd.GetElementbyId("center");
            HtmlNodeCollection movieList = center.SelectNodes("//ul[@class='movie_list']/li");

            if (movieList == null) {
                return new List<TusMovie>();
            }
            
            List<TusMovie> movies = new List<TusMovie>(movieList.Count);
            List<Task> tsk = new List<Task>();

            foreach (HtmlNode node in movieList) {
                string url = null;
                string sloName = null;

                HtmlNode tusUrl = node.SelectSingleNode("div[@class='inside']/h2[@class='ntb']/a[@href]");
                if (tusUrl != null) {
                    url = string.Format(MOVIE_URL, tusUrl.Attributes["href"].Value);
                    sloName = tusUrl.InnerTextOrNull();
                }
                string origName = node.SelectSingleNode("div[@class='inside']/h3[@class='ntb']/text()").InnerTextOrNull();

                TusMovie movie = new TusMovie(origName, sloName, url);
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

            Task.WaitAll(tsk.ToArray());
            return movies;
        }

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