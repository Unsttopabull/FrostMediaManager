using System.Collections.Generic;
using Common.Models.XML.Jukebox;

namespace Common.Models.DB.MovieVo {

    public class Actor : Person {
        public Actor() {
        }

        public Actor(string name, string character, string thumb) : base(name, thumb) {
            Movies = new HashSet<Movie>();
            Character = character;
        }

        public Actor(string name, string character) : this(name, character, null) {
        }

        public virtual ICollection<Movie> Movies { get; set; }

        public static explicit operator XjbXmlActor(Actor act) {
            return new XjbXmlActor(
                act.Character ?? "",
                act.Name ?? "",
                act.Thumb
            );
        }
    }
}