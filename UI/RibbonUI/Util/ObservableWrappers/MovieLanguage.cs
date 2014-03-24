using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieLanguage : MovieItemBase<ILanguage> {

        public MovieLanguage(ILanguage country) : base(country) {
        }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name {
            get { return _observedEntity.Name; }
            set { _observedEntity.Name = value; }
        }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO639 ISO3166 {
            get { return _observedEntity.ISO639; }
            set {
                _observedEntity.ISO639 = value;
                OnPropertyChanged();
            }
        }

        public string Image {
            get {
                if (ISO3166 == null) {
                    return null;
                }

                return GetImageSourceFromPath("Images/Languages/" + ISO3166.Alpha3 + ".png");
            }
        }
    }
}
