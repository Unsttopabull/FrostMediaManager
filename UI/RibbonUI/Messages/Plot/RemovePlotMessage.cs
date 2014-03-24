using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.Plot {
    public class RemovePlotMessage {

        public RemovePlotMessage(IPlot plot) {
            Plot = plot;
        }

        public IPlot Plot { get; set; }
    }
}
