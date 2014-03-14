using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Frost.Common.Models;
using Frost.Common.Models.ISO;
using RibbonUI.Annotations;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieCountry : MovieItemBase, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ICountry _country;

        public MovieCountry(ICountry country) {
            _country = country;
        }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name {
            get { return _country.Name; }
            set {
                _country.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO3166 ISO3166 {
            get { return _country.ISO3166; }
            set {
                _country.ISO3166 = value;
                OnPropertyChanged();
            }
        }

        public ICountry ObservedCountry {
            get { return _country; }
        }

        public ImageSource Image {
            get { return GetImageSourceFromPath("Images/Countries/" + ISO3166.Alpha3 + ".png"); }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
