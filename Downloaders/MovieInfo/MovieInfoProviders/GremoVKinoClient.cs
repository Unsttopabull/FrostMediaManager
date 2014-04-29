using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using HtmlAgilityPack;

namespace Frost.MovieInfoProviders {

    public class GremoVKinoClient : ParsingClient {
        private const int MAX = 9744;
        private const string URI = @"http://www.gremovkino.si/filmi/iskanje/seznam-filmi/";
        private const string TRAILER_URL = "http://www.gremovkino.si/trailer_single/show/{0}/576/344/1";
        private const string XPATH = "table/tr/td[@class='trailer_leftCell' and text()='{0} ']/following-sibling::td[@class='trailer_rightCell']";
        private const string YOUTUBE = "http://www.youtube.com/";
        private const string YOUTUBE_VIDEO_URL = "http://www.youtube.com/watch?v={0}";
        private int _numFailed;

        public GremoVKinoClient() : base("GremoVKino", true, false, false) {
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
            string maxUri = @"http://www.gremovkino.si/filmi/iskanje/seznam-filmi/" + MAX;

            List<string> uris = new List<string> { "http://www.gremovkino.si/filmi/iskanje/seznam-filmi/" };
            for (int i = 24; i <= MAX; i += 24) {
                uris.Add(URI + i);
            }

            string more = null;

            List<List<ParsedMovie>> parsedMovieLists = new List<List<ParsedMovie>>();
            Parallel.ForEach(uris, new ParallelOptions { MaxDegreeOfParallelism = 5 }, uri => {
                List<ParsedMovie> movieinfos = new List<ParsedMovie>();

                HtmlDocument hd = DownloadWebPage(uri);
                if (hd == null) {
                    return;
                }

                if (uri == maxUri) {
                    more = ParsePage(hd, movieinfos);
                }
                else {
                    ParsePage(hd, movieinfos);
                }

                lock (parsedMovieLists) {
                    parsedMovieLists.Add(movieinfos);
                }
            });

            if (!string.IsNullOrEmpty(more)) {
                HtmlDocument hd = DownloadWebPage(more);
                if (hd != null) {
                    parsedMovieLists.Add(IndexMovies(hd));
                }
            }

            AvailableMovies = parsedMovieLists.SelectMany(m => m);
        }

        #region Movie Indexing

        private List<ParsedMovie> IndexMovies(HtmlDocument hd) {
            List<ParsedMovie> movies = new List<ParsedMovie>();

            while (true) {
                string nextPageUrl = ParsePage(hd, movies);
                if (string.IsNullOrEmpty(nextPageUrl)) {
                    return movies;
                }

                hd = DownloadWebPage(nextPageUrl);
                if (hd == null) {
                    Thread.Sleep(2000);
                    if (_numFailed++ >= 5) {
                        _numFailed = 0;
                        return movies;
                    }

                    hd = DownloadWebPage(nextPageUrl);
                }
            }
        }

        private static string ParsePage(HtmlDocument hd, ICollection<ParsedMovie> movies) {
            HtmlNode searchResults = hd.GetElementbyId("trailers_list");
            searchResults = searchResults.SelectSingleNode("ul[1]");
            if (searchResults.ChildNodes.Count == 0) {
                return null;
            }

            foreach (HtmlNode movie in searchResults.SelectNodes("li[@title]")) {
                string sloName = null;
                string url = null;
                string origName = WebUtility.HtmlDecode(movie.Attributes["title"].Value);

                HtmlNode movieLink = movie.SelectSingleNode("a[@href]");
                if (movieLink != null) {
                    url = movieLink.Attributes["href"].Value;

                    HtmlNode sloNameNode = movieLink.SelectSingleNode("img[@alt]");
                    if (sloNameNode != null) {
                        sloName = WebUtility.HtmlDecode(sloNameNode.Attributes["alt"].Value);

                        if (!string.IsNullOrEmpty(sloName) && origName != sloName) {
                            origName = origName.Replace(sloName, "").Trim(' ', '(', ')');
                        }
                    }
                }
                movies.Add(new ParsedMovie(origName, sloName, url));
            }

            HtmlNode pagination = searchResults.SelectSingleNode("//div[@class='pagination']");

            if (pagination != null) {
                HtmlNode forward = pagination.SelectSingleNode("a[@href and text() = '&rsaquo;']");

                if (forward != null) {
                    return forward.Attributes["href"].Value;
                }
            }
            return null;
        }

        #endregion

        #region Movie information parsing

        public override ParsedMovieInfo ParseMovieInfo(ParsedMovie movie) {
            HtmlDocument hd = DownloadWebPage(movie.Url);

            return hd != null
                       ? ParseMovieInfo(hd.GetElementbyId("trailer_info"))
                       : null;
        }

        private ParsedMovieInfo ParseMovieInfo(HtmlNode movieInfo) {
            if (movieInfo == null) {
                return null;
            }

            ParsedMovieInfo mi = new ParsedMovieInfo();

            mi.Plot = movieInfo.SelectSingleNode("//div[@id='short_desc']/div[2]").InnerTextOrNull();
            HtmlNode right = movieInfo.SelectSingleNode("//div[@id='rightData']");

            mi.ImdbLink = right.SelectSingleNode(string.Format(XPATH, "Imdb:")).InnerTextOrNull(false);
            mi.Genres = right.SelectSingleNode(string.Format(XPATH, "Žanr:")).InnerTextSplitOrNull(true, ',');

            mi.Directors = right.SelectSingleNode(string.Format(XPATH, "Režija:"))
                                .InnerTextSplitOrNull(true, ',')
                                .Where(d => d != null)
                                .Select(d => new ParsedPerson(d));

            mi.Actors = right.SelectSingleNode(string.Format(XPATH, "Igrajo:"))
                             .InnerTextSplitOrNull(true, ',')
                             .Where(d => d != null)
                             .Select(d => new ParsedActor(d));

            mi.Writers = right.SelectSingleNode(string.Format(XPATH, "Scenarij:"))
                              .InnerTextSplitOrNull(true, ',')
                              .Where(d => d != null)
                              .Select(d => new ParsedPerson(d));

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

                IParsedVideo video = mi.Videos.FirstOrDefault(v => v.Type == VideoType.Trailer);
                if (video != null) {
                    mi.TrailerUrl = video.Url;
                }
            }
            else {
                HtmlNode trailer = movieInfo.SelectSingleNode("//div[@id='trailer_video_main']/iframe[@src]");
                if (trailer != null) {
                    mi.TrailerUrl = trailer.Attributes["src"].Value;
                }
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

            HtmlDocument hd = DownloadWebPage(string.Format(TRAILER_URL, videoId));
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