using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Frost.Common.Annotations;
using Frost.Common.Models;

namespace RibbonUI.Util.ObservableWrappers {

    public class MoviePerson : INotifyPropertyChanged, IPerson {
        private IPerson _person;
        public event PropertyChangedEventHandler PropertyChanged;

        public MoviePerson(IPerson person) {
            Person = person;
        }

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        public string Name {
            get { return _person.Name; }
            set {
                _person.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        public string Thumb {
            get {
                return _person.Thumb;
            }
            set {
                _person.Thumb = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        public string ImdbID {
            get { return _person.ImdbID; }
            set {
                _person.ImdbID = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Gets or sets movies where this person was a director.</summary>
        /// <value>The movies where this person was a director.</value>
        public ICollection<IMovie> MoviesAsDirector {
            get { return _person.MoviesAsDirector; }
        }

        /// <summary>Gets or sets movies where this person was a writer.</summary>
        /// <value>The movies where this person was a writer.</value>
        public ICollection<IMovie> MoviesAsWriter {
            get { return _person.MoviesAsWriter; }
        }

        public IPerson Person {
            get { return _person; }
            set {
                if (Equals(value, _person)) {
                    return;
                }
                _person = value;
                OnPropertyChanged();
                OnPropertyChanged("Name");
                OnPropertyChanged("Thumb");
                OnPropertyChanged("ImdbID");
                OnPropertyChanged("MoviesAsDirector");
                OnPropertyChanged("MoviesAsWriter");
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
