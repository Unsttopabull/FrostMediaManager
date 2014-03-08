using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Annotations;
using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {
    public class ObservableActor : IActor, INotifyPropertyChanged {
        private readonly IActor _actor;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableActor(IActor actor) {
            _actor = actor;
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return Actor.Name; }
            set {
                Actor.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get { return Actor.Thumb; }
            set {
                Actor.Thumb = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return Actor.ImdbID; }
            set {
                Actor.ImdbID = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        public ICollection<IMovie> MoviesAsDirector {
            get { return Actor.MoviesAsDirector; }
        }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        public ICollection<IMovie> MoviesAsWriter {
            get { return Actor.MoviesAsWriter; }
        }

        /// <summary>Gets movies this person acted in.</summary>
        /// <value>The movies this person acted in.</value>
        public IEnumerable<IMovie> MoviesAsActor {
            get { return Actor.MoviesAsActor; }
        }

        public string Character {
            get { return Actor.Character; }
            set {
                Actor.Character = value;
                OnPropertyChanged();
            }
        }

        public IMovie Movie {
            get { return _actor.Movie; }
            set { _actor.Movie = value; }
        }

        public IActor Actor {
            get { return _actor; }
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
