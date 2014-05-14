using System;
using System.Collections.Generic;
using Frost.Common.Comparers;
using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Util.ObservableWrappers {

    public class MoviePerson : ObservableBase<IPerson>, IEquatable<MoviePerson>, IEquatable<IPerson> {
        private readonly IEqualityComparer<IPerson> _comparer;

        public MoviePerson(IPerson person) : base(person) {
            _comparer = new PersonEqualityComparer();
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return _observedEntity.Name; }
            set {
                _observedEntity.Name = string.IsNullOrEmpty(value) ? null : value;

                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get {
                string thumb = _observedEntity.Thumb;
                return !string.IsNullOrEmpty(thumb) ? thumb : null;
            }
            set {
                _observedEntity.Thumb = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return _observedEntity.ImdbID; }
            set {
                _observedEntity.ImdbID = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        #region Equality Comparers

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MoviePerson other) {
            return !ReferenceEquals(null, other) &&
                   _comparer.Equals(_observedEntity, other.ObservedEntity);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IPerson other) {
            return !ReferenceEquals(null, other) &&
                   _comparer.Equals(_observedEntity, other);
        }

        #endregion

    }
}
