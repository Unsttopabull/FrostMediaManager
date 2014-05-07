using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Checking;
using Frost.SharpOpenSubtitles.Models.Movies;
using Frost.SharpOpenSubtitles.Models.Report;
using Frost.SharpOpenSubtitles.Models.Search;
using Frost.SharpOpenSubtitles.Models.Session;
using Frost.SharpOpenSubtitles.Models.UI;
using Frost.SharpOpenSubtitles.Models.Upload;

namespace Frost.SharpOpenSubtitles {

    [XmlRpcUrl("http://api.opensubtitles.org/xml-rpc")]
    public interface IOpenSubtitles : IXmlRpcProxy {

        #region Session

        /// <summary>
        /// Login user <paramref name="userName"/> identified by password <paramref name="password"/> communicating in language <paramref name="language"/> and working in client application useragent <paramref name="userAgent"/>.<br />
        /// This function should be always called when starting communication with OSDb server to identify user, specify application and start a new session (either registered user or anonymous).<br />
        /// If user has no account, blank username and password should be used.
        /// </summary>
        /// <param name="userName">Nickname of user that's trying to login, can be left blank if logging in anonymously.</param>
        /// <param name="password">User's password to verify identity, can be left blank if logging in anonymously.</param>
        /// <param name="language">​ISO639 2-letter language code to specify the language all subsequent communication should use (mainly for error messages).</param>
        /// <param name="userAgent">Identifier of application/useragent that is trying to execute this operation, must be specified, empty parameter is not allowed.</param>
        /// <returns>A status of the request and a session token if successfull.</returns>
        [XmlRpcMethod]
        LogInInfo LogIn(string userName, string password, string language, string userAgent);

        /// <summary>This will logout user identified by token token. This function should be called just before exiting/closing client application.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SessionInfo LogOut(string token);

        /// <summary>
        /// This function is used to keep the session token alive while client application is idling.<br />
        /// Should be called every 15 minutes between XML-RPC requests (in case user is idle or client application is not currently communicating with OSDb server) to keep the connection alive while client application is still running.<br />
        /// It can be also used to check if given token is still active.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SessionInfo NoOperation(string token);

        #endregion

        #region Search & Download

        /// <summary>Search for subtitle files matching your videos using either video file hashes or IMDb IDs.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="lookup">The Subtitle lookup information.</param>
        /// <remarks>This function can be used to search for subtitle files. There are two ways to call it:
        /// <list type="number">
        /// <item>
        ///     <description>using video file hashes (more at once allowed):
        ///         <list type="bullet">
        ///             <item><description>Search the database using video file hashes to get exact matches for your video files.</description></item>
        ///         </list>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>using IMDb IDs:
        ///         <list type="bullet">
        ///             <item><description>If method 1 returns no subtitle files, you can use this method to search for subtitle files matching given imdbid.</description></item>
        ///             <item><description>You'll most probably have to synchronize the subtitles yourself or try more to find a match. If you find one, please, contribute by uploading them using UploadSubtitles method.</description></item>
        ///             <item><description>When this method is used you don't have to specify moviehash and moviebytesize.</description></item>
        ///             <item><description>Some fields (IDSubMovieFile, MovieHash, MovieByteSize, MovieTimeMS) are missing in output when using this method.</description></item>
        ///         </list>
        ///     </description>
        /// </item>
        /// </list>
        /// If sublanguageid is empty or contains the string 'all' - search is performed for all languages.
        /// Also remember you can <b>not</b> combine imdbid and moviehash searches in one call.
        /// </remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SearchSubtitleInfo SearchSubtitles(string token, SubtitleLookupInfo[] lookup);

        /// <summary>Search for subtitle files matching your videos using either video file hashes or IMDb IDs.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="lookup">The Subtitle lookup information.</param>
        /// <remarks>This function can be used to search for subtitle files. There are two ways to call it:
        /// <list type="number">
        /// <item>
        ///     <description>using video file hashes (more at once allowed):
        ///         <list type="bullet">
        ///             <item><description>Search the database using video file hashes to get exact matches for your video files.</description></item>
        ///         </list>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>using IMDb IDs:
        ///         <list type="bullet">
        ///             <item><description>If method 1 returns no subtitle files, you can use this method to search for subtitle files matching given imdbid.</description></item>
        ///             <item><description>You'll most probably have to synchronize the subtitles yourself or try more to find a match. If you find one, please, contribute by uploading them using UploadSubtitles method.</description></item>
        ///             <item><description>When this method is used you don't have to specify moviehash and moviebytesize.</description></item>
        ///             <item><description>Some fields (IDSubMovieFile, MovieHash, MovieByteSize, MovieTimeMS) are missing in output when using this method.</description></item>
        ///         </list>
        ///     </description>
        /// </item>
        /// </list>
        /// If sublanguageid is empty or contains the string 'all' - search is performed for all languages.
        /// Also remember you can <b>not</b> combine imdbid and moviehash searches in one call.
        /// </remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SearchSubtitleInfo SearchSubtitles(string token, SubtitleImdbLookupInfo[] lookup);

