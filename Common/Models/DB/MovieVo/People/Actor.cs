using Common.Models.XML.Jukebox;

namespace Common.Models.DB.MovieVo.People {

    public class Actor : Person {
        private string _character;

        public Actor(string name) : base(name) {
        }

        public Actor(string name, string thumb) : base(name, thumb) {
        }

        public Actor(string name, string thumb, string character) : base(name, thumb) {
            _character = character;
        }

        public Actor(Person person, string character) : base(person.Id, person.Name, person.Thumb) {
            _character = character;
        }

        public string Character {
            get {
                return string.IsNullOrEmpty(_character)
                               ? null
                               : _character;
            }
            set { _character = value; }
        }

        public override string ToString() {
            return Name + " as " + Character;
        }

        public static explicit operator XjbXmlActor(Actor act) {
            return new XjbXmlActor(
                    act.Character ?? "",
                    act.Name ?? "",
                    act.Thumb
                    );
        }
    }

}