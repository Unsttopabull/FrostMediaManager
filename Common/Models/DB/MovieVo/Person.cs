using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.DB.MovieVo {

    public class Person{

        public Person() {
            Movies = new HashSet<MoviePerson>();
        }

        public Person(string name) : this() {
            Name = name;
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MoviePerson> Movies { get; set; }
    }
}
