using System;
using System.Collections.Generic;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;
using Frost.Providers.Xtreamer.Proxies.ChangeTrackers;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtSpecial : ChangeTrackingProxy<XjbPhpMovie>, ISpecial, IEquatable<XtSpecial> {
        private string _special;

        public XtSpecial(XjbPhpMovie movie, string special) : base(movie){
            _special = special;
            OriginalValues = new Dictionary<string, object> { { "Name", _special } };
        }

        public long Id {
            get { return 0; }
        }

        public string Name {
            get { return _special; }
            set {
                if (value != null && value.Equals(_special, StringComparison.CurrentCultureIgnoreCase)) {
                    return;
                }

                if (value != null) {
                    Entity.RemoveSpecial(_special);
                    Entity.AddSpecial(value);
                }
                else {
                    Entity.RemoveSpecial(_special);
                }
                _special = value;
                TrackChanges(value);
            }
        }

        public bool this[string propertyName] {
            get { return propertyName == "Name"; }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XtSpecial other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return string.Equals(_special, other._special);
        }
    }
}
