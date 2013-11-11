using Frost.SharpMediaInfo.Output.Properties;
using Frost.SharpMediaInfo.Output.Properties.Codecs;
using Frost.SharpMediaInfo.Output.Properties.Formats;

namespace Frost.SharpMediaInfo.Output {

    public class MediaImage : Media {

        public MediaImage(MediaFile mediaInfo) : base(mediaInfo, StreamKind.Image) {
            Format = new FormatWithWrapping(this);
            Codec = new Codec(this);
            WidthInfo = new SizeInfo(this, SizeType.Width);
            HeightInfo = new SizeInfo(this, SizeType.Height);
            StreamSizeInfo = new StreamSizeInfo(this);
            EncodingLibraryInfo = new EncodingLibraryInfo(this);
            LanguageInfo = new LanguageInfo(this);
            DisplayAspectRatioInfo = new Info(this, InfoType.DisplayAspectRatio);
            PixelAspectRatioInfo = new Info(this, InfoType.PixelAspectRatio);
        }

        /// <summary>Name of the track</summary>
        public string Title { get { return this["Title"]; } }

        /// <summary>Info about the Format used</summary>
        public FormatWithWrapping Format { get; private set; }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string MIME { get { return this["InternetMediaType"]; } }

        public Codec Codec { get; private set; }

        /// <summary>Width (aperture size if present) in pixel</summary>
        public string Width { get { return this["Width"]; } }
        public SizeInfo WidthInfo { get; private set; }

        /// <summary>Height (aperture size if present) in pixel</summary>
        public string Height { get { return this["Height"]; } }
        public SizeInfo HeightInfo { get; private set; }

        /// <summary>Pixel Aspect ratio</summary>
        public string PixelAspectRatio { get { return this["PixelAspectRatio"]; } }
        public Info PixelAspectRatioInfo { get; private set; }

        /// <summary>Display Aspect ratio</summary>
        public string DisplayAspectRatio { get { return this["DisplayAspectRatio"]; } }
        public Info DisplayAspectRatioInfo { get; private set; }

        public string ColorSpace { get { return this["ColorSpace"]; } }

        public string ChromaSubsampling { get { return this["ChromaSubsampling"]; } }

        public string Resolution { get { return this["Resolution"]; } }
        public string ResolutionString { get { return this["Resolution/String"]; } }

        public string BitDepth { get { return this["BitDepth"]; } }
        public string BitDepthString { get { return this["BitDepth/String"]; } }

        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionMode { get { return this["Compression_Mode"]; } }
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionModeString { get { return this["Compression_Mode/String"]; } }

        /// <summary>Current stream size divided by uncompressed stream size</summary>
        public string CompressionRatio { get { return this["Compression_Ratio"]; } }

        /// <summary>Streamsize in bytes</summary>
        public string StreamSize { get { return this["StreamSize"]; } }
        public StreamSizeInfo StreamSizeInfo { get; private set; }

        /// <summary>Software used to create the file</summary>
        public string EncodingLibrary { get { return this["Encoded_Library"]; } }
        public EncodingLibraryInfo EncodingLibraryInfo { get; private set; }

        /// <summary>Language (2-letter ISO 639-1 if exists, else 3-letter ISO 639-2, and with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn)</summary>
        public string Language { get { return this["Language"]; } }
        public LanguageInfo LanguageInfo { get; private set; }

        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string Default { get { return this["Default"]; } }
        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string DefaultString { get { return this["Default/String"]; } }

        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string Forced { get { return this["Forced"]; } }
        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string ForcedString { get { return this["Forced/String"]; } }

        public string Summary { get { return this["Summary"]; } }

        /// <summary>UTC time that the encoding of this item was completed began.</summary>
        public string EncodedDate { get { return this["Encoded_Date"]; } }

        /// <summary>UTC time that the tags were done for this item.</summary>
        public string TaggedDate { get { return this["Tagged_Date"]; } }

        public string Encryption { get { return this["Encryption"]; } }
    }
}