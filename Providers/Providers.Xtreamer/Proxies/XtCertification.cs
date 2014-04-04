using System.Collections.Generic;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtCertification : ChangeTrackingProxy<XjbPhpMovie>, ICertification {
        private string _countryName;
        private ICountry _country;

        public XtCertification(XjbPhpMovie movie, string country) : base(movie) {
            _countryName = country;
            _country = XtCountry.FromIsoCode(_countryName);

            OriginalValues = new Dictionary<string, object> {
                {"Rating", Entity.Certifications[_countryName]},
                {"Country", country}
            };
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating {
            get { return Entity.Certifications[_countryName]; }
            set {
                TrackChanges(value);
                Entity.Certifications[_countryName] = value;
            }
        }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ICountry Country {
            get { return _country; }
            set {
                string iso = "unk";
                if (value != null) {
                    iso = value.ISO3166.Alpha3;
                }
                TrackChanges(iso);

                string rating = Entity.Certifications[_countryName];
                Entity.Certifications.Remove(_countryName);
                Entity.Certifications.Add(iso, rating);
                _countryName = iso;

                _country = value;
            }
        }

        public bool this[string propertyName] {
            get { return propertyName == "Rating" || propertyName == "Country"; }
        }
    }
}
