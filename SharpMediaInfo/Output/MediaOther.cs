using Frost.MediaInfo.Output.Properties;
using Frost.MediaInfo.Output.Properties.Duration;
using Frost.MediaInfo.Output.Properties.Formats;

#pragma warning disable 1591

namespace Frost.MediaInfo.Output {
    public class MediaOther : Media {

        public MediaOther(MediaFile mediaInfo) : base(mediaInfo, StreamKind.Other) {
            Format = new Format(this);
            DurationInfo = new GeneralDurationInfo(this);
            LanguageInfo = new LanguageInfo(this);
        }

        /// <summary>Info about the Format used</summary>
        public Format Format { get; private set; }

        public string Type { get { return this[""]; } }

        /// <summary>How this file is muxed in the container</summary>
        public string MuxingMode { get { return this[""]; } }

        /// <summary>Play time of the stream in ms</summary>
        public string Duration { get { return this[""]; } }
        public GeneralDurationInfo DurationInfo { get; private set; }

        /// <summary>Frames per second</summary>
        public string FrameRate { get { return this[""]; } }
        /// <summary>Frames per second (with measurement)</summary>
        public string FrameRateString { get { return this[""]; } }

        /// <summary>Number of frames</summary>
        public string FrameCount { get { return this[""]; } }

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

        /// <summary>Name of the track</summary>
        public string Title { get { return this[""]; } }

        /// <summary>Language (2-letter ISO 639-1 if exists, else 3-letter ISO 639-2, and with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn)</summary>
        public string Language { get { return this[""]; } }
        public LanguageInfo LanguageInfo { get; private set; }
    }
}