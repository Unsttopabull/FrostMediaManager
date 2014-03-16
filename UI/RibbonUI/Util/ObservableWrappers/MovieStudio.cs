using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieStudio : MovieItemBase<IStudio> {
        public MovieStudio(IStudio studio) : base(studio){
        }

        public string Name {
            get { return _observedEntity.Name; }
            set {
                _observedEntity.Name = value;
                OnPropertyChanged();
            }
        }

        public string StudioLogo {
            get {
                if (string.IsNullOrEmpty(Name)) {
                    return null;
                }

                return GetImageSourceFromPath("Images/StudiosE/" + Name + ".png");
            }
        }
    }
}
