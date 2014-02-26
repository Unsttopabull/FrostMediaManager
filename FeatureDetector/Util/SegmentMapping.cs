using Frost.DetectFeatures.FileName;

namespace Frost.DetectFeatures.Util {

    public class SegmentMapping {

        public SegmentMapping() {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public SegmentMapping(string segment, SegmentType segmentType) {
            Segment = segment;
            SegmentType = segmentType;
        }
        
        public string Segment { get; set; }
        public SegmentType SegmentType { get; set; }
    }

}