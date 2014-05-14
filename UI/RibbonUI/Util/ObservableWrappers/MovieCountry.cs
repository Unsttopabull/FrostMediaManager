using System;
using System.Collections.Generic;
using Frost.Common.Comparers;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;

namespace Frost.RibbonUI.Util.ObservableWrappers {
    public class MovieCountry : MovieItemBase<ICountry>, IEquatable<MovieCountry>, IEquatable<ICountry> {
        private readonly IEqualityComparer<ICountry> _comparer;

        public MovieCountry(ICountry country) : base(country) {
            _comparer = new CountryEqualityComparer();
        }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name {
            get { return _observedEntity.Name; }
            set {
                _observedEntity.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO3166 ISO3166 {
            get { return _observedEntity.ISO3166; }
            set {
                _observedEntity.ISO3166 = value;
                OnPropertyChanged();
            }
        }

        public string Image {
            get {
                if (ISO3166 == null) {
                    return null;
                }

                return GetImageSourceFromPath("Images/Countries/" + ISO3166.Alpha3 + ".png");
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(MovieCountry other) {
            return !ReferenceEquals(null, other) &&
                   _comparer.Equals(_observedEntity, other.ObservedEntity);
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ICountry other) {
            return !ReferenceEquals(null, other) &&
                   _comparer.Equals(_observedEntity, other);
        }
    }
}
