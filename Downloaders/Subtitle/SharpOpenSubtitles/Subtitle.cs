using System;
using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Checking;
using Frost.SharpOpenSubtitles.Models.Report;
using Frost.SharpOpenSubtitles.Models.Search;
using Frost.SharpOpenSubtitles.Models.Session;
using Frost.SharpOpenSubtitles.Models.UI;
using Frost.SharpOpenSubtitles.Models.Upload;

namespace Frost.SharpOpenSubtitles {
    public class Subtitle {
        private readonly OpenSubtitlesClient _rpc;

        public Subtitle(OpenSubtitlesClient rpc) {
            _rpc = rpc;
        }

        #region Search & Download

        /// <summary>Search for subtitle files matching your videos using either video file hashes or IMDb IDs.</summary>
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
        public SearchSubtitleInfo Search(SubtitleLookupInfo[] lookup) {
            return _rpc.Proxy.SearchSubtitles(_rpc.Token, lookup);
        }

        /// <summary>Search for subtitle files matching your videos using either video file hashes or IMDb IDs.</summary>
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
        public SearchSubtitleInfo Search(SubtitleImdbLookupInfo[] lookup) {
            return _rpc.Proxy.SearchSubtitles(_rpc.Token, lookup);
        }

        /// <summary>Schedule a periodical search for subtitles matching given video files, send results to user's e-mail address.</summary>
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
        public SessionInfo SearchToMail(string[] sublangs, VideoInfo[] videos) {
            return _rpc.Proxy.SearchToMail(_rpc.Token, sublangs, videos);
        }

        /// <summary>Downloads given subtitle files.</summary>
        /// <param name="data">Array of subtitle file IDs</param>
        /// <remarks>This function can be used to download multiple subtitle files at once.</remarks>
        /// <returns>TBD</returns>
        public DownloadInfo Download(int[] data) {
            return _rpc.Proxy.DownloadSubtitles(_rpc.Token, data);
        }

        #endregion

        #region Upload

        /// <summary>Try to upload subtitles, perform pre-upload checking (i.e. check if subtitles already exist on server).</summary>
        /// <param name="subs">Subtitle file information, contains one or more <see cref="Frost.SharpOpenSubtitles.Models.Upload.SubFile">SubFile</see> structures</param>
        /// <remarks>
        /// This function is used to perform pre-upload checking on given subtitle files subfiles. It will check if given subtitles are well-formed, if they already exist on the server, etc.
        /// <list type="bullet">
        ///     <item><description>This must be called before <see cref="Upload">Upload(UploadInfo)</see>.</description></item>
        ///     <item><description>It takes 2 parameters. First is the session <c>token</c>, second is a structure of information for subtitles to be uploaded, minimum cd1 (one subtitle file) is required.</description></item>
        ///     <item><description>The function returns <c>alreadyindb = 1</c> when subtitles already exist in the database.</description></item>
        ///     <item>
        ///         <description>When they do not exist, <see cref="Search">Search(SubtitleLookupInfo)</see> is called and API tries to find existing subtitles based on moviehash/moviebytesize.<br />
        ///                      If some results are found, information is returned in <c>data</c> key as an array of <see cref="Frost.SharpOpenSubtitles.Models.Upload.UploadedSubtitle">UploadedSubtitle</see> structure. This is good for uploading - user should have <c>imdbid</c> field already filled.
        ///         </description>
        ///     </item>
        /// </list>
        /// </remarks>
        /// <returns>TBD</returns>
        public TryUploadInfo TryUpload(XmlRpcStruct subs) {
            return _rpc.Proxy.TryUploadSubtitles(_rpc.Token, subs);
        }

        /// <summary>Uploads given subtitles to OSDb server.</summary>
        /// <param name="data">Upload data structure consists of 2+ parts: basic upload information baseinfo and subtitle files cd1, cd2, ...</param>
        /// <remarks>
        /// This function takes care of uploading subtitles to OSDb server and should be called after <see cref="TryUpload">TryUpload(XmlRpcStruct)</see>.<br />
        /// Most of the information is the same as in <see cref="TryUpload">TryUploadSubtitles(XmlRpcStruct)</see>, the important part is subcontent.<br />
        /// It should be gzipped (without header) and then base64-encoded contents of the subtitle file.<br />
        /// </remarks>
        /// <returns>TBD</returns>
        public UploadStatusInfo Upload(UploadSubtitleInfo data) {
            return _rpc.Proxy.UploadSubtitles(_rpc.Token, data);
        }

