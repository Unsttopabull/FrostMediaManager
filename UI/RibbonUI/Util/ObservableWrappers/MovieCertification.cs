﻿using System;
using Frost.Common.Comparers;
using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Util.ObservableWrappers {
    public class MovieCertification : MovieItemBase<ICertification>, ICertification, IEquatable<ICertification> {
        private static CountryEqualityComparer _comparer = new CountryEqualityComparer();

        public MovieCertification(ICertification observed) : base(observed) {
        }

        public long Id { get { return _observedEntity.Id; } }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating {
            get { return _observedEntity.Rating; }
            set { _observedEntity.Rating = value; }
        }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ICountry Country {
            get { return _observedEntity.Country; }
            set { _observedEntity.Country = value; }
        }

        public string CountryImage {
            get {
                if (Country == null || Country.ISO3166 == null) {
                    return null;
                }

                string alpha3 = Country.ISO3166.Alpha3;
                return GetImageSourceFromPath("Images/Countries/" + alpha3 + ".png");
            }
        }

        public string IsoAlpha3 {
            get {
                if (Country == null || Country.ISO3166 == null) {
                    return null;
                }
                return Country.ISO3166.Alpha3.ToLower();
            }
        }

        public string CertificationImage {
            get {
                if (Country == null || Country.ISO3166 == null) {
                    return null;
                }

                string rating = Rating;
                if (Country.ISO3166.Alpha3.Equals("usa", StringComparison.OrdinalIgnoreCase)) {
                    rating = "mpaa" + rating.Replace("-", "");
                }

                return GetImageSourceFromPath(string.Format("Images/RatingsE/{0}/{1}.png", Country.ISO3166.Alpha3, rating));
            }
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ICertification other) {
            if (other == null) {
                return false;
            }

            return string.Equals(Rating, other.Rating) && _comparer.Equals(Country, other.Country);
        }
    }
}
