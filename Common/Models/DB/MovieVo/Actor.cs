using Common.Models.XML.Jukebox;

namespace Common.Models.DB.MovieVo {

    public class Actor : Person {
        private string _character;
        private string _thumb;

        public Actor(string name, string character, string thumb) : base(name) {
            _character = character;

            if (!string.IsNullOrEmpty(thumb)) {
                _thumb = thumb;
            }
        }

        public Actor(string name, string character) : this(name, character, null) {
        }

        public string Character {
            get { return string.IsNullOrEmpty(_character) ? null : _character; }
            set { _character = value; }
        }

        public string Thumb {
            get { return string.IsNullOrEmpty(_thumb) ? null : _thumb; }
            set { _thumb = value; }
        }

        public static explicit operator XjbXmlActor(Actor act) {
            return new XjbXmlActor(
                act.Character ?? "",
                act.Name ?? "",
                act.Thumb
            );
        }

        public static explicit operator MoviePerson(Actor act) {
            return new MoviePerson(act);
        }
    }
}