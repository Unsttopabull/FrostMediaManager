using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Annotations;
using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieStudio : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        public MovieStudio(IStudio studio) {
            Studio = studio;
        }

        public string Name {
            get { return Studio.Name; }
            set {
                Studio.Name = value;
                OnPropertyChanged();
            }
        }

        public IStudio Studio { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
