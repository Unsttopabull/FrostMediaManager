using System;

namespace Frost.DetectFeatures.Util.AspectRatio {

    public class AspectRatioInfo : IEquatable<float>, IComparable<float>, IComparable<AspectRatioInfo> {
        internal AspectRatioInfo(string name, float minRatio, float maxRatio, float usualRatio) {
            ComercialName = name;
            MinRatio = minRatio;
            MaxRatio = maxRatio;
            UsualRatio = usualRatio;
        }

        public string ComercialName { get; set; }

        public float MinRatio { get; set; }
        public float MaxRatio { get; set; }

        public float UsualRatio { get; set; }

        /// <summary>Compares the current object with another object of the same type.</summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.
        /// Zero This object is equal to <paramref name="other"/>.
        /// Greater than zero This object is greater than <paramref name="other"/>. 
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(float other) {
            if (other < MinRatio) {
                return 1;
            }

            return (other > MaxRatio)
                ? -1
                : 0;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>Is <c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(float other) {
            return other >= MinRatio && other <= MaxRatio;
        }

        /// <summary>Compares the current object with another object of the same type.</summary>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the <paramref name="other"/> parameter.Zero This object is equal to <paramref name="other"/>. Greater than zero This object is greater than <paramref name="other"/>.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public int CompareTo(AspectRatioInfo other) {
            return CompareTo(other.MinRatio);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString() {
            return UsualRatio + " (- " + MinRatio + " | + " + MaxRatio + ") => " + ComercialName;
        }
    }

}