using Frost.Common.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Messages.People {

    internal class RemoveActorMessage {

        public RemoveActorMessage(IActor actor) {
            Actor = actor;
        }

        public IActor Actor { get; set; }
    }

}