namespace Frost.SharpOpenSubtitles.Models.Upload {
    public class UploadedSubtitle {

        /// <summary></summary>
        public string IDMovieImdb;

        /// <summary></summary>
        public string MovieName;

        /// <summary></summary>
        public string MovieYear;

        /// <summary></summary>
        public string MovieHash;

        /// <summary></summary>
        public string MovieReleaseName;

        /// <summary></summary>
        public string IDMovie;

        /// <summary></summary>
        public string MovieNameEng;

        /// <summary></summary>
        public string MovieImdbRating;

        /// <summary>0 means new moviehash was inserted into the database.</summary>
        public string MoviefilenameWasAlreadyInDb;

        /// <summary>0 means new moviefilename was inserted into the database.</summary>
        public string HashWasAlreadyInDb;
    }
}