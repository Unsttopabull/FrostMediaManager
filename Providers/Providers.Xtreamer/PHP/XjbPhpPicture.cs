using Frost.PHPtoNET.Attributes;

namespace Frost.Providers.Xtreamer.PHP {

    [PHPName("Coretis_VO_Picture")]
    public class XjbPhpPicture {
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

        public XjbPhpPicture() {
        }

        public XjbPhpPicture(string picId, string type, string size, string width, string height, string path) {
            PictureId = picId;
            Type = type;
            Size = size;
            Width = width;
            Height = height;
            Path = path;
        }

        ///<summary>The id for this row in DB</summary>
        [PHPName("id")]
        public long Id;

        ///<summary>Picture height in pixels</summary>
        /// <example>\eg{ <c>720</c>}</example>
        [PHPName("height")]
        public string Height;

        ///<summary>Scraper"s picture Id prefixed with scraper"s name</summary>
        ///<example>\eg{ <c>tmdb4bc92150017a3c57fe00d378</c>}</example>
        [PHPName("picId")]
        public string PictureId;

        ///<summary>Picture type (original, mid, thumb)</summary>
        ///<example>eg{''<c>original</c>'', ''<c>mid</c>'', ''<c>thumb</c>''}</example>
        [PHPName("size")]
        public string Size;

        ///<summary>Picture type (poster, fanart)</summary>
        ///<example>\eg{ ''<c>poster</c>'' (portrait) or ''<c>fanart</c>'' (landscape)}</example>
        [PHPName("type")]
        public string Type;

        ///<summary>Picture url</summary>
        /// <example>\eg{ <c>http://passion-xbmc.org/scraper/Gallery/main/Poster_Transformers2laRevanche-288440.jpg</c>}</example>
        [PHPName("url")]
        public string Path;

        ///<summary>Picture width in pixels</summary>
        ///<example>\eg{ <c>1280</c>}</example>
        [PHPName("width")]
        public string Width;

    }

}
