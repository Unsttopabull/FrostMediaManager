using Frost.Common.Models.Provider;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieActor : MoviePerson {

        public MovieActor(IActor actor) : base(actor) {
        }

        public string Character {
            get { 
                string role = ((IActor)_observedEntity).Character;
                return string.IsNullOrEmpty(role)
                    ? null
                    : role;
            }
            set {
                ((IActor)_observedEntity).Character = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }
    }
}
