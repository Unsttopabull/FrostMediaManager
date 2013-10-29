using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.XML.Jukebox;

namespace Common.Models.DB.MovieVo {

    public class Person {
        private string _thumb, _character;

        public Person() {
            MoviesAsDirector = new HashSet<Movie>();
            MoviesAsWriter = new HashSet<Movie>();
            MoviesAsActor = new HashSet<Movie>();
        }

        public Person(string name) : this() {
            Name = name;
        }

        public Person(string name, string thumb) : this(name) {
            if (!string.IsNullOrEmpty(thumb)) {
                _thumb = thumb;
            }
        }

        public Person(string name, string thumb, string character) : this(name, thumb) {
            Character = character;
        }


        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Thumb {
            get { return string.IsNullOrEmpty(_thumb) ? null : _thumb; }
            set { _thumb = value; }
        }

        public string Character {
            get { return string.IsNullOrEmpty(_character) ? null : _character; }
            set { _character = value; }
        }

        [InverseProperty("Directors")]
        public virtual ICollection<Movie> MoviesAsDirector { get; set; }

        [InverseProperty("Actors")]
        public virtual ICollection<Movie> MoviesAsActor { get; set; }

        [InverseProperty("Writers")]
        public virtual ICollection<Movie> MoviesAsWriter { get; set; }

        public static explicit operator XjbXmlActor(Person act) {
            return new XjbXmlActor(
                act.Character ?? "",
                act.Name ?? "",
                act.Thumb
            );
        }
    }
}
