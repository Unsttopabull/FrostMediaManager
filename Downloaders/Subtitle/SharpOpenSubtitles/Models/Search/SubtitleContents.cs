using System;
using System.IO;
using System.IO.Compression;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Search {
    public class SubtitleContents {

        /// <summary>Subtitle ID</summary>
        [XmlRpcMember("idsubtitlefile")]
        public string SubtitleFileID;

        /// <summary>Base64 and GZIPed file contents</summary>
        [XmlRpcMember("data")]
        public string Data; //Base64 GZIP

        public void SaveSubtitle(string filepath) {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Data))) {
                using (GZipStream gZip = new GZipStream(ms, CompressionMode.Decompress)) {
                    using (FileStream fs = File.Create(filepath)) {
                        gZip.CopyTo(fs);
                    }
                }
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return SubtitleFileID;
        }
    }
}