﻿using System;
using Frost.Common.Comparers;
using Frost.Common.Models.Provider;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieStudio : MovieItemBase<IStudio>, IEquatable<MovieStudio> {
        private readonly HasNameEqualityComparer _comparer;

        public MovieStudio(IStudio studio) : base(studio){
            _comparer = new HasNameEqualityComparer();
        }

        public string Name {
            get { return _observedEntity.Name; }
            set {
                _observedEntity.Name = value;
                OnPropertyChanged();
            }
        }

        public string StudioLogo {
            get {
                return !string.IsNullOrEmpty(Name)
                    ? GetImageSourceFromPath("Images/StudiosE/" + Name + ".png")
                    : null;
            }
        }

        #region Equality Comparers

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public override bool Equals(object other) {
            return Equals(other as MovieStudio);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MovieStudio other) {
            return !ReferenceEquals(null, other) &&
                   _comparer.Equals(_observedEntity, other.ObservedEntity);
        }

        /// <summary>Serves as a hash function for a particular type.</summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        public override int GetHashCode() {
            return _comparer.GetHashCode(_observedEntity);
        }

        #endregion
    }
}
