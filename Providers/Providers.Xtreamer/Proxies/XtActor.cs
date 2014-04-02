using System;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtActor : XtPerson, IActor, IEquatable<XtActor>, IEquatable<IActor> {

        public XtActor(XjbPhpPerson person) : base(person) {
            OriginalValues.Add("Character", Entity.Character);
        }

        public string Character {
            get { return Entity.Character; }
            set {
                TrackChanges(value);
                Entity.Character = value;
            }
        }

        public override bool this[string propertyName] {
            get { return propertyName == "Character" || base[propertyName]; }
        }

        #region Equality comparers

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XtActor other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (Entity == null && other.Entity == null) {
                return true;
            }

            return Entity != null && Entity.Equals(other.Entity);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IActor other) {
            if (ReferenceEquals(null, other)) {
                return false;
            }

            if (other is XtActor) {
                return Equals(other as XtActor);
            }

            return string.Equals(Name, other.Name, StringComparison.CurrentCultureIgnoreCase) &&
                   string.Equals(Character, other.Character, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion
    }

}