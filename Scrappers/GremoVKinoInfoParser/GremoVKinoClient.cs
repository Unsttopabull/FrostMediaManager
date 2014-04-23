using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Frost.InfoParsers;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers.GremoVKino {

    public class GremoVKinoClient : ParsingClient {
        private const string URL = "http://www.kolosej.si{0}";
        private const string TRAILER_URL = "http://www.gremovkino.si/trailer_single/show/{0}/576/344/1";
        private const string XPATH = "table/tr/td[@class='trailer_leftCell' and text()='{0} ']/following-sibling::td[@class='trailer_rightCell']";
        private const string YOUTUBE = "http://www.youtube.com/";
        private const string YOUTUBE_VIDEO_URL = "http://www.youtube.com/watch?v={0}";

        public GremoVKinoClient() : base("gremovkino") {
        }

        public override List<ParsedMovie> Parse() {
            throw new NotImplementedException();
        }

        public List<ParsedMovie> Parse(string movieTitle) {
            string list;
            using (WebClient webCl = new WebClient { Encoding = Encoding.UTF8 }) {
                webCl.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                webCl.Headers.Add(HttpRequestHeader.Referer, "http://www.gremovkino.si/filmi/iskanje/seznam-filmi");
                webCl.Headers.Add("Origin", "http://www.gremovkino.si");
                webCl.Headers.Add("Host", "www.gremovkino.si");
                webCl.Headers.Add(HttpRequestHeader.UserAgent,
                    "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36 OPR/18.0.1284.68");

                byte[] uploadData = Encoding.UTF8.GetBytes(string.Format("mainsearch={0}", movieTitle));
                byte[] response = webCl.UploadData("http://www.gremovkino.si/filmi/iskanje/seznam-filmi", uploadData);
                list = Encoding.UTF8.GetString(response);
            }

            HtmlDocument hd = new HtmlDocument();
            hd.Load(new StringReader(list));

            List<ParsedMovie> movies = new List<ParsedMovie>();

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
                movies.Add(new ParsedMovie(origName, sloName, url));
            }

            AvailableMovies = movies;
            return movies;
        }


        #region Movie information parsing

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            HtmlDocument hd = ParsedMovieInfo.DownloadWebPage(movie.Url);

            if (hd == null) {
                return null;
            }

            return ParseMovieInfo(hd.GetElementbyId("trailer_info"));
        }

        private ParsedMovieInfo ParseMovieInfo(HtmlNode movieInfo) {
            if (movieInfo == null) {
                return null;
            }

            ParsedMovieInfo mi = new ParsedMovieInfo();

            mi.Summary = movieInfo.SelectSingleNode("//div[@id='short_desc']/div[2]").InnerTextOrNull();
            HtmlNode right = movieInfo.SelectSingleNode("//div[@id='rightData']");

            mi.ImdbLink = right.SelectSingleNode(string.Format(XPATH, "Imdb:")).InnerTextOrNull(false);
            mi.Genres = right.SelectSingleNode(string.Format(XPATH, "Žanr:")).InnerTextSplitOrNull(true, ',');
            mi.Directors = right.SelectSingleNode(string.Format(XPATH, "Režija:")).InnerTextSplitOrNull(true, ',');
            mi.Writers = right.SelectSingleNode(string.Format(XPATH, "Scenarij:")).InnerTextSplitOrNull(true, ',');
            mi.Country = right.SelectSingleNode(string.Format(XPATH, "Država:")).InnerTextOrNull();
            mi.Duration = right.SelectSingleNode(string.Format(XPATH, "Trajanje:")).InnerTextOrNull();
            mi.OfficialSite = right.SelectSingleNode(string.Format(XPATH, "Uradna stran:")).InnerTextOrNull(false);

            HtmlNodeCollection videos = movieInfo.SelectNodes("//div[@id='subVideos']/table/tr[position() > 2]");
            if (videos != null) {
                Task[] arr = new Task[videos.Count];
                for (int i = 0; i < videos.Count; i++) {
                    arr[i] = ParseVideoList(videos[i]).ContinueWith(t => {
                        if (t.IsCanceled || t.IsFaulted) {
                            return;
                        }

                        lock (mi.Videos) {
                            mi.Videos.Add(t.Result);
                        }
                    });
                }

                Task.WaitAll(arr);
            }

            return mi;
        }

        private async Task<ParsedVideo> ParseVideoList(HtmlNode video) {
            string videoId = video.GetAttributeValue("onclick", null);
            if (videoId == null) {
                return null;
            }

            ParsedVideo pv = new ParsedVideo { Url = await GetVideoUrl(videoId) };
            if (string.IsNullOrEmpty(pv.Url)) {
                return null;
            }

            pv.Title = video.SelectSingleNode("td[3]").InnerTextOrNull();
            pv.Type = GetVideoType(pv.Title);

            HtmlNode langNode = video.SelectSingleNode("td/img[@title]");
            if (langNode != null) {
                string[] languages = langNode.Attributes["title"].Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < languages.Length; i++) {
                    languages[i] = languages[i].Replace("jezik: ", "")
                                               .Replace("podnapisi: ", "")
                                               .Trim();
                }

                if (languages.Length > 0) {
                    pv.Language = languages[0];
                }
                if (languages.Length > 1) {
                    pv.SubtitleLanguage = languages[1];
                }
            }

            pv.Duration = video.SelectSingleNode("td[5]").InnerTextOrNull();
            return pv;
        }

        private VideoType GetVideoType(string title) {
            if (title.ContainsIgnoreCase("Review") || title.Contains("recenzija")) {
                return VideoType.Review;
            }

            if (title.ContainsIgnoreCase("trailer") || title.Contains("napovednik")) {
                return VideoType.Trailer;
            }

            if (title.ContainsIgnoreCase("Interview") || title.ContainsIgnoreCase("Intervju")) {
                return VideoType.Interview;
            }

            if (title.ContainsIgnoreCase("Featurette")) {
                return VideoType.Featurete;
            }

            if (title.Contains("Behind the Scenes")) {
                return VideoType.BehindTheScenes;
            }

            if (title.ContainsIgnoreCase("Clip") || title.ContainsIgnoreCase("Klip") || title.Contains("izrezek")) {
                return VideoType.Clip;
            }

            if (title.ContainsIgnoreCase("TV Spot")) {
                return VideoType.TvSpot;
            }

            return VideoType.Unknown;
        }

        private async Task<string> GetVideoUrl(string videoId) {
            int idxStart = videoId.IndexOf(',');
            int idxEnd = videoId.IndexOf(',', idxStart + 1);

            videoId = videoId.Substring(++idxStart, idxEnd - idxStart);

            HtmlDocument hd = ParsedMovieInfo.DownloadWebPage(string.Format(TRAILER_URL, videoId));
            HtmlNode urlNode = hd.DocumentNode.SelectSingleNode("//embed[@src and @type='application/x-shockwave-flash']");
            if (urlNode == null) {
                return null;
            }

            string url = urlNode.Attributes["src"].Value;
            if (!url.StartsWith(YOUTUBE)) {
                return url;
            }

            int queryParam = url.IndexOf('&');
            int dirEnd = url.LastIndexOf('/');

            if (queryParam != -1) {
                url = url.Substring(++dirEnd, queryParam - dirEnd);
                url = string.Format(YOUTUBE_VIDEO_URL, url);
            }
            return url;
        }

        #endregion


    }

}