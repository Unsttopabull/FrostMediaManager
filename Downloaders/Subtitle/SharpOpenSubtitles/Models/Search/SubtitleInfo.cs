using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class SubtitleInfo {

        [XmlRpcMember("IDSubMovieFile")]
        public string SubtitleMovieFileId;

        [XmlRpcMember("MovieHash")]
        public string MovieHash;

        [XmlRpcMember("MovieByteSize")]
        public string MovieByteSize;

        [XmlRpcMember("MovieTimeMS")]
        public string MovieTimeMS;

        [XmlRpcMember("IDSubtitleFile")]
        public string SubtitleFileId;


        [XmlRpcMember("SubFileName")]
        public string SubtitleFileName;

        [XmlRpcMember("SubActualCD")]
        public string PartNumber;

        [XmlRpcMember("SubSize")]
        public string SubtitleSize;

        [XmlRpcMember("SubHash")]
        public string SubtitleHash;

        [XmlRpcMember("IDSubtitle")]
        public string SubtitleId;


        [XmlRpcMember("UserID")]
        public string UserID;


        [XmlRpcMember("SubLanguageID")]
        public string SubtitleLanguageId;

        [XmlRpcMember("SubFormat")]
        public string SubtitleFormat;

        [XmlRpcMember("SubSumCD")]
        public string NumParts;

        [XmlRpcMember("SubAuthorComment")]
        public string AuthorComment;

        [XmlRpcMember("SubAddDate")]
        public string DateAdded;

        [XmlRpcMember("SubBad")]
        public string IsBad;

        [XmlRpcMember("SubRating")]
        public string Rating;

        [XmlRpcMember("SubDownloadsCnt")]
        public string DownloadCount;


        [XmlRpcMember("MovieReleaseName")]
        public string MovieReleaseName;

        [XmlRpcMember("IDMovie")]
        public string MovieId;

        [XmlRpcMember("IDMovieImdb")]
        public string ImdbMovieId;

        [XmlRpcMember("MovieName")]
        public string MovieName;

        [XmlRpcMember("MovieNameEng")]
        public string MovieNameEng;

        [XmlRpcMember("MovieYear")]
        public string ReleaseYear;

        [XmlRpcMember("MovieImdbRating")]
        public string MovieImdbRating;


        [XmlRpcMember("UserNickName")]
        public string UploaderName;

        [XmlRpcMember("UserRank")]
        public string UploaderRank;


        [XmlRpcMember("ISO639")]
        public string ISO639;

        [XmlRpcMember("LanguageName")]
        public string LanguageName;


        [XmlRpcMember("SubHearingImpaired")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string IsForHearingImpaired;

        [XmlRpcMember("SubHD")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string IsForHD;


        [XmlRpcMember("SubDownloadLink")]
        public string SubtitleGGzipDownloadLink;

        [XmlRpcMember("ZipDownloadLink")]
        public string SubtitleZipDownloadLink;

        [XmlRpcMember("SubtitlesLink")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string SubtitlesLink;
    }
}