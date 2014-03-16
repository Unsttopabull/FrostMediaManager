using Frost.Common.Models;

namespace RibbonUI.Messages.People {

    internal class AddActorMessage {

        public AddActorMessage(IActor actor) {
            Actor = actor;
        }

        public IActor Actor { get; set; }
    }

}