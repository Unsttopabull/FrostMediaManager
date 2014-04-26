using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharpTraktTvAPI.Models.Movie;
using SharpTraktTvAPI.Models.Movie.SummariesModels;

namespace SharpTraktTvAPI {

    public enum SummaryLength {
        Default,
        Normal,
        Full
    }

    public class MovieApi {
        private readonly string _apiKey;

        public MovieApi(string apiKey) {
            _apiKey = apiKey;
        }

        #region Summary

        public MovieSummaryResponse Summary(string titleSlug) {
            return GetSummary(titleSlug);
        }

        public MovieSummaryResponse SummaryByTitleAndYear(string title, int year) {
            return GetSummary(title.Replace(" ", "-") + "-" + year);
        }

        public MovieSummaryResponse SummaryById(string id) {
            return GetSummary(id);
        }

        public MovieSummaryResponse GetSummary(string value) {
            URLBuilder url = new URLBuilder(SharpTraktTv.API_URL_BASE);
            url.AddSegmentPath("movie", "summary.json", _apiKey, value);

            return url.GetResposeAs<MovieSummaryResponse>();
        }

        #endregion

        #region Summaries

        public MovieSummaryResponse Summaries(SummaryLength extended, params MovieSummarySearch[] values) {
            return GetSummaries(values.Select(ms => ms.GetSlug()));
        }

        public MovieSummaryResponse Summaries(params MovieSummarySearch[] values) {
            return GetSummaries(values.Select(ms => ms.GetSlug()));
        }

        public MovieSummaryResponse SummariesById(params string[] id) {
            return GetSummaries(id);
        }

        public MovieSummaryResponse GetSummaries(IEnumerable<string> values, SummaryLength length = SummaryLength.Default) {
            URLBuilder url = new URLBuilder(SharpTraktTv.API_URL_BASE);
            url.AddSegmentPath("movie", "summaries.json", _apiKey, string.Join(",", values));

            if (length != SummaryLength.Default) {
                url.AddSegment(length.ToString().ToLowerInvariant());
            }

            return url.GetResposeAs<MovieSummaryResponse>();
        }

        #endregion

    }
}
