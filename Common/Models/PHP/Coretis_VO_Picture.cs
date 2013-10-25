namespace Common.Models.PHP {
    public class Coretis_VO_Picture {

        #region Konstante
        ///<summary>The DVD / Blu-ray cover art.</summary>
        public const string TYPE_POSTER = "poster";

        ///<summary>The background picture. Called backdrop on some scrapers</summary>
        public const string TYPE_FANART = "fanart";

        ///<summary>The background picture. Called backdrop on some scrapers</summary>
        public const string TYPE_SCREEN = "screen";

        ///<summary>The original size.</summary>
        public const string SIZE_ORIGINAL = "original";

        ///<summary>The mid/preview size.</summary>
        public const string SIZE_MID = "mid";

        ///<summary>The thumb/cover size.</summary>
        public const string SIZE_THUMB = "thumb";
        #endregion

        public Coretis_VO_Picture() {
        }

        public Coretis_VO_Picture(string picId, string type, string size, string width, string height, string url) {
            this.picId = picId;
            this.type = type;
            this.size = size;
            this.width = height;
            this.url = url;
        }

        ///<summary>The id for this row in DB</summary>
        public long id;

        ///<summary>Scraper"s picture Id prefixed with scraper"s name</summary>
        ///<example>tmdb4bc92150017a3c57fe00d378</example>
        public string picId;

        ///<summary>Picture type (poster, fanart)</summary>
        /// <example>poster (portrait) / fanart (landscape)</example>
        public string type;

        ///<summary>Picture type (original, mid, thumb)</summary>
        /// <example>original</example>
        public string size;

        ///<summary>Picture width in pixels</summary>
        /// <example>1280</example>
        public string width;

        ///<summary>Picture height in pixels</summary>
        /// <example>720</example>
        public string height;

        ///<summary>Picture url</summary>
        /// <example>http://passion-xbmc.org/scraper/Gallery/main/Poster_Transformers2laRevanche-288440.jpg</example>
        public string url;
    }
}

