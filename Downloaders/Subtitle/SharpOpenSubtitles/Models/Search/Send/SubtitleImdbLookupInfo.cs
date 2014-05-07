using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {

    public class SubtitleImdbLookupInfo {

        public SubtitleImdbLookupInfo() {
            
        }

        public SubtitleImdbLookupInfo(string imdbId, IEnumerable<string> languageAlpha3) {
            ImdbID = imdbId;
            SubLanguageID = string.Join(",", languageAlpha3);
        }

        /// <summary>Initializes a new instance of the <see cref="SubtitleLookupInfo"/> class.</summary>
        public SubtitleImdbLookupInfo(string imdbId, params string[] subLanguages) : this(imdbId, (IEnumerable<string>)subLanguages){
        }
         
        /// <summary>List of language ISO639-3 language codes to search for, divided by ',' (e.g. 'cze,eng,slo'), see <see cref="IOpenSubtitles.GetSubLanguages">GetSubLanguages(string)</see> function for a list of available languages</summary>
        [XmlRpcMember("sublanguageid")]
        public string SubLanguageID;

        /// <summary>​IMDb ID of movie this video is part of, belongs to.</summary>
        [XmlRpcMember("imdbid")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string ImdbID;
    }

}