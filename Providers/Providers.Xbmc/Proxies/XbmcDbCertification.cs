﻿using Frost.Common.Models.Provider;
using Frost.Common.Util.ISO;
using Frost.Providers.Xbmc.DB;

namespace Frost.Providers.Xbmc.Proxies {
    public class XbmcCertification : ICertification {
        private readonly XbmcDbMovie _movie;
        private readonly ICountry _country;

        public XbmcCertification(XbmcDbMovie movie) {
            _movie = movie;
            _country = new XbmcCountry(ISOCountryCodes.Instance.GetByISOCode("USA"));
        }

        public long Id { get; private set; }

        public bool this[string propertyName] {
            get { return true; }
        }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating {
            get { return _movie.MpaaRating; }
            set { _movie.MpaaRating = value; }
        }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ICountry Country {
            get { return _country; }
            set { }
        }
    }
}