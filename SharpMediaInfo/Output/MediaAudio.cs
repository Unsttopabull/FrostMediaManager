using Frost.MediaInfo.Output.Properties;
using Frost.MediaInfo.Output.Properties.Codecs;
using Frost.MediaInfo.Output.Properties.Delay;
using Frost.MediaInfo.Output.Properties.Duration;
using Frost.MediaInfo.Output.Properties.Formats;

#pragma warning disable 1591

namespace Frost.MediaInfo.Output {
    public class MediaAudio : Media {

        public MediaAudio(MediaFile mediaInfo) : base(mediaInfo, StreamKind.Audio) {
            Format = new AudioFormat(this);
            Codec = new AudioCodec(this);
            BitRateInfo = new BitRateInfo(this);
            DurationInfo = new ExtendedDurationInfo(this, false);
            SourceDurationInfo = new ExtendedDurationInfo(this, true);
            LanguageInfo = new LanguageInfo(this);
            DelayInfo = new DelayInfo(this, false);
            DelayOriginalInfo = new DelayInfo(this, true);
            EncodedLibraryInfo = new EncodedLibraryInfo(this);
            VideoDelayInfo = new VideoDelayInfo(this);
            Video0DelayInfo = new VideoDelayInfo(this);
            Interleave = new Interleave(this);
            ChannelInfo = new ChannelInfo(this);

            StreamSizeInfo = new StreamSizeInfo(this, StreamSizeType.StreamSize);
            SourceStreamSizeInfo = new StreamSizeInfo(this, StreamSizeType.SourceStreamSize);
            StreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedStreamSize);
            SourceStreamSizeEncodedInfo = new StreamSizeInfo(this, StreamSizeType.EncodedSourceStreamSize);
        }

        /// <summary>Format used</summary>
        public AudioFormat Format { get; private set; }

        /// <summary>Codec used</summary>
        public AudioCodec Codec { get; private set; }

        /// <summary>Internet Media Type (aka MIME Type, Content-Type)</summary>
        public string InternetMediaType { get { return this[""]; } }

        /// <summary>How this stream is muxed in the container</summary>
        public string MuxingMode { get { return this[""]; } }
        /// <summary>More info (text) about the muxing mode</summary>
        public string MuxingModeMoreInfo { get { return this[""]; } }

        /// <summary>Play time of the stream</summary>
        public string Duration { get { return this[""]; } }
        public ExtendedDurationInfo DurationInfo { get; private set; }

        /// <summary>Source Play time of the stream</summary>
        public string SourceDuration { get { return this[""]; } }
        public ExtendedDurationInfo SourceDurationInfo { get; private set; }

        /// <summary>Bit rate in bps</summary>
        public string BitRate { get { return this[""]; } }
        public BitRateInfo BitRateInfo { get; private set; }

        /// <summary>Number of channels</summary>
        public string Channels { get { return this[""]; } }
        public ChannelInfo ChannelInfo { get; private set; }

        /// <summary>Sampling rate</summary>
        public string SamplingRate { get { return this[""]; } }
        /// <summary>Sampling rate in KHz</summary>
        public string SamplingRateString { get { return this[""]; } }
        /// <summary>Sample count (based on sampling rate)</summary>
        public string SamplingCount { get { return this[""]; } }
        /// <summary>Source Sample count (based on sampling rate)</summary>
        public string SourceSamplingCount { get { return this[""]; } }

        /// <summary>Frames per second</summary>
        public string FrameRate { get { return this[""]; } }
        /// <summary>Frames per second (with measurement)</summary>
        public string FrameRateString { get { return this[""]; } }

        /// <summary>Frame count (a frame contains a count of samples depends of the format)</summary>
        public string FrameCount { get { return this[""]; } }
        /// <summary>Source Frame count (a frame contains a count of samples depends of the format)</summary>
        public string SourceFrameCount { get { return this[""]; } }

        public string Resolution { get { return this[""]; } }
        public string ResolutionString { get { return this[""]; } }

        /// <summary>Resolution in bits (8, 16, 20, 24)</summary>
        public string BitDepth { get { return this[""]; } }
        /// <summary>Resolution in bits (8, 16, 20, 24)</summary>
        public string BitDepthString { get { return this[""]; } }

        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionMode { get { return this[""]; } }
        /// <summary>Compression mode (Lossy or Lossless)</summary>
        public string CompressionModeString { get { return this[""]; } }
        /// <summary>Current stream size divided by uncompressed stream size</summary>
        public string CompressionRatio { get { return this[""]; } }

        /// <summary>Delay fixed in the stream (relative) IN MS</summary>
        public string Delay { get { return this[""]; } }
        public DelayInfo DelayInfo { get; private set; }

        /// <summary>Delay fixed in the raw stream (relative) IN MS</summary>
        public string DelayOriginal { get { return this[""]; } }
        public DelayInfo DelayOriginalInfo { get; private set; }

        /// <summary>Delay fixed in the stream (absolute / video)</summary>
        public string VideoDelay { get { return this[""]; } }
        public VideoDelayInfo VideoDelayInfo { get; private set; }

        public string Video0Delay { get { return this[""]; } }
        public VideoDelayInfo Video0DelayInfo { get; private set; }

        /// <summary>The gain to apply to reach 89dB SPL on playback</summary>
        public string ReplayGainGain { get { return this[""]; } }
        public string ReplayGainGainString { get { return this[""]; } }
        /// <summary>The maximum absolute peak value of the item</summary>
        public string ReplayGainPeak { get { return this[""]; } }

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

        /// <summary>How this stream file is aligned in the container</summary>
        public string Alignment { get { return this[""]; } }
        /// <summary>Where this stream file is aligned in the container</summary>
        public string AlignmentString { get { return this[""]; } }

        public Interleave Interleave { get; private set; }

        /// <summary>Name of the track</summary>
        public string Title { get { return this[""]; } }

        /// <summary>Software used to create the file</summary>
        public string EncodedLibrary { get { return this[""]; } }
        public EncodedLibraryInfo EncodedLibraryInfo { get; private set; }

        /// <summary>Language (2-letter ISO 639-1 if exists, else 3-letter ISO 639-2, and with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn)</summary>
        public string Language { get { return this[""]; } }
        public LanguageInfo LanguageInfo { get; private set; }

        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string Default { get { return this[""]; } }
        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string DefaultString { get { return this["DefaultString"]; } }

        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string Forced { get { return this[""]; } }
        /// <summary>Set if that track should be used if no language found matches the user preference.</summary>
        public string ForcedString { get { return this["ForcedString"]; } }

        /// <summary>UTC time that the encoding of this item was completed began.</summary>
        public string EncodedDate { get { return this[""]; } }

        /// <summary>UTC time that the tags were done for this item.</summary>
        public string TaggedDate { get { return this[""]; } }

        public string Encryption { get { return this[""]; } }
    }
}