        /// <summary>Schedule a periodical search for subtitles matching given video files, send results to user's e-mail address.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="sublangs">Array of subtitle file language IDs (ISO639-3 codes), if no languages are specified (array is empty), system will try to find subtitles in all languages.</param>
        /// <param name="videos">Array of video file information.</param>
        /// <remarks>
        /// This function will perodically search for subtitles in languages sublangs and matching video files videos and send the results to user's e-mail address <br />
        /// Available only to registered users.<br /><br />
        /// <c>Scenario</c>: user has a directory with movies he cannot find subtitles to.
        /// <list type="bullet">
        ///     <item><description>With this function he can subscribe to possible results, when someone else uploads matching subtitles.</description></item>
        ///     <item><description>Once a day/week (based on user's profile) the system will send subtitle link by e-mail to user's e-mail address.</description></item>
        /// </list>
        /// </remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SessionInfo SearchToMail(string token, string[] sublangs, VideoInfo[] videos);

        /// <summary>Downloads given subtitle files.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="data">Array of subtitle file IDs</param>
        /// <remarks>This function can be used to download multiple subtitle files at once.</remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        DownloadInfo DownloadSubtitles(string token, int[] data);

        #endregion

        #region Upload

        /// <summary>Try to upload subtitles, perform pre-upload checking (i.e. check if subtitles already exist on server).</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="subs">Subtitle file information, contains one or more <see cref="Frost.SharpOpenSubtitles.Models.Upload.SubFile">SubFile</see> structures</param>
        /// <remarks>
        /// This function is used to perform pre-upload checking on given subtitle files subfiles. It will check if given subtitles are well-formed, if they already exist on the server, etc.
        /// <list type="bullet">
        ///     <item><description>This must be called before <see cref="UploadSubtitles">UploadSubtitles(string, UploadInfo)</see>.</description></item>
        ///     <item><description>It takes 2 parameters. First is the session <c>token</c>, second is a structure of information for subtitles to be uploaded, minimum cd1 (one subtitle file) is required.</description></item>
        ///     <item><description>The function returns <c>alreadyindb = 1</c> when subtitles already exist in the database.</description></item>
        ///     <item>
        ///         <description>When they do not exist, <see cref="SearchSubtitles">SearchSubtitles(string, SubtitleLookupInfo)</see> is called and API tries to find existing subtitles based on moviehash/moviebytesize.<br />
        ///                      If some results are found, information is returned in <c>data</c> key as an array of <see cref="Frost.SharpOpenSubtitles.Models.Upload.UploadedSubtitle">UploadedSubtitle</see> structure. This is good for uploading - user should have <c>imdbid</c> field already filled.
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        TryUploadInfo TryUploadSubtitles(string token, XmlRpcStruct subs);

        /// <summary>Uploads given subtitles to OSDb server.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="data">Upload data structure consists of 2+ parts: basic upload information baseinfo and subtitle files cd1, cd2, ...</param>
        /// <remarks>
        /// This function takes care of uploading subtitles to OSDb server and should be called after <see cref="TryUploadSubtitles">TryUploadSubtitles(string, XmlRpcStruct)</see>.<br />
        /// Most of the information is the same as in <see cref="TryUploadSubtitles">TryUploadSubtitles(string, XmlRpcStruct)</see>, the important part is subcontent.<br />
        /// It should be gzipped (without header) and then base64-encoded contents of the subtitle file.<br />
        /// </remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        UploadStatusInfo UploadSubtitles(string token, UploadSubtitleInfo data);

        #endregion

        #region Movies

        /// <summary>Searches for movies matching given movie title <paramref name="query"/>.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="query">Movie title user is searching for, this is cleaned-up a bit (remove dvdrip, etc.) before searching.</param>
        /// <returns>
        /// Returns array of movies data found on IMDb.com and in internal server movie database.<br />
        /// Manually added movies can be identified by ID starting at 10000000.
        /// </returns>
        [XmlRpcMethod]
        ImdbSearchInfo SearchMoviesOnIMDB(string token, string query);

        /// <summary>Get details about given movie.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="imdbID">MDb ID of requested movie, can be taken from results of <see cref="SearchMoviesOnIMDB">SearchMoviesOnIMDB(string, string)</see></param>
        /// <returns>
        /// Returns structure with movie information about given movie <paramref name="imdbID"/> containing movie title, release year, directors, cast, ...<br />
        /// All information is gathered from ​<a href="www.imdb.com">IMDb</a>.
        /// </returns>
        [XmlRpcMethod]
        ImdbMovieDetailsInfo GetIMDBMovieDetails(string token, string imdbID);

        /// <summary>Allows registered users to insert new movies (not stored in IMDb) to the database.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
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
        [XmlRpcMethod]
        InsertMovieStatus InsertMovie(string token, InsertMovieInfo movieinfo);

        #endregion

        #region Reporting & rating

        /// <summary>Get some basic server information.</summary>
        /// <returns>This function returns a structure with basic server information (urls, contacts) and some statistics, including number of users currently online.</returns>
        [XmlRpcMethod]
        ServerInfo ServerInfo();

