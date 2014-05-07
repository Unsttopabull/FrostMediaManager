using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class SubtitleLookupInfo {

        /// <summary>Initializes a new instance of the <see cref="SubtitleLookupInfo"/> class.</summary>
        public SubtitleLookupInfo() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="SubtitleLookupInfo"/> class.</summary>
        public SubtitleLookupInfo(string movieHash, double movieByteSize, IEnumerable<string> subLanguageIDs) {
            MovieHash = movieHash;
            MovieByteSize = movieByteSize;            
            SubLanguageID = string.Join(",", subLanguageIDs);
        }

        /// <summary>Initializes a new instance of the <see cref="SubtitleLookupInfo"/> class.</summary>
        public SubtitleLookupInfo(string movieHash, double movieByteSize, params string[] subLanguageIDs) : this(movieHash, movieByteSize, (IEnumerable<string>)subLanguageIDs) {
        }

        /// <summary>List of language ISO639-3 language codes to search for, divided by ',' (e.g. 'cze,eng,slo'), see <see cref="IOpenSubtitles.GetSubLanguages">GetSubLanguages(string)</see> function for a list of available languages</summary>
        [XmlRpcMember("sublanguageid")]
        public string SubLanguageID;

        /// <summary>Video file hash as calculated by one of the implementation functions as seen on <a href="http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes">Hash Source Codes page</a>.</summary>
        [XmlRpcMember("moviehash")]
        public string MovieHash;

        /// <summary>Size of video file in byte.s</summary>
        [XmlRpcMember("moviebytesize")]
        public double MovieByteSize;

        /// <summary>​IMDb ID of movie this video is part of, belongs to.</summary>
        [XmlRpcMember("imdbid")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string ImdbID;
    }
}