using System.Globalization;
using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Checking;
using Frost.SharpOpenSubtitles.Models.Movies;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles {

    public class Movie {
        private readonly OpenSubtitlesClient _rpc;

        internal Movie(OpenSubtitlesClient rpc) {
            _rpc = rpc;
        }

        /// <summary>Searches for movies matching given movie title <paramref name="query"/>.</summary>
        /// <param name="query">Movie title user is searching for, this is cleaned-up a bit (remove dvdrip, etc.) before searching.</param>
        /// <returns>
        /// Returns array of movies data found on IMDb.com and in internal server movie database.<br />
        /// Manually added movies can be identified by ID starting at 10000000.
        /// </returns>
        public ImdbSearchInfo SearchOnIMDB(string query) {
            return _rpc.Proxy.SearchMoviesOnIMDB(_rpc.Token, query);
        }

        /// <summary>Get details about given movie.</summary>
        /// <param name="imdbID">IMDb ID (without leading 'tt') of requested movie, can be taken from results of <see cref="SearchOnIMDB">SearchOnIMDB(string)</see></param>
        /// <returns>
        /// Returns structure with movie information about given movie <paramref name="imdbID"/> containing movie title, release year, directors, cast, ...<br />
        /// All information is gathered from ​<a href="www.imdb.com">IMDb</a>.
        /// </returns>
        public ImdbMovieDetailsInfo GetImdbDetails(string imdbID) {
            return _rpc.Proxy.GetIMDBMovieDetails(_rpc.Token, imdbID);
        }

        /// <summary>Get details about given movie.</summary>
        /// <param name="imdbID">MDb ID of requested movie, can be taken from results of <see cref="SearchOnIMDB">SearchOnIMDB(string)</see></param>
        /// <returns>
        /// Returns structure with movie information about given movie <paramref name="imdbID"/> containing movie title, release year, directors, cast, ...<br />
        /// All information is gathered from ​<a href="www.imdb.com">IMDb</a>.
        /// </returns>
        public ImdbMovieDetailsInfo GetImdbDetails(int imdbID) {
            return _rpc.Proxy.GetIMDBMovieDetails(_rpc.Token, imdbID.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>Allows registered users to insert new movies (not stored in IMDb) to the database.</summary>
        /// <param name="movieinfo">Information about the movie to be inserted.</param>
        /// <remarks>
        /// Guidelines for implementation:
        /// <list type="bullet">
        ///     <item><description>When loading a movie first try to auto-detect IMDb ID from .NFO files accompanying the release.</description></item>
        ///     <item><description>When uploading without IMDb ID user enters a movie title (and year) first call SearchMoviesOnIMDB().</description></item>
        ///     <item><description>If this returns no matches and user checks that the movie doesn't exist on IMDb, allow to insert a new movie using this function.</description></item>
        ///     <item><description>This needs to be done to avoid duplicates.</description></item>
        /// </list>
        /// </remarks>
        /// <returns>TBD</returns>
        public InsertMovieStatus Insert(InsertMovieInfo movieinfo) {
            return _rpc.Proxy.InsertMovie(_rpc.Token, movieinfo);
        }

        /// <summary>Allows registered users to insert new movies (not stored in IMDb) to the database.</summary>
        /// <param name="movieName">The movie title.</param>
        /// <param name="movieYear">The movie release year.</param>
        /// <remarks>
        /// Guidelines for implementation:
        /// <list type="bullet">
        ///     <item><description>When loading a movie first try to auto-detect IMDb ID from .NFO files accompanying the release.</description></item>
        ///     <item><description>When uploading without IMDb ID user enters a movie title (and year) first call SearchMoviesOnIMDB().</description></item>
        ///     <item><description>If this returns no matches and user checks that the movie doesn't exist on IMDb, allow to insert a new movie using this function.</description></item>
        ///     <item><description>This needs to be done to avoid duplicates.</description></item>
        /// </list>
        /// </remarks>
        /// <returns>TBD</returns>
        public InsertMovieStatus Insert(string movieName, string movieYear) {
            return _rpc.Proxy.InsertMovie(_rpc.Token, new InsertMovieInfo(movieName, movieYear));
        }

        /// <summary>Checks if given video file hashes <paramref name="hashes"/> are already stored in the database.</summary>
        /// <param name="hashes">Array of video file hashes.</param>
        /// <returns>
        /// If found, the server will return basic movie information, including IMDb ID, movie title, release year.<br />
        /// This information can be then used in client application to automatically fill (or verify) movie info.
        /// </returns>
        public MovieHashInfo CheckHash(string[] hashes) {
            return _rpc.Proxy.CheckMovieHash(_rpc.Token, hashes);
        }

        /// <summary>Report wrong subtitle file to video file combination.</summary>
        /// <param name="idSubMovieFile">Identifier of the subtitle file to video file combination</param>
        /// <remarks>
        /// This method is used to report subtitle file to video file combination, i.e. subtitle contents are correct but but they are not synchronized to match this video file<br />
        /// With this method number of reports is counted in database and after a certain number of reports is reached, hash is automatically deleted from the database.
        /// </remarks>
        /// <returns>TBD</returns>
        public SessionInfo ReportWrongHash([XmlRpcParameter("IDSubMovieFile")] string idSubMovieFile) {
            return _rpc.Proxy.ReportWrongMovieHash(_rpc.Token, idSubMovieFile);
        }
    }

}