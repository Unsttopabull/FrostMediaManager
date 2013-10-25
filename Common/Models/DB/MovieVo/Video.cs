using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class Video {

        public Video(string codec, int width, int height) {
            Codec = codec;

            if (height > 0) {
                Height = height;
            }

            if (width > 0) {
                Width = width;
            }            
        }

        public Video(string codec, string format, string type, string source, double aspect, int height, int width) : this(codec, width, height) {
            Format = format;
            Type = type;
            Source = source;

            if (aspect > 0) {
                Aspect = aspect;
            }
        }

        public Video(string codec, string aspect, int height, int width) : this(codec, width, height) {
            double dbl;
            double.TryParse(aspect, out dbl);

            if (dbl > 0) {
                Aspect = dbl;
            }        
        }

        [Key]
        public long Id { get; set; }

        ///<summary>With what this video was made from</summary>
        /// <example>TS TC TELESYNC CAM HDRIP DVDRIP BDRIP DTV HD2DVD HDDVDRIP HDTVRIP VHS SCREENER RECODE</example>
        public string Source { get; set; }

        /// <summary>The type of the video</summary>
        /// <example>XVID DVD5 DVD9 DVDR BLUERAY BD HD2DVD X264</example>
        public string Type { get; set; }

        ///<summary>Resolution and format of the video</summary>
        /// <example>720p 1080p 720i 1080i PAL HDTV INTERLACED LETTERBOX</example>
        public string Format { get; set; }

        public string Codec { get; set; }

        public double? Aspect { get; set; }

        public int? Width { get; set; }

        public int? Height { get; set; }

        public long MovieId { get; set; }

        [Required]
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}
