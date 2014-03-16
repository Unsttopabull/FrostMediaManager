using Frost.Common.Models;

namespace RibbonUI.Messages.Studio {

    internal class AddStudioMessage {

        public AddStudioMessage(IStudio studio) {
            Studio = studio;
        }

        public IStudio Studio { get; set; }
    }

}