using Frost.Common.Models.DB.MovieVo.Files;
using Frost.SharpMediaInfo;

namespace Frost.FeatureDetector {
    public class FeatureDetector {
        private MediaFile _mediaFile;

        public FeatureDetector(string filepath) {
            _mediaFile = new MediaFile(filepath, false);
        }

        public Video GetVideInfo() {
            Video v = new Video();

            return v;
        }
    }
}
