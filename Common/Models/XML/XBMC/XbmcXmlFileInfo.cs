using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Frost.Common.Models.XML.XBMC {

    /// <summary>Represents movie's file information ready to be serialized</summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public class XbmcXmlFileInfo {
        private const string SEPARATOR = " / ";

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlFileInfo"/> class.</summary>
        public XbmcXmlFileInfo() {
            StreamDetails = new XbmcStreamDetails();
        }

        /// <summary>Gets or sets the information about containing streams.</summary>
        /// <value>The information about containing streams</value>
        [XmlElement("streamdetails", Form = XmlSchemaForm.Unqualified)]
        public XbmcStreamDetails StreamDetails { get; set; }

        /// <summary>Gets or sets the information about video streams in this file.</summary>
        /// <value>The information about video streams in this file.</value>
        /// <remarks>Its a proxy property to <see cref="XbmcStreamDetails"/>'s property <see cref="XbmcStreamDetails.Video">Video</see>.</remarks>
        /// <returns>Returns a list of video streams information if they exist; otherwise <c>null</c>.</returns>
        [XmlIgnore]
        public List<XbmcXmlVideoInfo> Videos {
            get { return StreamDetails.Video; }
            set { StreamDetails.Video = value; }
        }

        /// <summary>Gets or sets the information about audio streams in this file.</summary>
        /// <value>The information about audio streams in this file.</value>
        /// <remarks>Its a proxy property to <see cref="XbmcStreamDetails"/>'s property <see cref="XbmcStreamDetails.Audio">Audio</see>.</remarks>
        /// <returns>Returns a list of audio streams information if they exist; otherwise <c>null</c>.</returns>
        [XmlIgnore]
        public List<XbmcXmlAudioInfo> Audios {
            get { return StreamDetails.Audio; }
            set { StreamDetails.Audio = value; }
        }

        /// <summary>Gets or sets the information about video streams in this file.</summary>
        /// <value>The information about video streams in this file.</value>
        /// <remarks>Its a proxy property to <see cref="XbmcStreamDetails"/>'s property <see cref="XbmcStreamDetails.Subtitles">Subtitles</see>.</remarks>
        /// <returns>Returns a list of subtitle streams information if they exist; otherwise <c>null</c>.</returns>
        [XmlIgnore]
        public List<XbmcXmlSubtitleInfo> Subtitles {
            get { return StreamDetails.Subtitles; }
            set { StreamDetails.Subtitles = value; }
        }

        /// <summary>Gets subtitle languages as an array.</summary>
        /// <returns>An array with subtitle languages or <c>null</c> if there are no subtitles with known languages</returns>
        public string[] GetSubtitleLanguages() {
            if (Subtitles != null) {
                return Subtitles.Where(s => s.Language != null)
                                .Select(s => s.Language)
                                .ToArray();
            }
            return null;
        }

        /// <summary>Gets the subtitle languages formatted in a single string separated with " / ".</summary>
        /// <returns>A single formatted string with languages separated with " / " or <c>null</c> if there are no subtitles with known languages</returns>
        public string GetSubtitleLanguagesFormatted() {
            string[] langs = GetSubtitleLanguages();
            return langs != null
                       ? string.Join(SEPARATOR, langs)
                       : null;
        }

        /// <summary>Gets the information if a stream details with specified <c><paramref name="type">type</paramref></c> exists or not.</summary>
        /// <param name="type">The stream.</param>
        /// <returns>Returs <c>true</c> if a stream information of specified <c><paramref name="type">type</paramref></c> is available; otherwise <c>false</c>.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Throws when the specified <c><paramref name="type">type</paramref></c> is out of bounds (Enum value does not exist)</exception>
        public bool InfoExists(MediaType type) {
            switch (type) {
                case MediaType.Audio:
                    return StreamDetails.Audio != null && StreamDetails.Audio.Count > 0;
                case MediaType.Video:
                    return StreamDetails.Video != null && StreamDetails.Video.Count > 0;
                case MediaType.Subtitles:
                    return StreamDetails.Subtitles != null && StreamDetails.Subtitles.Count > 0;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }
    }
}