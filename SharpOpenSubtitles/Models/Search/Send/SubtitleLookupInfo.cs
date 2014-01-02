﻿using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search.Send {
    public class SubtitleLookupInfo {

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