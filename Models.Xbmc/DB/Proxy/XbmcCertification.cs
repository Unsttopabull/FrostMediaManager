using Frost.Common.Models;
using Frost.Common.Util.ISO;

namespace Frost.Providers.Xbmc.DB.Proxy {
    public class XbmcCertification : ICertification {
        private readonly XbmcMovie _movie;
        private readonly ICountry _country;

        public XbmcCertification(XbmcMovie movie) {
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
