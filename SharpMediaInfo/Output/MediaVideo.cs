using SharpMediaInfo.Output.Properties;
using SharpMediaInfo.Output.Properties.Codecs;
using SharpMediaInfo.Output.Properties.Delay;
using SharpMediaInfo.Output.Properties.Duration;
using SharpMediaInfo.Output.Properties.Formats;

#pragma warning disable 1591

namespace SharpMediaInfo.Output {
    using Info = Properties.Info;

    public class MediaVideo : Media {

        internal MediaVideo(MediaFile mi) : base(mi, StreamKind.Video) {
            Format = new VideoFormat(this);
            Codec = new VideoCodec(this);
            WidthInfo = new SizeInfo(this);
            HeightInfo = new SizeInfo(this);
            FrameRateInfo = new FrameRateInfo(this);
            BitRateInfo = new BitRateInfo(this);
            SourceDurationInfo = new ExtendedDurationInfo(this, true);
            DurationInfo = new ExtendedDurationInfo(this, false);
            LanguageInfo = new LanguageInfo(this);
            DelayInfo = new DelayInfo(this, false);
            DelayOriginalInfo = new DelayInfo(this, true);
            EncodedLibraryInfo = new EncodedLibraryInfo(this);
            ScanTypeInfo = new Info(this, InfoType.ScanType);
            ScanOrderInfo = new Info(this, InfoType.ScanOrder);
            DisplayAspectRatioInfo = new Info(this, InfoType.DisplayAspectRatio);
            PixelAspectRatioInfo = new Info(this, InfoType.PixelAspectRatio);

            StreamSizeInfo = new StreamSizeInfo(this, StreamSizeType.StreamSize);
            SourceStreamSizeInfo = new StreamSizeInfo(this, StreamSizeType.SourceStreamSize);
            StreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedStreamSize);
            SourceStreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedSourceStreamSize);            
        }

        /// <summary>Info about the Format used</summary>
        public VideoFormat Format { get; private set; }
        public VideoCodec Codec { get; private set; }

        /// <summary>Multiview, profile of the base stream</summary>
        public string MultiViewBaseProfile { get { return this[""]; } }
        /// <summary>Multiview, count of views</summary>
        public string MultiViewCount { get { return this[""]; } }
        /// <summary>Multiview, how views are muxed in the container in case of it is not muxing in the stream</summary>
        public string MultiViewLayout { get { return this[""]; } }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string InternetMediaType { get { return this[""]; } }

        /// <summary>How this file is muxed in the container</summary>
        public string MuxingMode { get { return this[""]; } }

        /// <summary>Play time of the stream in ms</summary>
        public string Duration { get { return this[""]; } }
        public ExtendedDurationInfo DurationInfo { get; private set; }

        /// <summary>Source Play time of the stream</summary>
        public string SourceDuration { get { return this[""]; } }
        public ExtendedDurationInfo SourceDurationInfo { get; private set; }

        /// <summary>Bit rate in bps</summary>
        public string BitRate { get { return this[""]; } }
        public BitRateInfo BitRateInfo { get; private set; }

        /// <summary>Width (aperture size if present) in pixel</summary>
        public string Width { get { return this[""]; } }
        public SizeInfo WidthInfo { get; private set; }

        /// <summary>Height (aperture size if present) in pixel</summary>
        public string Height { get { return this[""]; } }
        public SizeInfo HeightInfo { get; private set; }

        /// <summary>Pixel Aspect ratio</summary>
        public string PixelAspectRatio { get { return this[""]; } }
        public Info PixelAspectRatioInfo { get; private set; }

        /// <summary>Display Aspect ratio</summary>
        public string DisplayAspectRatio { get { return this[""]; } }
        public Info DisplayAspectRatioInfo { get; private set; }

        /// <summary>Active Format Description (AFD value)</summary>
        public string ActiveFormatDescription { get { return this[""]; } }
        /// <summary>Active Format Description (text)</summary>
        public string ActiveFormatDescriptionString { get { return this[""]; } }
        /// <summary>Active Format Description (AFD value) muxing mode (Ancillary or Raw stream)</summary>
        public string ActiveFormatDescriptionMuxingMode { get { return this[""]; } }
        
        /// <summary>Rotation</summary>
        public string Rotation { get { return this[""]; } }
        /// <summary>Rotation (if not horizontal)</summary>
        public string RotationString { get { return this[""]; } }

        /// <summary>Frames per second</summary>
        public string FrameRate { get { return this[""]; } }
        public FrameRateInfo FrameRateInfo { get; private set; }

        /// <summary>Number of frames</summary>
        public string FrameCount { get { return this[""]; } }
        /// <summary>Source Number of frames</summary>
        public string SourceFrameCount { get { return this[""]; } }
        
        /// <summary>NTSC or PAL</summary>
        public string Standard { get { return this[""]; } }
        
        public string Resolution { get { return this[""]; } }
        public string ResolutionString { get { return this[""]; } }
        
        public string Colorimetry { get { return this[""]; } }
        
        public string ColorSpace { get { return this[""]; } }
        
        public string ChromaSubsampling { get { return this[""]; } }
        
        /// <summary>16/24/32</summary>
        public string BitDepth { get { return this[""]; } }
        /// <summary>16/24/32 bits</summary>
        public string BitDepthString { get { return this[""]; } }
        
        public string ScanType { get { return this[""]; } }
        public Info ScanTypeInfo { get; private set; }
        
        public string ScanOrder { get { return this[""]; } }
        public Info ScanOrderInfo { get; private set; }

        public string Interlacement { get { return this[""]; } }
        public string InterlacementString { get { return this[""]; } }
        
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionMode { get { return this[""]; } }
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionModeString { get { return this[""]; } }

        /// <summary>Current stream size divided by uncompressed stream size</summary>
        public string CompressionRatio { get { return this[""]; } }

        /// <summary>bits/(Pixel*Frame) (like Gordian Knot)</summary>
        public string BitsPerPixelFrame { get{ return this[""]; } }

        /// <summary>Delay fixed in the stream (relative) IN MS</summary>
        public string Delay { get { return this[""]; } }
        public DelayInfo DelayInfo { get; private set; }
        
        /// <summary>Delay fixed in the raw stream (relative) IN MS</summary>
        public string DelayOriginal { get { return this[""]; } }
        public DelayInfo DelayOriginalInfo { get; private set; }
        
        /// <summary>TimeStamp fixed in the stream (relative) IN MS</summary>
        public string TimeStampFirstFrame { get { return this[""]; } }
        /// <summary>TimeStamp with measurement</summary>
        public string TimeStampFirstFrameString { get { return this[""]; } }
        /// <summary>TimeStamp with measurement</summary>
        public string TimeStampFirstFrameString1 { get { return this[""]; } }
        /// <summary>TimeStamp with measurement</summary>
        public string TimeStampFirstFrameString2 { get { return this[""]; } }
        /// <summary>TimeStamp in format : HH:MM:SS.MMM</summary>
        public string TimeStampFirstFrameString3 { get { return this[""]; } }
        
        /// <summary>Time code in HH:MM:SS:FF (HH:MM:SS</summary>
        public string TimeCodeFirstFrame { get { return this[""]; } }
        /// <summary>Time code settings</summary>
        public string TimeCodeSettings { get { return this[""]; } }
        /// <summary>Time code source (Container, Stream, SystemScheme1, SDTI, ANC...)</summary>
        public string TimeCodeSource { get { return this[""]; } }

        /// <summary>Streamsize in bytes</summary>
        public string StreamSize { get { return this[""]; } }
        public StreamSizeInfo StreamSizeInfo { get; private set; }

        /// <summary>Source Streamsize in bytes</summary>
        public string SourceStreamSize { get { return this[""]; } }
        public StreamSizeInfo SourceStreamSizeInfo { get; private set; }

        /// <summary>Encoded Streamsize in bytes</summary>
        public string StreamSizeEncoded { get { return this[""]; } }
        public StreamSizeInfo StreamSizeEncodedInfo { get; private set; }

        /// <summary>Source Encoded Streamsize in bytes</summary>
        public string SourceStreamSizeEncoded { get { return this[""]; } }
        public StreamSizeInfo SourceStreamSizeEncodedInfo { get; private set; }
        
        /// <summary>Name of the track</summary>
        public string Alignment { get { return this[""]; } }
        public string AlignmentString { get { return this[""]; } }
        
        /// <summary>Name of the track</summary>
        public string Title { get { return this[""]; } }
        
        /// <summary>Software. Identifies the name of the software package used to create the file, such as Microsoft WaveEdit.</summary>
        public string EncodedApplication { get { return this[""]; } }
        /// <summary>Software. Identifies the name of the software package used to create the file, such as Microsoft WaveEdit.</summary>
        public string EncodedApplicationUrl { get { return this[""]; } }

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
        
        /// <summary>UTC time that the encoding of this item was completed began.</summary>
        public string EncodedDate { get { return this[""]; } }

        /// <summary>UTC time that the tags were done for this item.</summary>
        public string TaggedDate { get { return this[""]; } }
        
        public string Encryption { get { return this[""]; } }
        
        /// <summary>Defines the size of the buffer needed to decode the sequence.</summary>
        public string BufferSize { get { return this[""]; } }
    }
}