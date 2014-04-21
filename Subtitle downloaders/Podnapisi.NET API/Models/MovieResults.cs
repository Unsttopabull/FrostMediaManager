using System.Collections;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class MovieResults : XmlRpcStruct, IEnumerable<MovieMatch> {

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<MovieMatch> GetEnumerator() {
            IEnumerator enumerator = base.GetEnumerator();
            while (enumerator.MoveNext()) {
                DictionaryEntry entry = (DictionaryEntry) enumerator.Current;

                MovieMatch mm = new MovieMatch((string)entry.Key);
                XmlRpcStruct value = (XmlRpcStruct) entry.Value;

                if (value.Contains("movieId")) {
                    mm.MovieId = (int) value["movieId"];
                }

                if (value.Contains("movieTitle")) {
                    mm.MovieTitle = (string) value["movieTitle"];
                }

                if (value.Contains("movieYear")) {
                    mm.MovieYear = (int) value["movieYear"];
                }

                if (value.Contains("movieType")) {
                    mm.MovieType = (MediaType) value["movieType"];
                }

                if (value.Contains("tvSeason")) {
                    mm.TvSeason = (int) value["tvSeason"];
                }

                if (value.Contains("tvEpisode")) {
                    mm.TvEpisode = (int) value["tvEpisode"];
                }

                if (value.Contains("subtitles")) {
                    mm.Subtitles = ParseSubtitles((XmlRpcStruct[]) value["subtitles"]);
                }

                yield return mm;
            }
        }

        private SubtitleResult[] ParseSubtitles(XmlRpcStruct[] subtitles) {
            SubtitleResult[] subs = new SubtitleResult[subtitles.Length];

            for (int i = 0; i < subtitles.Length; i++) {
                subs[i] = new SubtitleResult();

                if (subtitles[i].Contains("id")) {
                    subs[i].ID = (int) subtitles[i]["id"];
                }

                if (subtitles[i].Contains("lang")) {
                    subs[i].LanguageCode = (string) subtitles[i]["lang"];
                }

                if (subtitles[i].Contains("uploader")) {
                    subs[i].Uploader = (string) subtitles[i]["uploader"];
                }

                if (subtitles[i].Contains("uploaderId")) {
                    subs[i].UploaderId = (int) subtitles[i]["uploaderId"];
                }

                if (subtitles[i].Contains("weight")) {
                    subs[i].MatchRanking = (int) subtitles[i]["weight"];
                }

                if (subtitles[i].Contains("release")) {
                    subs[i].Release = (string) subtitles[i]["release"];
                }

                if (subtitles[i].Contains("flags")) {
                    subs[i].Flags = (string) subtitles[i]["flags"];

                    if (string.IsNullOrEmpty(subs[i].Flags)) {
                        subs[i].Flags = null;
                    }
                }

                if (subtitles[i].Contains("rating")) {
                    subs[i].Rating = (int) subtitles[i]["rating"];
                }

                if (subtitles[i].Contains("inexact")) {
                    subs[i].Inexact = (bool) subtitles[i]["inexact"];
                }
            }
            return subs;
        }
    }
}
