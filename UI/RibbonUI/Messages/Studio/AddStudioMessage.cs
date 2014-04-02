using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.Studio {

    internal class AddStudioMessage {

        public AddStudioMessage(IStudio studio) {
            Studio = studio;
        }

        public IStudio Studio { get; set; }
    }

}