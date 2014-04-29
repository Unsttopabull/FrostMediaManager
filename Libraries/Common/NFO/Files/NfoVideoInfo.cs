using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using Frost.Common.Models.Provider;

namespace Frost.Common.NFO.Files {

    /// <summary>Represents serialized information about a video stream in a movie</summary>
    [Serializable]
    public class NfoVideoInfo {

        /// <summary>Initializes a new instance of the <see cref="NfoVideoInfo"/> class.</summary>
        public NfoVideoInfo() {
        }

        /// <summary>Initializes a new instance of the <see cref="NfoVideoInfo"/> class.</summary>
        public NfoVideoInfo(IVideo video) {
            Codec = video.CodecId;

            if (video.Aspect.HasValue) {
                Aspect = (double) video.Aspect;
            }

            if (video.Width.HasValue) {
                Width = (int) video.Width;
            }

            if (video.Height.HasValue) {
                Height = (int) video.Height;
            }

            if (video.Duration.HasValue) {
                DurationInSecondsSpecified = true;
                DurationInSeconds = (int) Math.Round(video.Duration.Value / 1000.0);
            }

            if (video.Language != null && video.Language.ISO639 != null) {
                Language = !string.IsNullOrEmpty(video.Language.ISO639.Alpha3) 
                    ? video.Language.ISO639.Alpha3 
                    : video.Language.ISO639.Alpha2;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="NfoVideoInfo"/> class.</summary>
        /// <param name="codec">The codec in which the video is encoded.</param>
        /// <param name="aspect">The ratio between width and height (width / height).</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="durationInSeconds">Duration of the video in seconds.</param>
        /// <param name="language">The language of the video in short format.</param>
        /// <param name="longLanguage">The full name of the language.</param>
        public NfoVideoInfo(string codec, double aspect, int width, int height, int durationInSeconds, string language, string longLanguage) {
            Codec = codec;
            Aspect = aspect;
            Width = width;
            Height = height;
            DurationInSeconds = durationInSeconds;
            DurationInSecondsSpecified = true;
            Language = language;
            LongLanguage = longLanguage;
        }

        /// <summary>Initializes a new instance of the <see cref="NfoVideoInfo"/> class.</summary>
        /// <param name="codec">The codec in which the video is encoded.</param>
        /// <param name="aspect">The ratio between width and height (width / height).</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        /// <param name="durationInSeconds">Duration of the video in seconds.</param>
        public NfoVideoInfo(string codec, double aspect, int width, int height, int durationInSeconds) : this(codec, aspect, width, height) {
            DurationInSeconds = durationInSeconds;
            DurationInSecondsSpecified = true;
        }

        /// <summary>Initializes a new instance of the <see cref="NfoVideoInfo"/> class.</summary>
        /// <param name="codec">The codec in which the video is encoded.</param>
        /// <param name="aspect">The ratio between width and height (width / height).</param>
        /// <param name="width">The width of the video.</param>
        /// <param name="height">The height of the video.</param>
        public NfoVideoInfo(string codec, double aspect, int width, int height) : this(codec, aspect, width, height, 0, null, null) {
            DurationInSecondsSpecified = false;
        }

        /// <summary>Gets or sets the codec in which the video is encoded.</summary>
        /// <value>The codec in which the video is encoded.</value>
        /// <example>XVID, DIVX, MPEG4</example>
        [XmlElement("codec", Form = XmlSchemaForm.Unqualified)]
        public string Codec { get; set; }

        /// <summary>The ratio between width and height (width / height)</summary>
        /// <example>1.333</example>
        [XmlElement("aspect", Form = XmlSchemaForm.Unqualified)]
        public double Aspect { get; set; }

        /// <summary>Gets or sets the width of the video.</summary>
        /// <value>The width of the video.</value>
        [XmlElement("width", Form = XmlSchemaForm.Unqualified)]
        public int Width { get; set; }

        /// <summary>Gets or sets the height of the video.</summary>
        /// <value>The height of the video.</value>
        [XmlElement("height", Form = XmlSchemaForm.Unqualified)]
        public int Height { get; set; }

        /// <summary>Gets or sets the duration of the video in seconds.</summary>
        /// <remarks>If 0 there is no iformation about duration</remarks>
        /// <value>The duration of the video in seconds.</value>
        [XmlElement("durationinseconds", Form = XmlSchemaForm.Unqualified)]
        public int DurationInSeconds { get; set; }

        /// <summary>Gets or sets a value indicating whether <see cref="NfoVideoInfo.DurationInSeconds"/> should be serialized.</summary>
        /// <value>Is <c>true</c> if the <see cref="NfoVideoInfo.DurationInSeconds"/> should be serialized; otherwise, <c>false</c>.</value>
        [XmlIgnore]
        public bool DurationInSecondsSpecified { get; set; }

        /// <summary>Gets or sets the language this video is in.</summary>
        /// <value>The language is in.</value>
        [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>Gets or sets the longer language descriptor this video is in.</summary>
        /// <value>the longer language descriptor this video is in.</value>
        [XmlElement("longlanguage", Form = XmlSchemaForm.Unqualified)]
        public string LongLanguage { get; set; }

    }

}
