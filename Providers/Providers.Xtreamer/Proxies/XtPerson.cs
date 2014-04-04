using System;
using System.Collections.Generic;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtPerson : ChangeTrackingProxy<XjbPhpPerson>, IPerson, IEquatable<XtPerson> {

        public XtPerson(XjbPhpPerson person) : base(person) {
            OriginalValues = new Dictionary<string, object> {
                {"Name", Entity.Name}
            };
        }

        public long Id {
            get { return Entity.Id; }
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return Entity.Name; }
            set {
                Entity.Name = value;
                TrackChanges(value);
            } 
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get { return null; }
            set { } 
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return null; }
            set { } 
        }

        public virtual bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Name":
                        return true;
                    default:
                        return false;
                }
            }
        }

        #region Equality comparers

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XtPerson other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (Entity == null && other.Entity == null) {
                return true;
            }

            return Entity != null && Entity.Equals(other.Entity);
        }

        #endregion
    }
}
