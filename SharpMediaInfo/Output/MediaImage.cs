using Frost.MediaInfo.Output.Properties;
using Frost.MediaInfo.Output.Properties.Codecs;
using Frost.MediaInfo.Output.Properties.Formats;

namespace Frost.MediaInfo.Output {

    public class MediaImage : Media {

        public MediaImage(MediaFile mediaInfo) : base(mediaInfo, StreamKind.Image) {
            Format = new FormatWithWrapping(this);
            Codec = new Codec(this);
            WidthInfo = new SizeInfo(this);
            HeightInfo = new SizeInfo(this);
            StreamSizeInfo = new StreamSizeInfo(this);
            EncodedLibraryInfo = new EncodedLibraryInfo(this);
            LanguageInfo = new LanguageInfo(this);
            DisplayAspectRatioInfo = new Properties.Info(this, InfoType.DisplayAspectRatio);
            PixelAspectRatioInfo = new Properties.Info(this, InfoType.PixelAspectRatio);
        }

        /// <summary>Name of the track</summary>
        public string Title { get { return this[""]; } }

        /// <summary>Info about the Format used</summary>
        public FormatWithWrapping Format { get; private set; }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string InternetMediaType { get { return this[""]; } }

        public Codec Codec { get; private set; }

        /// <summary>Width (aperture size if present) in pixel</summary>
        public string Width { get { return this[""]; } }
        public SizeInfo WidthInfo { get; private set; }

        /// <summary>Height (aperture size if present) in pixel</summary>
        public string Height { get { return this[""]; } }
        public SizeInfo HeightInfo { get; private set; }

        /// <summary>Pixel Aspect ratio</summary>
        public string PixelAspectRatio { get { return this[""]; } }
        public Properties.Info PixelAspectRatioInfo { get; private set; }

        /// <summary>Display Aspect ratio</summary>
        public string DisplayAspectRatio { get { return this[""]; } }
        public Properties.Info DisplayAspectRatioInfo { get; private set; }

        public string ColorSpace { get { return this[""]; } }

        public string ChromaSubsampling { get { return this[""]; } }

        public string Resolution { get { return this[""]; } }
        public string ResolutionString { get { return this[""]; } }

        public string BitDepth { get { return this[""]; } }
        public string BitDepthString { get { return this[""]; } }

        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionMode { get { return this[""]; } }
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionModeString { get { return this[""]; } }

        /// <summary>Current stream size divided by uncompressed stream size</summary>
        public string CompressionRatio { get { return this[""]; } }

        /// <summary>Streamsize in bytes</summary>
        public string StreamSize { get { return this[""]; } }
        public StreamSizeInfo StreamSizeInfo { get; private set; }

        /// <summary>Software used to create the file</summary>
        public string EncodedLibrary { get { return this[""]; } }
        public EncodedLibraryInfo EncodedLibraryInfo { get; private set; }

        /// <summary>Language (2-letter ISO 639-1 if exists, else 3-letter ISO 639-2, and with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn)</summary>
        public string Language { get { return this[""]; } }
        public LanguageInfo LanguageInfo { get; private set; }

        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string Default { get { return this[""]; } }
        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string DefaultString { get { return this[""]; } }

        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string Forced { get { return this[""]; } }
        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string ForcedString { get { return this[""]; } }

        public string Summary { get { return this[""]; } }

        /// <summary>UTC time that the encoding of this item was completed began.</summary>
        public string EncodedDate { get { return this[""]; } }

        /// <summary>UTC time that the tags were done for this item.</summary>
        public string TaggedDate { get { return this[""]; } }

        public string Encryption { get { return this[""]; } }
    }
}