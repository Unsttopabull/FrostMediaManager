using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Frost.Common.Models;
using Frost.Common.Models.ISO;
using RibbonUI.Annotations;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieLanguage : MovieItemBase, INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly ILanguage _language;

        public MovieLanguage(ILanguage country) {
            _language = country;
        }

        /// <summary>Gets or sets the country name.</summary>
        /// <value>The name of the country.</value>
        public string Name {
            get { return _language.Name; }
            set { _language.Name = value; }
        }

        /// <summary>Gets or sets the ISO 3166-1 Information.</summary>
        /// <value>The ISO 3166-1 Information.</value>
        public ISO639 ISO3166 {
            get { return _language.ISO639; }
            set {
                _language.ISO639 = value;
                OnPropertyChanged();
            }
        }

        public ILanguage ObservedLanguage {
            get { return _language; }
        }

        public ImageSource Image {
            get { return GetImageSourceFromPath("Images/Languages/" + ISO3166.Alpha3 + ".png"); }
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
