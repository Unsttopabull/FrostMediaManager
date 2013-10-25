using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common.Models.DB.MovieVo {
    public class Country {
        public Country() {
            Movies = new HashSet<Movie>();
        }

        public Country(string name) : this() {
            Name = name;
        }

        [Key]
        public long ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

        public static Country[] GetFromNames(string[] countryNames) {
            int numCountries = countryNames.Length;

            Country[] countries = new Country[numCountries];
            for (int i = 0; i < numCountries; i++) {
                countries[i] = new Country(countryNames[i]);
            }

            return countries;
        }
    }
}
