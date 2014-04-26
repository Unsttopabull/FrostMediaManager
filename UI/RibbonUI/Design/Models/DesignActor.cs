using Frost.Common.Models.Provider;
using Frost.InfoParsers.Models;

namespace RibbonUI.Design.Models {
    class DesignActor : DesignPerson, IActor {

        public DesignActor() {
        }

        public DesignActor(string name, string character) {
            Name = name;
            Character = character;
        }

        public DesignActor(IParsedActor name) {
            Name = name.Name;
            Thumb = name.Thumb;
            ImdbID = name.ImdbID;
            Character = name.Character;
        }

        public string Character { get; set; }
    }
}
