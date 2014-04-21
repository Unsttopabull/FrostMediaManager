using System;
using System.Threading.Tasks;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers.GremoVKino {

    [Serializable]
    public class GremoVKinoMovieInfo : ParsedMovieInfo {
        private const string TRAILER_URL = "http://www.gremovkino.si/trailer_single/show/{0}/576/344/1";
        private const string XPATH = "table/tr/td[@class='trailer_leftCell' and text()='{0} ']/following-sibling::td[@class='trailer_rightCell']";
        private const string YOUTUBE = "http://www.youtube.com/";
        private const string YOUTUBE_VIDEO_URL = "http://www.youtube.com/watch?v={0}";

        public Task<IParsedMovieInfo> ParseMoviePage(string url) {
            IParsedMovieInfo info = this;

            return Task.Run(() => {
                IsFinished = false;

                HtmlDocument hd = DownloadWebPage(url);

                if (hd == null) {
                    return null;
                }

                ParseMovieInfo(hd.GetElementbyId("trailer_info"));

                IsFinished = true;
                return info;
            });
        }

        private void ParseMovieInfo(HtmlNode movieInfo) {
            if (movieInfo == null) {
                return;
            }

            Summary = movieInfo.SelectSingleNode("//div[@id='short_desc']/div[2]").InnerTextOrNull();
            HtmlNode right = movieInfo.SelectSingleNode("//div[@id='rightData']");

            ImdbLink = right.SelectSingleNode(string.Format(XPATH, "Imdb:")).InnerTextOrNull();
            Genres = right.SelectSingleNode(string.Format(XPATH, "Žanr:")).InnerTextSplitOrNull(',');
            Directors = right.SelectSingleNode(string.Format(XPATH, "Režija:")).InnerTextSplitOrNull(',');
            Writers = right.SelectSingleNode(string.Format(XPATH, "Scenarij:")).InnerTextSplitOrNull(',');
            Country = right.SelectSingleNode(string.Format(XPATH, "Država:")).InnerTextOrNull();
            Duration = right.SelectSingleNode(string.Format(XPATH, "Trajanje:")).InnerTextOrNull();
            OfficialSite = right.SelectSingleNode(string.Format(XPATH, "Uradna stran:")).InnerTextOrNull();

            HtmlNodeCollection videos = movieInfo.SelectNodes("//div[@id='subVideos']/table/tr[position() > 2]");
            if (videos != null) {
                Task[] arr = new Task[videos.Count];
                for (int i = 0; i < videos.Count; i++) {
                    arr[i] = ParseVideoList(videos[i]);
                }

                Task.WaitAll(arr);
            }
        }

        private async Task ParseVideoList(HtmlNode video) {
            string videoId = video.GetAttributeValue("onclick", null);
            if (videoId != null) {
                ParsedVideo pv = new ParsedVideo();

                pv.Url = await GetVideoUrl(videoId);
                if (string.IsNullOrEmpty(pv.Url)) {
                    return;
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

                Videos.Add(pv);
            }
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

        private Task<string> GetVideoUrl(string videoId) {
            int idxStart = videoId.IndexOf(',');
            int idxEnd = videoId.IndexOf(',', idxStart + 1);

            videoId = videoId.Substring(++idxStart, idxEnd - idxStart);

            HtmlDocument hd = DownloadWebPage(string.Format(TRAILER_URL, videoId));
            HtmlNode urlNode = hd.DocumentNode.SelectSingleNode("//embed[@src and @type='application/x-shockwave-flash']");
            if (urlNode != null) {
                string url = urlNode.Attributes["src"].Value;
                if (url.StartsWith(YOUTUBE)) {
                    int queryParam = url.IndexOf('&');
                    int dirEnd = url.LastIndexOf('/');

                    if (queryParam != -1) {
                        url = url.Substring(++dirEnd, queryParam - dirEnd);
                        url = string.Format(YOUTUBE_VIDEO_URL, url);
                    }
                }
                return Task.FromResult(url);
            }
            return Task.FromResult<string>(null);
        }
    }

}