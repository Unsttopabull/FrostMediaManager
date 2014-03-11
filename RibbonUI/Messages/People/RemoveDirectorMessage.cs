using Frost.Common.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Messages.People {

    internal class RemoveDirectorMessage {

        public RemoveDirectorMessage(IPerson director) {
            Director = director;
        }

        public IPerson Director { get; set; }

    }

}