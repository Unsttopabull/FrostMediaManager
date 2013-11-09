using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frost.Common.Models.DB.XBMC.StreamDetails {

    /// <summary>Represents information about a video stream in a file.</summary>
    public class XbmcVideoDetails : XbmcStreamDetails, IEquatable<XbmcVideoDetails> {

        /// <summary>Initializes a new instance of the <see cref="XbmcVideoDetails"/> class.</summary>
        /// <param name="codec">The video codec.</param>
        /// <param name="aspect">Aspect ratio. The ratio between width and height (width / height)</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="duration">The duration of the video in seconds.</param>
        public XbmcVideoDetails(string codec, double? aspect, long? width, long? height, long? duration) : this(new XbmcFile(), codec, aspect, width, height, duration) {
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcVideoDetails"/> class.</summary>
        /// <param name="file">The file that contains this video stream.</param>
        /// <param name="codec">The video codec.</param>
        /// <param name="aspect">Aspect ratio. The ratio between width and height (width / height)</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="duration">The duration of the video in seconds.</param>
        public XbmcVideoDetails(XbmcFile file, string codec, double? aspect, long? width, long? height, long? duration) : base(file) {
            Codec = codec;
            Aspect = aspect;
            Width = width;
            Height = height;
            Duration = duration;
        }

        /// <summary>Gets or sets the video codec.</summary>
        /// <value>The video codec.</value>
        /// <example>\eg{ <c>XVID, DIVX, MPEG4</c>}</example>
        [Column("strVideoCodec")]
        public string Codec { get; set; }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <example>\eg{ <c>1.333</c>}</example>
        [Column("fVideoAspect")]
        public double? Aspect { get; set; }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        [Column("iVideoWidth")]
        public long? Width { get; set; }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        [Column("iVideoHeight")]
        public long? Height { get; set; }

        /// <summary>Gets or sets the duration of the video in seconds.</summary>
        /// <value>The duration of the video in seconds.</value>
        [Column("iVideoDuration")]
        public long? Duration { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcVideoDetails other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Codec == other.Codec &&
                   Aspect == other.Aspect &&
                   Width == other.Width &&
                   Height == other.Height &&
                   Duration == other.Duration;
        }

    }

}
