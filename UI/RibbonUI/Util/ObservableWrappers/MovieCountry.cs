using Frost.Common.Models;
using Frost.Common.Models.ISO;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieCountry : MovieItemBase<ICountry>{
        public MovieCountry(ICountry country) : base(country) {
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
    }
}
