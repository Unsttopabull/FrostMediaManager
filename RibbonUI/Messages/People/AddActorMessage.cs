using Frost.Common.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Messages.People {

    internal class AddActorMessage {

        public AddActorMessage(IActor actor) {
            Actor = actor;
        }

        public IActor Actor { get; set; }
    }

}