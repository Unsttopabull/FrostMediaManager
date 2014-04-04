using System;
using Frost.Common.Util;
using Frost.DetectFeatures.FileName;

namespace Frost.DetectFeatures.Util {

    public class SegmentMapping : IEquatable<SegmentMapping>, IKeyValue {

        public SegmentMapping() {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public SegmentMapping(string segment, SegmentType segmentType) {
            Segment = segment;
            SegmentType = segmentType;
        }
        
        public string Segment { get; set; }
        public SegmentType SegmentType { get; set; }

        #region IKeyValue

        string IKeyValue.Key {
            get { return Segment; }
        }

        string IKeyValue.Value {
            get { return SegmentType.ToString(); }
        }

        #endregion

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(SegmentMapping other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(Segment, other.Segment);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} => {1}", Segment, SegmentType);
        }
    }

}