using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Frost.SharpMediaInfo.Output.Properties;

#pragma warning disable 1591

namespace Frost.SharpMediaInfo.Output {
    public abstract class Media : IEnumerable<KeyValuePair<string, string>> {
        private bool _cached;
        private MediaFile MediaFile;
        private StreamKind StreamKind;
        private Dictionary<string, string>[] Properties;
        protected int CachedStreamCount = -1;

        protected Media(MediaFile mediaInfo, StreamKind streamKind) {
            MediaFile = mediaInfo;
            StreamKind = streamKind;
            StreamNumber = 0;
            Properties = new Dictionary<string, string>[StreamCount];
            CachedStreamCount = StreamCount;
            CodecID = new CodecID(this);
        }

        #region Properties

        public string Status { get { return this["Status"]; } }

        public string StreamKindID { get { return this["StreamKindID"]; } }
        public string StreamKindPos { get { return this["StreamKindPos"]; } }
        public string StreamOrder { get { return this["StreamOrder"]; } }

        public string FirstPacketOrder { get { return this["FirstPacketOrder"]; } }

        public string ID { get { return this["ID"]; } }
        public string IDString { get { return this["IDString"]; } }
        public string UniqueID { get { return this["UniqueID"]; } }
        public string UniqueIDString { get { return this["UniqueID/String"]; } }
        public string MenuID { get { return this["MenuID"]; } }
        public string MenuIDString { get { return this["MenuID/String"]; } }

        /// <summary>The default stream number to use when accessing media info through properties</summary>
        public int StreamNumber { get; set; }

        /// <summary>Count of Stream Kind streams</summary>
        public int StreamCount {
            get {
                if (CachedStreamCount == -1) {
                    CachedStreamCount = MediaFile.CountGet(StreamKind);
                }
                return CachedStreamCount;
            }
        }

        public CodecID CodecID { get; private set; }

        /// <summary>Gets a value indicating whether information exists about this kind of stream</summary>
        /// <value><c>true</c> if info is available; otherwise, <c>false</c>.</value>
        /// <remarks>Equivalent to checking if StreamCount is equal to 0</remarks>
        public bool Any { get { return StreamCount != 0; } }

        #endregion

        #region Indexers
        /// <summary>Get a piece of information about a media element</summary>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in string format ("Codec", "Width"...) </param>
        public string this[string parameter] {
            get {
                if (_cached && Properties[StreamNumber].ContainsKey(parameter)) {
                    return Properties[StreamNumber][parameter];
                }

                return MediaFile.Get(StreamKind, StreamNumber, parameter);
            }
        }

        /// <summary>Get a piece of information about a media element (parameter is an integer)</summary>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in integer format (first parameter, second parameter...)</param>
        public string this[int parameter] {
            get {
                return MediaFile.Get(StreamKind, StreamNumber, parameter);
            }
        }
        #endregion

        #region Getters & Functions
        internal virtual void ParseInform(XElement track, int streamNumber) {
            if (_cached) {
                return;
            }

            Properties[streamNumber] = new Dictionary<string, string>();

            foreach (XElement xElement in track.Nodes()) {
                Properties[streamNumber][xElement.Name.LocalName] = xElement.Value;
            }
            _cached = true;
        }

        /// <summary>Get a piece of information about an image</summary>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in string format ("Codec", "Width"...) </param>
        /// <param name="kindOfInfo">Kind of information you want about the parameter (the text, the measure, the help...)</param>
        /// <param name="kindOfSearch">Where to look for the parameter</param>
        /// <returns>a string about information you search, an empty string if there is a problem</returns>
        public string Get(string parameter, InfoKind kindOfInfo, InfoKind kindOfSearch) {
            return MediaFile.Get(StreamKind, StreamNumber, parameter, kindOfInfo, kindOfSearch);
        }

        /// <summary>Get a piece of information about an image (parameter is an integer)</summary>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in integer format (first parameter, second parameter...)</param>
        /// <param name="kindOfInfo">Kind of information you want about the parameter (the text, the measure, the help...)</param>
        /// <returns>a string about information you search, an empty string if there is a problem</returns>
        public string Get(int parameter, InfoKind kindOfInfo) {
            return MediaFile.Get(StreamKind, StreamNumber, parameter, kindOfInfo);
        }
        #endregion

        #region IEnumerable
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return Properties[StreamNumber].GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            StringBuilder sb = new StringBuilder(10000);
            foreach (KeyValuePair<string, string> kvp in Properties[StreamNumber]) {
                sb.AppendLine(kvp.Key + " : " + kvp.Value);
            }
            return sb.ToString();
        }
    }
}