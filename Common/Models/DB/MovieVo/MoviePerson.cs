using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class MoviePerson {

        public MoviePerson(Person person, string job) {
            Person = person;
            Job = job;
        }

        public MoviePerson(string personName, string job)
            : this(new Person(personName), job) {
        }

        public MoviePerson(Actor actor) : this(actor, "actor") {
            
        }

        [Key]
        public long Id { get; set; }
        public long MovieId { get; set; }
        public long PersonId { get; set; }
        public string Job { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }
    }
}
