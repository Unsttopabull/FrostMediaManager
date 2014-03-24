using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.People {

    internal class RemoveDirectorMessage {

        public RemoveDirectorMessage(IPerson director) {
            Director = director;
        }

        public IPerson Director { get; set; }

    }

}