        #endregion

        #region Rating

        /// <summary>Allows registered users to rate subtitle idsubtitle giving a score score where <c>1</c> is worst and <c>10</c> is best.</summary>
        /// <param name="vote">Structure to rate (cast a vote) on a subtitle.</param>
        /// <remarks>Users cannot rate subtitles they uploaded and each user can rate a specific subtitle only once.</remarks>
        /// <returns>TBD</returns>
        public VoteInfo Vote(SubtitleVote vote) {
            return _rpc.Proxy.SubtitlesVote(_rpc.Token, vote);
        }

        /// <summary>Allows registered users to rate subtitle idsubtitle giving a score score where <c>1</c> is worst and <c>10</c> is best.</summary>
        /// <param name="subtitleID">ID of subtitle (NOT subtitle file) user wants to rate.</param>
        /// <param name="score">Subtitle rating, must be in interval 1 (worst) to 10 (best).</param>
        /// <remarks>Users cannot rate subtitles they uploaded and each user can rate a specific subtitle only once.</remarks>
        /// <returns>TBD</returns>
        public VoteInfo Vote(string subtitleID, double score) {
            return _rpc.Proxy.SubtitlesVote(_rpc.Token, new SubtitleVote(subtitleID, score));
        }

        /// <summary>Allows registered users to add a new comment to subtitle.</summary>
        /// <param name="data">Comment input data structure.</param>
        /// <returns>TBD</returns>
        public SessionInfo AddComment(SubtitleComment data) {
            return _rpc.Proxy.AddComment(_rpc.Token, data);
        }

        /// <summary>Allows registered users to add a new comment to subtitle.</summary>
        /// <param name="subtitleID">Subtitle identifier [BEWARE! this is not the ID of subtitle file but of the whole subtitle (a subtitle can contain multiple subtitle files)].</param>
        /// <param name="comment">User's comment.</param>
        /// <param name="badSubtitle">Optional parameter. If set to <c>true</c>, subtitles are marked as bad.</param>
        /// <returns>TBD</returns>
        public SessionInfo AddComment(int subtitleID, string comment, bool badSubtitle = false) {
            return _rpc.Proxy.AddComment(_rpc.Token, new SubtitleComment(subtitleID, comment, badSubtitle));
        }

        #endregion

        #region User Interface

        /// <summary>Get list of supported subtitle languages-</summary>
        /// <param name="language">
        /// ISO639-1 2-letter language code of user's interface language.<br />
        /// Language names (LanguageName) will be returned in this language.
        /// </param>
        /// <returns>
        /// Returns list of supported subtitle languages in the specified language.<br />
        /// If language code is not recognized or the language is not supported returns in english.
        /// </returns>
        public SubLanguageInfo GetLanguages(string language = "en") {
            if (language.Length > 2) {
                throw new ArgumentException("An ISO369 2 letter code expected, but a string longer than 2 characters was provided.", "language");
            }

            return _rpc.Proxy.GetSubLanguages(language);
        }

        /// <summary>Detect language for given strings.</summary>
        /// <param name="data">
        /// Array of strings you want language detected for.
        /// <list type="bullet">
        ///     <item><description>These strings should be gzipped (without header) and then base64-encoded (to improve transfer speed and save bandwidth).</description></item>
        ///     <item><description>Strings should be at least 4096 bytes long for good results - but you can send the whole subtitle contents.</description></item>
        /// </list>
        /// </param>
        /// <returns>Given an array of strings data the function will return a structure with detected languages for all the strings given as parameters.</returns>
        public LangDetectInfo DetectLanguage(string[] data) {
            return _rpc.Proxy.DetectLanguage(_rpc.Token, data);
        }

        #endregion

        #region Checking

        /// <summary>Check if given subtitle files are already stored in the database.</summary>
        /// <param name="hashes">Array of subtitle file hashes (MD5 hashes of subtitle file contents)</param>
        /// <returns>
        /// This function returns subtitle file IDs for given subtitle file hashes.<br />
        /// This can be used to quickly check (for a large list of subtitle files) which subtitle files are already stored in the database (e.g. before uploading).
        /// </returns>
        public SubtitleHashInfo CheckSubHash(params string[] hashes) {
            return _rpc.Proxy.CheckSubHash(_rpc.Token, hashes);
        }

        #endregion

    }
}
