using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Frost.Common.Models;
using Frost.Common.Properties;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieStudio : MovieItemBase, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IStudio _studio;

        public MovieStudio(IStudio studio) {
            _studio = studio;
        }

        public string Name {
            get { return ObservedStudio.Name; }
            set {
                ObservedStudio.Name = value;
                OnPropertyChanged();
            }
        }

        public IStudio ObservedStudio { get { return _studio; }}

        public ImageSource StudioLogo {
            get {
                if (string.IsNullOrEmpty(Name)) {
                    return null;
                }

                return GetImageSourceFromPath("Images/StudiosE/" + Name + ".png");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
