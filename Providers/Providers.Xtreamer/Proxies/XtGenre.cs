using System;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    class XtGenre : Proxy<XjbPhpGenre>, IGenre, IEquatable<XtGenre> {

        public XtGenre() : base(new XjbPhpGenre()) {
        }

        public XtGenre(XjbPhpGenre genre) : base(genre) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public string Name {
            get { return Entity.Name; }
            set {
                Entity.Name = value;
            }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case"Name":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XtGenre other) {
            return Entity != null && Entity.Equals(other.ObservedEntity);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return Entity != null
                ? Entity.Name
                : base.ToString();
        }
    }
}
