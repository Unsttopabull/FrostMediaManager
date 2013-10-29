using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.DB.MovieVo {
    public class Set {
        private Set() {
            Movies = new HashSet<Movie>();
        }

        public Set(string name) : this() {
            Name = name;
        }

        [Key]
        public long Id { get; set; }

        public string Name { get; set; }

        protected ICollection<Movie> Movies { get; set; }
    }
}