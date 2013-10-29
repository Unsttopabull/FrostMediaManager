using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo.People {

    public class MovieActor {

        public MovieActor(Person person, string character) {
            Person = person;
            Character = character;
        }

        public MovieActor(string personName, string character) : this(new Person(personName), character) {
        }

        [Key]
        public long Id { get; set; }
        public string Character { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }


        public static explicit operator Actor(MovieActor ma) {
            return new Actor(ma.Person, ma.Character);
        }
    }
}
