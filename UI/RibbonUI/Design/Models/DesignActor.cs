using Frost.Common.Models.Provider;
using Frost.InfoParsers.Models.Info;

namespace Frost.RibbonUI.Design.Models {
    class DesignActor : DesignPerson, IActor {

        public DesignActor() {
        }

        public DesignActor(string name, string character) {
            Name = name;
            Character = character;
        }

        public DesignActor(IParsedActor actor) : this(actor.Name, actor.Character) {
            Thumb = actor.Thumb;
            ImdbID = actor.ImdbID;
        }

        public string Character { get; set; }
    }
}
