using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Models;
using Frost.Common.Properties;

namespace RibbonUI.Util.ObservableWrappers {
    public class MovieActor : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly IActor _actor;

        public MovieActor(IActor actor) {
            _actor = actor;
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return ObservedActor.Name; }
            set {
                _actor.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get { return ObservedActor.Thumb; }
            set {
                _actor.Thumb = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return ObservedActor.ImdbID; }
            set {
                _actor.ImdbID = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        public IEnumerable<IMovie> MoviesAsDirector {
            get { return _actor.MoviesAsDirector; }
        }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        public IEnumerable<IMovie> MoviesAsWriter {
            get { return _actor.MoviesAsWriter; }
        }

        public string Character {
            get { return _actor.Character; }
            set {
                _actor.Character = value;
                OnPropertyChanged();
            }
        }

        public IActor ObservedActor {
            get { return _actor; }
        }

        public MoviePerson ObservedPerson {
            get { return new MoviePerson(_actor); }
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
