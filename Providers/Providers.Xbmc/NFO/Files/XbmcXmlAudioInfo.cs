using System;
using System.Xml.Schema;
using System.Xml.Serialization;
using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace Frost.Providers.Xbmc.NFO.Files {

    /// <summary>Represents serialized information about an audio stream in a movie</summary>
    [Serializable]
    public class XbmcXmlAudioInfo {

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlAudioInfo"/> class.</summary>
        public XbmcXmlAudioInfo() {
        }

        public XbmcXmlAudioInfo(IAudio audio) {
            Codec = audio.Codec;
            Channels = audio.NumberOfChannels ?? 0;

            if (audio.Language != null && audio.Language.ISO639 != null) {
                Language = audio.Language.ISO639.Alpha3;
            }
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlAudioInfo"/> class.</summary>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The number of chanells in the audio stream (5.1 has 6 chanels)</param>
        /// <param name="language">The language of this audio in a 3 letter abreviation (ISO 639-2 Code).</param>
        /// <param name="longLanguage">The full name of the language in this audio stream</param>
        public XbmcXmlAudioInfo(string codec, int channels, string language, string longLanguage) {
            Codec = codec;
            Language = language;
            LongLanguage = longLanguage;
            Channels = channels;
        }

        /// <summary>Initializes a new instance of the <see cref="XbmcXmlAudioInfo"/> class.</summary>
        /// <param name="codec">The codec this audio is encoded in.</param>
        /// <param name="channels">The number of chanells in the audio stream (5.1 has 6 chanels)</param>
        /// <param name="language">The language of this audio in a 3 letter abreviation (ISO 639-2 Code).</param>
        public XbmcXmlAudioInfo(string codec, int channels, string language) : this(codec, channels, language, null) {
        }

        /// <summary>Gets or sets the codec this audio is encoded in.</summary>
        /// <value>The codec this audio is encoded in.</value>
        /// <example>\eg{ <c>MP3, AC3, FLAC</c>}</example>
        [XmlElement("codec", Form = XmlSchemaForm.Unqualified)]
        public string Codec { get; set; }

        /// <summary>Gets or sets the language of this audio in a 3 letter abreviation (ISO 639-2 Code).</summary>
        /// <value>The language of this audio in a 3 letter abreviation (ISO 639-2 Code).</value>
        /// <example>\eg{<c>"eng"</c> for English, <c>"spa"</c> for Spanish }</example>
        [XmlElement("language", Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        /// <summary>Gets or sets the full name of the language in this audio stream.</summary>
        /// <value>The full name of the language in this audio stream</value>
        [XmlElement("longlanguage", Form = XmlSchemaForm.Unqualified)]
        public string LongLanguage { get; set; }

        /// <summary>Gets or sets the number of chanells in the audio stream (5.1 has 6 chanels)</summary>
        /// <value>The number of chanells in the audio stream (5.1 has 6 chanels)</value>
        [XmlElement("channels", Form = XmlSchemaForm.Unqualified)]
        public int Channels { get; set; }
    }

}