        /// <summary>Report wrong subtitle file to video file combination.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="subMovieFileID">Identifier of the subtitle file to video file combination</param>
        /// <remarks>
        /// This method is used to report subtitle file to video file combination, i.e. subtitle contents are correct but but they are not synchronized to match this video file<br />
        /// With this method number of reports is counted in database and after a certain number of reports is reached, hash is automatically deleted from the database.
        /// </remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SessionInfo ReportWrongMovieHash(string token, [XmlRpcParameter("IDSubMovieFile")] string subMovieFileID);

        /// <summary>Allows registered users to rate subtitle idsubtitle giving a score score where <c>1</c> is worst and <c>10</c> is best.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="vote">Structure to rate (cast a vote) on a subtitle.</param>
        /// <remarks>Users cannot rate subtitles they uploaded and each user can rate a specific subtitle only once.</remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        VoteInfo SubtitlesVote(string token, SubtitleVote vote);

        /// <summary>Allows registered users to add a new comment to subtitle.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="data">Comment input data structure.</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        SessionInfo AddComment(string token, SubtitleComment data);

        #endregion

        #region User Interface

        /// <summary>Get list of supported subtitle languages-</summary>
        /// <param name="language">
        /// ISO639-1 2-letter language code of user's interface language.<br />
        /// Language names (LanguageName) will be returned in this language.
        /// </param>
        /// <returns>Returns list of supported subtitle languages.</returns>
        [XmlRpcMethod]
        SubLanguageInfo GetSubLanguages(string language = "en");

        /// <summary>Detect language for given strings.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="data">
        /// Array of strings you want language detected for.
        /// <list type="bullet">
        ///     <item><description>These strings should be gzipped (without header) and then base64-encoded (to improve transfer speed and save bandwidth).</description></item>
        ///     <item><description>Strings should be at least 4096 bytes long for good results - but you can send the whole subtitle contents.</description></item>
        /// </list>
        /// </param>
        /// <remarks>Given an array of strings data the function will return a structure with detected languages for all the strings given as parameters.</remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod]
        LangDetectInfo DetectLanguage(string token, string[] data);

        /// <summary>Get available translations for given program</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="program">Name of the program/client application you want translations for (currently supported: subdownloader, oscar).</param>
        /// <returns>Returns a structure containing all available translations for a program program.</returns>
        [XmlRpcMethod]
        AvailableTranslationsInfo GetAvailableTranslations(string token, string program);

        /// <summary>Get a translation for given program and language.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="iso639">Language ​ISO639-1 2-letter code.</param>
        /// <param name="format">
        /// Available formats:
        /// <list type="bullet">
        ///     <item><description>gnugettext compatible: mo, po</description></item>
        ///     <item><description>additional: txt, xml</description></item>
        /// </list>
        /// </param>
        /// <param name="program">Name of the program/client application you want translations for (currently supported: subdownloader, oscar).</param>
        /// <remarks>
        /// This function is used to provide multi-language support for client applications in a single environment.<br />
        /// The language translation strings are stored on the server which provides user interface to edit/update them.<br />
        /// Updated strings can then be downloaded to client application using this method.
        /// </remarks>
        /// <returns>Returns base64-encoded translation strings for program <paramref name="program"/> and language <paramref name="iso639"/> in format <paramref name="format"/>.</returns>
        [XmlRpcMethod]
        Translation GetTranslation(string token, string iso639, string format, string program);

        /// <summary>Checks for the latest version of application <paramref name="programName"/>.</summary>
        /// <param name="programName">Name of the program/client application you want to check (currently supported: subdownloader, oscar).</param>
        /// <returns>Returns download links and changelog (latest features and bugfixes).</returns>
        [XmlRpcMethod]
        UpdateInfo AutoUpdate([XmlRpcParameter("program_name")] string programName);

        #endregion

        #region Checking

        /// <summary>Checks if given video file hashes <paramref name="hashes"/> are already stored in the database.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="hashes">Array of video file hashes.</param>
        /// <returns>
        /// If found, the server will return basic movie information, including IMDb ID, movie title, release year.<br />
        /// This information can be then used in client application to automatically fill (or verify) movie info.
        /// </returns>
        [XmlRpcMethod]
        MovieHashInfo CheckMovieHash(string token, string[] hashes);

        /// <summary>Check if given subtitle files are already stored in the database.</summary>
        /// <param name="token">Token string identifying user's session, taken from <see cref="LogIn">LogIn(string, string, string, string)</see> result structure.</param>
        /// <param name="hashes">Array of subtitle file hashes (MD5 hashes of subtitle file contents)</param>
        /// <returns>
        /// This function returns subtitle file IDs for given subtitle file hashes.<br />
        /// This can be used to quickly check (for a large list of subtitle files) which subtitle files are already stored in the database (e.g. before uploading).
        /// </returns>
        [XmlRpcMethod]
        SubtitleHashInfo CheckSubHash(string token, string[] hashes);

        #endregion
    }

}