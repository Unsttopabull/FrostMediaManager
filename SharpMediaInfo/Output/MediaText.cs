using Frost.SharpMediaInfo.Output.Properties;
using Frost.SharpMediaInfo.Output.Properties.Codecs;
using Frost.SharpMediaInfo.Output.Properties.Delay;
using Frost.SharpMediaInfo.Output.Properties.Duration;
using Frost.SharpMediaInfo.Output.Properties.Formats;

#pragma warning disable 1591

namespace Frost.SharpMediaInfo.Output {

    public class MediaText : Media {

        internal MediaText(MediaFile mediaInfo) : base(mediaInfo, StreamKind.Text) {
            Format = new FormatWithWrapping(this);
            Codec = new TextCodec(this);
            FrameRateInfo = new FrameRateInfo(this);
            BitRateInfo = new BitRateInfo(this);
            DurationInfo = new ExtendedDurationInfo(this, false);
            SourceDurationInfo = new ExtendedDurationInfo(this, true);
            LanguageInfo = new LanguageInfo(this);
            DelayInfo = new DelayInfo(this, false);
            DelayInfo = new DelayInfo(this, true);
            EncodingLibraryInfo = new EncodingLibraryInfo(this);
            VideoDelayInfo = new VideoDelayInfo(this);
            Video0DelayInfo = new VideoDelayInfo(this);

            StreamSizeInfo = new StreamSizeInfo(this);
            SourceStreamSizeInfo = new StreamSizeInfo(this, StreamSizeType.SourceStreamSize);
            StreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedStreamSize);
            SourceStreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedSourceStreamSize);
        }

        /// <summary>Info about the Format used</summary>
        public FormatWithWrapping Format { get; private set; }
        public TextCodec Codec { get; private set; }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string MIME { get { return this["InternetMediaType"]; } }

        /// <summary>How this stream is muxed in the container</summary>
        public string MuxingMode { get { return this["MuxingMode"]; } }
        /// <summary>More info (text) about the muxing mode</summary>
        public string MuxingModeMoreInfo { get { return this["MuxingMode_MoreInfo"]; } }

        /// <summary>Play time of the stream in ms</summary>
        public string Duration { get { return this["Duration"]; } }
        public ExtendedDurationInfo DurationInfo { get; private set; }

        /// <summary>Source Play time of the stream</summary>
        public string SourceDuration { get { return this["Source_Duration"]; } }
        public ExtendedDurationInfo SourceDurationInfo { get; private set; }

        /// <summary>Bit rate in bps</summary>
        public string BitRate { get { return this["BitRate"]; } }
        public BitRateInfo BitRateInfo { get; private set; }

        public string Width { get { return this["Width"]; } }
        public string WidthString { get { return this["Width/String"]; } }

        public string Height { get { return this["Height"]; } }
        public string HeightString { get { return this["Height/String"]; } }

        /// <summary>Frames per second</summary>
        public string FrameRate { get { return this["FrameRate"]; } }
        public FrameRateInfo FrameRateInfo { get; private set; }

        /// <summary>Number of frames</summary>
        public string FrameCount { get { return this["FrameCount"]; } }
        /// <summary>Source Number of frames</summary>
        public string SourceFrameCount { get { return this["Source_FrameCount"]; } }

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

        /// <summary>Delay fixed in the stream (relative) IN MS</summary>
        public string Delay { get { return this["Delay"]; } }
        public DelayInfo DelayInfo { get; private set; }

        /// <summary>Delay fixed in the raw stream (relative) IN MS</summary>
        public string DelayOriginal { get { return this["Delay_Original"]; } }
        public DelayInfo DelayOriginalInfo { get; private set; }

        public string VideoDelay { get { return this["Video_Delay"]; } }
        public VideoDelayInfo VideoDelayInfo { get; private set; }

        public string Video0Delay { get { return this["Video0_Delay"]; } }
        public VideoDelayInfo Video0DelayInfo { get; private set; }

        /// <summary>Streamsize in bytes</summary>
        public string StreamSize { get { return this["StreamSize"]; } }
        public StreamSizeInfo StreamSizeInfo { get; private set; }

        /// <summary>Source Streamsize in bytes</summary>
        public string SourceStreamSize { get { return this["Source_StreamSize"]; } }
        public StreamSizeInfo SourceStreamSizeInfo { get; private set; }

        /// <summary>Encoded Streamsize in bytes</summary>
        public string StreamSizeEncoded { get { return this["StreamSize_Encoded"]; } }
        public StreamSizeInfo StreamSizeEncodedInfo { get; private set; }

        /// <summary>Source Encoded Streamsize in bytes</summary>
        public string SourceStreamSizeEncoded { get { return this["StreamSize_Encoded"]; } }
        public StreamSizeInfo SourceStreamSizeEncodedInfo { get; private set; }

        /// <summary>Name of the track</summary>
        public string Title { get { return this["Title"]; } }

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