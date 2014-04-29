using System;
using System.Collections.Generic;
using System.Xml.Schema;
using System.Xml.Serialization;
using Frost.Common.NFO.Files;

namespace Frost.Common.NFO {

    /// <summary>Holds information about streams details in a movie.</summary>
    [Serializable]
    public class NfoStreamDetails {

        /// <summary>Gets or sets the information about video streams.</summary>
        /// <value>The information about video streams</value>
        [XmlElement("video", Form = XmlSchemaForm.Unqualified)]
        public List<NfoVideoInfo> Video { get; set; }

        /// <summary>Gets or sets the information about audio streams.</summary>
        /// <value>The information about audio streams</value>
        [XmlElement("audio", Form = XmlSchemaForm.Unqualified)]
        public List<NfoAudioInfo> Audio { get; set; }

        /// <summary>Gets or sets the information about subtitles streams.</summary>
        /// <value>The information about subtitles streams</value>
        [XmlElement("subtitle", Form = XmlSchemaForm.Unqualified)]
        public List<NfoSubtitleInfo> Subtitles { get; set; }

    }

}
