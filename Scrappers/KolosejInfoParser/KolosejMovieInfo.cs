using System;
using System.Threading.Tasks;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers.Kolosej {

    [Serializable]
    public class KolosejMovieInfo : ParsedMovieInfo {

        public Task<IParsedMovieInfo> ParseMoviePage(string url) {
            IParsedMovieInfo info = this;
            return Task.Run(() => {
                IsFinished = false;

                HtmlDocument hd = DownloadWebPage(url);

                if (hd == null) {
                    return null;
                }

                HtmlNode mainContent = hd.GetElementbyId("main-content-one-column");
                HtmlNode movieInfo = mainContent.SelectSingleNode("//div[@class='movie-info']");
                ParseMovieInfo(movieInfo);

                Summary = mainContent.SelectSingleNode("//div[@class='summary']").InnerTextOrNull();

                HtmlNode trailer = mainContent.SelectSingleNode("//div[@class='inline-trailer']/iframe[@src]");
                if (trailer != null) {
                    TrailerUrl = trailer.Attributes["src"].Value.Trim();
                }

                IsFinished = true;
                return info;
            });
        }

        private void ParseMovieInfo(HtmlNode movieInfo) {
            if (movieInfo == null) {
                return;
            }

            string distrib = movieInfo.SelectSingleNode("div[@class='distribution']").InnerTextOrNull();
            if (distrib != null) {
                Distribution = distrib.Replace("Film distribucije ", "");
            }

            HtmlNode officialSite = movieInfo.SelectSingleNode("span[@class='title-orig']/a[@href]");
            if (officialSite != null) {
                OfficialSite = officialSite.Attributes["href"].Value;
            }

            HtmlNode duration = movieInfo.SelectSingleNode("span[@class='duration']/text()");
            if (duration != null) {
                Duration = duration.InnerText.Replace('\t', ' ').Replace('\n',' ').Trim();
            }

            ReleaseYear = movieInfo.SelectSingleNode("span[@class='year']/text()").InnerTextOrNull();
            Country = movieInfo.SelectSingleNode("span[@class='country']/a/text()").InnerTextOrNull();
            Language = movieInfo.SelectSingleNode("span[@class='language']/text()").InnerTextOrNull();
            Writers = movieInfo.SelectSingleNode("span[@class='screenplay']/text()").InnerTextSplitOrNull(",", " in ");
            Directors = movieInfo.SelectNodes("span[@class='director']/a/text()").InnerTextOrNull();
            Actors = movieInfo.SelectNodes("span[@class='actors']/a/text()").InnerTextOrNull();

            HtmlNode imdb = movieInfo.SelectSingleNode("//a[text()='IMDB']");
            if (imdb != null && imdb.HasAttributes) {
                ImdbLink = imdb.Attributes[0].Value;
            }
        }
    }

}