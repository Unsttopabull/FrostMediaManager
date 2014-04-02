using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.People {

    internal class AddDirectorMessage {

        public AddDirectorMessage(IPerson director) {
            Director = director;
        }

        public IPerson Director { get; set; }
    }

}