using CookComputing.XmlRpc;
using Frost.PodnapisiNET.Models;

namespace Frost.PodnapisiNET {

    [XmlRpcUrl("http://ssp.podnapisi.net:8000/RPC2/")]
    public interface IPodnapisiNet : IXmlRpcProxy {

        [XmlRpcMethod("initiate")]
        LogInInfo Initiate(string useragent);

        [XmlRpcMethod("authenticate")]
        FilterInfo Authenticate(string session, string username, string password);

        /// <summary>Terminates the specified session.</summary>
        /// <param name="session">The session token obtained using <see cref="Initiate">Initiate(string)</see>.</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod("terminate")]
        StatusInfo Terminate(string session);

        /// <summary>Searches for a matching subtitles</summary>
        /// <param name="session">The session token obtained using <see cref="Initiate">Initiate(string)</see>.</param>
        /// <param name="hashes">A list of strings where each of them represents a unique movie file hash calculated from <a href="http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes">MovieHash</a> algorithm.</param>
        /// <remarks>The search will use users language filters that are defined on the site by <see cref="SetFilters">SetFilters(string, bool, string[], bool)</see></remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod("search")]
        //SearchResult Search(string session, string[] hashes);
        SearchResult Search(string session, string[] hashes);

        /// <summary>Sets the language filters to be used when searching.</summary>
        /// <param name="session">The session token obtained using <see cref="Initiate">Initiate(string)</see>.</param>
        /// <param name="restrictSearch">if set to <c>true</c> enables language filters; otherwise they are disabled.</param>
        /// <param name="searchLanguages">An array of language ids (see <see cref="SupportedLanguages">SupportedLanguages()</see>).</param>
        /// <param name="persistant">if set to <c>true</c> the filter persists through sessions; otherwise applies only for current session.</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod("setFilters")]
        FilterInfo SetFilters(string session, bool restrictSearch, [XmlRpcEnumMapping(EnumMapping.Number)] LanguageId[] searchLanguages, bool persistant);

        /// <summary>Sets persistent the language filters to be used when searching.</summary>
        /// <param name="session">The session token obtained using <see cref="Initiate">Initiate(string)</see>.</param>
        /// <param name="restrictSearch">if set to <c>true</c> enables language filters; otherwise they are disabled.</param>
        /// <param name="searchLanguages">An array of language ids (see <see cref="SupportedLanguages">SupportedLanguages()</see>).</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod("setFilters")]
        FilterInfo SetFilters(string session, bool restrictSearch, [XmlRpcEnumMapping(EnumMapping.Number)] LanguageId[] searchLanguages);

        /// <summary>Gets the download information for specified subtitles</summary>
        /// <param name="session">The session token obtained using <see cref="Initiate">Initiate(string)</see>.</param>
        /// <param name="subtitles">An array of subtitle identifiers (subtitleId) which we want to download.</param>
        /// <remarks>User needs to be authenticated to download subtitles through SSP.</remarks>
        /// <returns>TBD</returns>
        [XmlRpcMethod("download")]
        DownloadInfo Download(string session, string[] subtitles);

        /// <summary>Get a list of supported languages.</summary>
        /// <param name="session">The session token obtained using <see cref="Initiate">Initiate(string)</see>.</param>
        /// <returns>TBD</returns>
        [XmlRpcMethod("supportedLanguages")]
        SupportedLanguagesInfo SupportedLanguages(string session);
    }
}
