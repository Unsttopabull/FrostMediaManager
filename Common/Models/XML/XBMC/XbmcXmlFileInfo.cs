using System;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Models.XML.XBMC {
    public enum MediaType {
        Audio,
        Video,
        Subtitles
    }

    /// <remarks/>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class XbmcXmlFileInfo {
        private const string SEPARATOR = " / ";

        public XbmcXmlFileInfo() {
            StreamDetails = new StreamDetails();
        }

        [XmlElement("streamdetails", Form = XmlSchemaForm.Unqualified)]
        public StreamDetails StreamDetails { get; set; }

        [XmlIgnore]
        public XbmcXmlVideoInfo[] Videos {
            get { return StreamDetails.Video; }
            set { StreamDetails.Video = value; }
        }

        [XmlIgnore]
        public XbmcXmlAudioInfo[] Audios {
            get { return StreamDetails.Audio; }
            set { StreamDetails.Audio = value; }
        }

        [XmlIgnore]
        public XbmcXmlSubtitleInfo[] Subtitles {
            get { return StreamDetails.Subtitle; }
            set { StreamDetails.Subtitle = value; }
        }

        public string[] GetSubtitleLanguages() {
            if (Subtitles != null) {
                return Subtitles.Where(s => s.Language != null)
                                .Select(s => s.Language)
                                .ToArray();
            }
            return null;
        }

        public string GetSubtitleLanguagesFormatted() {
            string[] langs = GetSubtitleLanguages();
            return langs != null
                ? string.Join(SEPARATOR, langs)
                : null;
        }

        public bool InfoExists(MediaType stream) {
            switch (stream) {
                case MediaType.Audio:
                    return StreamDetails.Audio != null && StreamDetails.Audio.Length > 0;
                case MediaType.Video:
                    return StreamDetails.Video != null && StreamDetails.Video.Length > 0;
                case MediaType.Subtitles:
                    return StreamDetails.Subtitle != null && StreamDetails.Subtitle.Length > 0;
                default:
                    throw new ArgumentOutOfRangeException("stream");
            }
        }
    }
}