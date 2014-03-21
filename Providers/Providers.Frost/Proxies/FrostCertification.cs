using Frost.Common.Models;
using Frost.Providers.Frost.DB;
using Frost.Providers.Frost.Provider;

namespace Frost.Providers.Frost.Proxies {
    public class FrostCertification : ProxyBase<Certification>, ICertification {

        public FrostCertification(Certification art, FrostMoviesDataDataService service) : base(art, service) {
        }

        public long Id {
            get { return Entity.Id; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Id":
                    case "Rating":
                    case "Country":
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating {
            get { return Entity.Rating; }
            set { Entity.Rating = value; }
        }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ICountry Country {
            get { return Entity.Country; }
            set { Entity.Country = Service.FindOrCreateCountry(value, true); }
        }
    }
}
