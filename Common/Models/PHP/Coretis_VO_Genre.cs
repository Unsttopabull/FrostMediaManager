using Frost.Common.Models.DB.MovieVo;

namespace Frost.Common.Models.PHP {

    public class Coretis_VO_Genre {

        /// <summary>Initializes a new instance of the <see cref="Coretis_VO_Genre"/> class.</summary>
        public Coretis_VO_Genre() {
        }

        /// <summary>Initializes a new instance of the <see cref="Coretis_VO_Genre"/> class.</summary>
        /// <param name="name">The genre name.</param>
        public Coretis_VO_Genre(string name) {
            this.name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="Coretis_VO_Genre"/> class.</summary>
        /// <param name="id">The database identifier.</param>
        /// <param name="name">The genre name.</param>
        public Coretis_VO_Genre(int id, string name) : this(name) {
            this.id = id;
        }

        /// <summary>The id for this row in DB</summary>
        /// <remarks>id in DB</remarks>
        public int id { get; set; }

        /// <summary>Gets or sets the name of the genre</summary>
        /// <value>The name of the genre</value>
        /// <example>\eg{ ''<c>horror</c>'', ''<c>comedy</c>''}</example>
        public string name { get; set; }

        /// <summary>Converts an instance of <see cref="Coretis_VO_Genre"/> to an instance of <see cref="Genre"/></summary>
        /// <param name="genre">The genre to convert</param>
        /// <returns>An instance of <see cref="Genre"/> converted from <see cref="Coretis_VO_Genre"/></returns>
        public static explicit operator Genre(Coretis_VO_Genre genre) {
            return new Genre(genre.name);
        }

    }

}
