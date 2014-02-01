using System;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Frost.MovieInfoParsers.PlanetTus {

    [Serializable]
    public class TusMovieInfo : ParsedMovieInfo {

        public Task<TusMovieInfo> ParseMoviePage(string url) {
            TusMovieInfo info = this;
            return Task.Run(() => {
                IsFinished = false;

                HtmlDocument hd = DownloadWebPage(url);

                if (hd == null) {
                    return null;
                }

                HtmlNode center = hd.GetElementbyId("center");
                Summary = GetSummary(center);
                GetInfo(center);

                IsFinished = true;
                return info;
            });
        }

        private void GetInfo(HtmlNode center) {
            HtmlNode movieData = center.SelectSingleNode("//div[@class='movie_data']/dl");

            if (movieData == null) {
                return;
            }

            HtmlNode imdb = movieData.SelectSingleNode("dt/a[@href and text()='IMDB']");
            if (imdb != null) {
                ImdbLink = imdb.Attributes["href"].Value;

                HtmlNode imdbRating = imdb.ParentNode.SelectSingleNode("following-sibling::dd[1]");
                if (imdbRating != null) {
                    ImdbRating = imdbRating.InnerText.Trim();
                }
            }

            Genres = movieData.SelectNodes("dt[text()='Zvrst:']/following-sibling::dd[1]/a").InnerTextOrNull();
            Duration = movieData.SelectSingleNode("dt[text()='Trajanje:']/following-sibling::dd[1]").InnerTextOrNull();
            Actors = movieData.SelectNodes("dt[text()='Igrajo:']/following-sibling::dd[1]/a").InnerTextOrNull();
            Directors = movieData.SelectNodes("dt[text()='Režija:']/following-sibling::dd[1]/a").InnerTextOrNull();
            Writers = movieData.SelectNodes("dt[text()='Scenarij:']/following-sibling::dd[1]/a").InnerTextOrNull();
            Distribution = movieData.SelectSingleNode("dt[text()='Distributer:']/following-sibling::dd[1]/a").InnerTextOrNull();
            OfficialSite = movieData.SelectSingleNode("dt[text()='Spletna stran:']/following-sibling::dd[1]/a").InnerTextOrNull();
            ReleaseYear = movieData.SelectSingleNode("dt[text()='Letnik:']/following-sibling::dd[1]/a").InnerTextOrNull();
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
    }

}