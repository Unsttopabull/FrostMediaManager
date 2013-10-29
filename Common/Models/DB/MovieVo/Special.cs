using System.Collections.Generic;

namespace Common.Models.DB.MovieVo {
    public class Special {

        public Special(string value) {
            Value = value;
        }

        public long Id { get; set; }

        public string Value { get; set; }

        public ICollection<Movie> Movies { get; set; }
    }
}