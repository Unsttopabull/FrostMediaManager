using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.Studio {

    internal class RemoveStudioMessage {
        public RemoveStudioMessage(IStudio studio) {
            Studio = studio;
        }
        public IStudio Studio { get; set; }
    }

}