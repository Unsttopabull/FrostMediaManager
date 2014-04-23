using Frost.Common.Models.Provider;

namespace RibbonUI.Design.Models {
    class DesignActor : DesignPerson, IActor {

        public DesignActor() {
        }

        public DesignActor(string name, string character) {
            Name = name;
            Character = character;
        }

        public string Character { get; set; }
    }
}
