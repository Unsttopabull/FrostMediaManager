using Frost.SharpMediaInfo.Output.Properties;
using Frost.SharpMediaInfo.Output.Properties.BitRate;
using Frost.SharpMediaInfo.Output.Properties.Codecs;
using Frost.SharpMediaInfo.Output.Properties.Delay;
using Frost.SharpMediaInfo.Output.Properties.Duration;
using Frost.SharpMediaInfo.Output.Properties.Formats;
using Frost.SharpMediaInfo.Output.Properties.FrameRate;

#pragma warning disable 1591

namespace Frost.SharpMediaInfo.Output {

    public class MediaVideo : Media {

        internal MediaVideo(MediaFile mi) : base(mi, StreamKind.Video) {
            Format = new VideoFormat(this);
            Codec = new VideoCodec(this);
            WidthInfo = new SizeInfo(this, SizeType.Width);
            HeightInfo = new SizeInfo(this, SizeType.Height);
            FrameRateInfo = new VideoFrameRateInfo(this);
            BitRateInfo = new BitRateInfo(this);
            SourceDurationInfo = new ExtendedDurationInfo(this, true);
            DurationInfo = new ExtendedDurationInfo(this, false);
            LanguageInfo = new LanguageInfo(this);
            DelayInfo = new DelayInfo(this, false);
            DelayOriginalInfo = new DelayInfo(this, true);
            EncodingLibraryInfo = new EncodingLibraryInfo(this);
            ScanTypeInfo = new Info(this, InfoType.ScanType);
            ScanOrderInfo = new Info(this, InfoType.ScanOrder);
            DisplayAspectRatioInfo = new Info(this, InfoType.DisplayAspectRatio);
            PixelAspectRatioInfo = new Info(this, InfoType.PixelAspectRatio);

            StreamSizeInfo = new StreamSizeInfo(this);
            SourceStreamSizeInfo = new StreamSizeInfo(this, StreamSizeType.SourceStreamSize);
            StreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedStreamSize);
            SourceStreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedSourceStreamSize);            
        }

        /// <summary>Info about the Format used</summary>
        public VideoFormat Format { get; private set; }
        public VideoCodec Codec { get; private set; }

        /// <summary>Multiview, profile of the base stream</summary>
        public string MultiViewBaseProfile { get { return this["MultiView_BaseProfile"]; } }
        /// <summary>Multiview, count of views</summary>
        public string MultiViewCount { get { return this["MultiView_Count"]; } }
        /// <summary>Multiview, how views are muxed in the container in case of it is not muxing in the stream</summary>
        public string MultiViewLayout { get { return this["MultiView_Layout"]; } }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string MIME { get { return this["InternetMediaType"]; } }

        /// <summary>How this file is muxed in the container</summary>
        public string MuxingMode { get { return this["MuxingMode"]; } }

        /// <summary>Play time of the stream in ms</summary>
        public string Duration { get { return this["Duration"]; } }
        public ExtendedDurationInfo DurationInfo { get; private set; }

        /// <summary>Source Play time of the stream</summary>
        public string SourceDuration { get { return this["Source_Duration"]; } }
        public ExtendedDurationInfo SourceDurationInfo { get; private set; }

        /// <summary>Bit rate in bps</summary>
        public string BitRate { get { return this["BitRate"]; } }
        public BitRateInfo BitRateInfo { get; private set; }

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

        /// <summary>Active Format Description (AFD value)</summary>
        public string ActiveFormatDescription { get { return this["ActiveFormatDescription"]; } }
        /// <summary>Active Format Description (text)</summary>
        public string ActiveFormatDescriptionString { get { return this["ActiveFormatDescription/String"]; } }
        /// <summary>Active Format Description (AFD value) muxing mode (Ancillary or Raw stream)</summary>
        public string ActiveFormatDescriptionMuxingMode { get { return this["ActiveFormatDescription_MuxingMode"]; } }
        
        /// <summary>Rotation</summary>
        public string Rotation { get { return this["Rotation"]; } }
        /// <summary>Rotation (if not horizontal)</summary>
        public string RotationString { get { return this["Rotation/String"]; } }

        /// <summary>Frames per second</summary>
        public string FrameRate { get { return this["FrameRate"]; } }
        public VideoFrameRateInfo FrameRateInfo { get; private set; }

        /// <summary>Number of frames</summary>
        public string FrameCount { get { return this["FrameCount"]; } }
        /// <summary>Source Number of frames</summary>
        public string SourceFrameCount { get { return this["Source_FrameCount"]; } }
        
        /// <summary>NTSC or PAL</summary>
        public string Standard { get { return this["Standard"]; } }
        
        public string Resolution { get { return this["Resolution"]; } }
        public string ResolutionString { get { return this["Resolution/String"]; } }
        
        public string Colorimetry { get { return this["Colorimetry"]; } }
        
        public string ColorSpace { get { return this["ColorSpace"]; } }
        
        public string ChromaSubsampling { get { return this["ChromaSubsampling"]; } }
        
        /// <summary>16/24/32</summary>
        public string BitDepth { get { return this["BitDepth"]; } }
        /// <summary>16/24/32 bits</summary>
        public string BitDepthString { get { return this["BitDepth/String"]; } }
        
        public string ScanType { get { return this["ScanType"]; } }
        public Info ScanTypeInfo { get; private set; }
        
        public string ScanOrder { get { return this["ScanOrder"]; } }
        public Info ScanOrderInfo { get; private set; }

        public string Interlacement { get { return this["Interlacement"]; } }
        public string InterlacementString { get { return this["Interlacement/String"]; } }
        
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionMode { get { return this["Compression_Mode"]; } }
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionModeString { get { return this["Compression_Mode/String"]; } }

        /// <summary>Current stream size divided by uncompressed stream size</summary>
        public string CompressionRatio { get { return this["Compression_Ratio"]; } }

        /// <summary>bits/(Pixel*Frame) (like Gordian Knot)</summary>
        public string BitsPerPixelFrame { get{ return this["Bits-(Pixel*Frame)"]; } }

        /// <summary>Delay fixed in the stream (relative) IN MS</summary>
        public string Delay { get { return this["Delay"]; } }
        public DelayInfo DelayInfo { get; private set; }
        
        /// <summary>Delay fixed in the raw stream (relative) IN MS</summary>
        public string DelayOriginal { get { return this["Delay_Original"]; } }
        public DelayInfo DelayOriginalInfo { get; private set; }
        
        /// <summary>TimeStamp fixed in the stream (relative) IN MS</summary>
        public string TimeStampFirstFrame { get { return this["TimeStamp_FirstFrame"]; } }
        /// <summary>TimeStamp with measurement</summary>
        public string TimeStampFirstFrameString { get { return this["TimeStamp_FirstFrame/String"]; } }
        /// <summary>TimeStamp with measurement</summary>
        public string TimeStampFirstFrameString1 { get { return this["TimeStamp_FirstFrame/String1"]; } }
        /// <summary>TimeStamp with measurement</summary>
        public string TimeStampFirstFrameString2 { get { return this["TimeStamp_FirstFrame/String2"]; } }
        /// <summary>TimeStamp in format : HH:MM:SS.MMM</summary>
        public string TimeStampFirstFrameString3 { get { return this["TimeStamp_FirstFrame/String3"]; } }
        
        /// <summary>Time code in HH:MM:SS:FF (HH:MM:SS</summary>
        public string TimeCodeFirstFrame { get { return this["TimeCode_FirstFrame"]; } }
        /// <summary>Time code settings</summary>
        public string TimeCodeSettings { get { return this["TimeCode_Settings"]; } }
        /// <summary>Time code source (Container, Stream, SystemScheme1, SDTI, ANC...)</summary>
        public string TimeCodeSource { get { return this["TimeCode_Source"]; } }

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
        public string SourceStreamSizeEncoded { get { return this["Source_StreamSize_Encoded"]; } }
        public StreamSizeInfo SourceStreamSizeEncodedInfo { get; private set; }
        
        /// <summary>Name of the track</summary>
        public string Alignment { get { return this["Alignment"]; } }
        public string AlignmentString { get { return this["Alignment/String"]; } }
        
        /// <summary>Name of the track</summary>
        public string Title { get { return this["Title"]; } }
        
        /// <summary>Software. Identifies the name of the software package used to create the file, such as Microsoft WaveEdit.</summary>
        public string EncodingApplication { get { return this["Encoded_Application"]; } }
        /// <summary>Software. Identifies the name of the software package used to create the file, such as Microsoft WaveEdit.</summary>
        public string EncodingApplicationUrl { get { return this["Encoded_Application/Url"]; } }

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
        
        /// <summary>UTC time that the encoding of this item was completed began.</summary>
        public string EncodedDate { get { return this["Encoded_Date"]; } }

        /// <summary>UTC time that the tags were done for this item.</summary>
        public string TaggedDate { get { return this["Tagged_Date"]; } }
        
        public string Encryption { get { return this["Encryption"]; } }
        
        /// <summary>Defines the size of the buffer needed to decode the sequence.</summary>
        public string BufferSize { get { return this["BufferSize"]; } }
    }
}