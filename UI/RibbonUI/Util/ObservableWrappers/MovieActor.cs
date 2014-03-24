using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieActor : ObservableBase<IActor> {

        public MovieActor(IActor actor) : base(actor) {
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return _observedEntity.Name; }
            set {
                _observedEntity.Name = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get {
                string thumb = _observedEntity.Thumb;
                return !string.IsNullOrEmpty(thumb) ? thumb : null;
            }
            set {
                _observedEntity.Thumb = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return _observedEntity.ImdbID; }
            set {
                _observedEntity.ImdbID = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        ///// <summary>Gets or sets movies where this person was a director.</summary>
        ///// <value>The movies where this person was a director.</value>
        //public IEnumerable<IMovie> MoviesAsDirector {
        //    get { return _actor.MoviesAsDirector; }
        //}

        ///// <summary>Gets or sets movies where this person was a writer.</summary>
        ///// <value>The movies where this person was a writer.</value>
        //public IEnumerable<IMovie> MoviesAsWriter {
        //    get { return _actor.MoviesAsWriter; }
        //}

        public string Character {
            get { 
                string role = _observedEntity.Character;
                return string.IsNullOrEmpty(role)
                    ? null
                    : role;
            }
            set {
                _observedEntity.Character = string.IsNullOrEmpty(value) ? null : value;
                OnPropertyChanged();
            }
        }

        public MoviePerson ObservedPerson {
            get { return new MoviePerson(_observedEntity); }
        }
    }
}
