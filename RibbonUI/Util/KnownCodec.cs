using System.IO;

namespace RibbonUI.Util {
    public class KnownCodec {

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public KnownCodec(string codecId, string imagePath) {
            CodecId = codecId;
            ImagePath = imagePath;
        }

        public KnownCodec(string filePath, bool isVideo) {
            ImagePath = "file://" + filePath;

            string codecId = Path.GetFileNameWithoutExtension(filePath);
            if (isVideo) {
                if (codecId != null) {
                    CodecId = codecId.Replace("vcodec_", "");
                }
            }
            else {
                if (codecId != null) {
                    CodecId = codecId.Replace("acodec_", "");
                }                
            }
        }

        public string CodecId { get; set; }

        public string ImagePath { get; set; }
    }
}
