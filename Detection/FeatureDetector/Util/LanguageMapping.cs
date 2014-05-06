using System;
using Frost.Common.Util.Collections;

namespace Frost.DetectFeatures.Util {
    public class LanguageMapping : IEquatable<LanguageMapping>, IKeyValue {

        public LanguageMapping() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public LanguageMapping(string mapping, string iso639Alpha3) {
            Mapping = mapping;
            ISO639Alpha3 = iso639Alpha3;
        }

        public string Mapping { get; set; }

        public string ISO639Alpha3 { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(LanguageMapping other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(Mapping, other.Mapping, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} => {1}", Mapping, ISO639Alpha3);
        }

        #region IKeyValue

        string IKeyValue.Key {
            get { return Mapping; }
        }

        string IKeyValue.Value {
            get { return ISO639Alpha3; }
        }

        #endregion

    }
}
