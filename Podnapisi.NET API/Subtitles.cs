using Frost.PodnapisiNET.Models;

namespace Frost.PodnapisiNET {

    public class Subtitles {
        private readonly PodnapisiNetClient _rpc;

        public Subtitles(PodnapisiNetClient rpc) {
            _rpc = rpc;
        }

        /// <summary>Searches for a matching subtitles</summary>
        /// <param name="hashes">A list of strings where each of them represents a unique movie file hash calculated from <a href="http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes">MovieHash</a> algorithm.</param>
        /// <remarks>The search will use users language filters that are defined on the site by <see cref="IPodnapisiNet.SetFilters">SetFilters(string, bool, string[], bool)</see></remarks>
        /// <returns>TBD</returns>
        //public SearchResult Search(params string[] hashes) {
        public SearchResult Search(params string[] hashes) {
            _rpc.CheckToken();
            return _rpc.Proxy.Search(_rpc.Token, hashes);
        }

        /// <summary>Sets the language filters to be used when searching.</summary>
        /// <param name="searchLanguages">An array of language ids (see <see cref="IPodnapisiNet.SupportedLanguages">SupportedLanguages()</see>).</param>
        /// <returns>TBD</returns>
        //public FilterInfo SetFilters(bool restrictSearch, string[] searchLanguages, bool persistant = true) {
        public FilterInfo SetFilters(params LanguageId[] searchLanguages) {
            _rpc.CheckToken();

            return _rpc.Proxy.SetFilters(_rpc.Token, true, searchLanguages);
        }

        /// <summary>Sets the language filters to be used when searching.</summary>
        /// <param name="searchLanguages">An array of language ids (see <see cref="IPodnapisiNet.SupportedLanguages">SupportedLanguages()</see>).</param>
        /// <param name="persistant">if set to <c>true</c> the filter persists through sessions; otherwise applies only for current session.</param>
        /// <returns>TBD</returns>
        //public FilterInfo SetFilters(bool restrictSearch, string[] searchLanguages, bool persistant = true) {
        public FilterInfo SetFilters(LanguageId[] searchLanguages, bool persistant) {
            _rpc.CheckToken();

            return _rpc.Proxy.SetFilters(_rpc.Token, true, searchLanguages, persistant);
        }

        public FilterInfo DisableFilters(bool persistant = true) {
            _rpc.CheckToken();

            return _rpc.Proxy.SetFilters(_rpc.Token, false, new LanguageId[] { }, persistant);
        }

        /// <summary>Gets the download information for specified subtitles</summary>
        /// <param name="subtitles">An array of subtitle identifiers (subtitleId) which we want to download.</param>
        /// <remarks>User needs to be authenticated to download subtitles through SSP.</remarks>
        /// <returns>TBD</returns>
        public DownloadInfo Download(params string[] subtitles) {
            _rpc.CheckToken();

            return _rpc.Proxy.Download(_rpc.Token, subtitles);
        }
    }

}