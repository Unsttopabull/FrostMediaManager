using System;
using Frost.Common.Models.Provider;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieActor : MoviePerson, IEquatable<IActor> {

        public MovieActor(IActor actor) : base(actor) {
        }

        public string Character {
            get { 
                string role = ((IActor)_observedEntity).Character;
                return string.IsNullOrEmpty(role)
                    ? null
                    : role;
            }
            set {
                ((IActor)_observedEntity).Character = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IActor other) {
            return base.Equals(other) &&
                   string.Equals(Character, other.Character, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
