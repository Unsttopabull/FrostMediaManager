using System;
using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtCertification : ICertification {
        private readonly XjbPhpMovie _movie;
        private string _countryName;
        private ICountry _country;

        public XtCertification(XjbPhpMovie movie, string country) {
            _movie = movie;
            _countryName = country;

            _country = XtCountry.FromIsoCode(_countryName);
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating {
            get { return _movie.Certifications[_countryName]; }
            set { _movie.Certifications[_countryName] = value; }
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

                string rating = _movie.Certifications[_countryName];
                _movie.Certifications.Remove(_countryName);
                _movie.Certifications.Add(iso, rating);
                _countryName = iso;

                _country = value;
            }
        }

        public bool this[string propertyName] {
            get { throw new NotImplementedException(); }
        }
    }
}
