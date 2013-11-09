using Frost.MediaInfo.Output.Properties;
using Frost.MediaInfo.Output.Properties.Codecs;
using Frost.MediaInfo.Output.Properties.Delay;
using Frost.MediaInfo.Output.Properties.Duration;
using Frost.MediaInfo.Output.Properties.Formats;

#pragma warning disable 1591

namespace Frost.MediaInfo.Output {
    public class MediaMenu : Media{

        public MediaMenu(MediaFile mediaInfo) : base(mediaInfo, StreamKind.Menu) {
            Codec = new Codec(this);
            Format = new Format(this);
            DurationInfo = new GeneralDurationInfo(this);
            LanguageInfo = new LanguageInfo(this);
            ServiceInfo = new ServiceInfo(this);
            DelayInfo = new DelayInfo(this, false);
        }

        public Codec Codec { get; private set; }
        /// <summary>Info about the Format used</summary>
        public Format Format { get; private set; }

        /// <summary>Play time of the stream in ms</summary>
        public string Duration { get { return this[""]; } }
        public GeneralDurationInfo DurationInfo { get; private set; }

        /// <summary>Delay fixed in the stream (relative) IN MS</summary>
        public string Delay { get { return this[""]; } }
        public DelayInfo DelayInfo { get; private set; }

        /// <summary>List of programs available</summary>
        public string List { get { return this[""]; } }
        /// <summary>List of programs available</summary>
        public string ListStreamKind { get { return this[""]; } }
        /// <summary>List of programs available</summary>
        public string ListStreamPos { get { return this[""]; } }
        /// <summary>List of programs available</summary>
        public string ListString { get { return this[""]; } }

        /// <summary>Name of this menu</summary>
        public string Title { get { return this[""]; } }

        public string Language { get { return this[""]; } }
        public LanguageInfo LanguageInfo { get; private set; }

        public ServiceInfo ServiceInfo { get; private set; }

        public string NetworkName { get { return this[""]; } }
        public string OriginalNetworkName { get { return this[""]; } }

        public string Countries { get { return this[""]; } }

        public string TimeZones { get { return this[""]; } }

        /// <summary>Used by third-party developers to know about the beginning of the chapters list, to be used by Get(Stream_Menu, x, Pos), where Pos is an Integer between Chapters_Pos_Begin and Chapters_Pos_End</summary>
        public string ChaptersPosBegin { get { return this[""]; } }

        /// <summary>Used by third-party developers to know about the end of the chapters list (this position excluded)</summary>
        public string ChaptersPosEnd { get { return this[""]; } }
    }
}