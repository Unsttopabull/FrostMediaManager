using Frost.Common.Models;

namespace RibbonUI.Messages.Plot {
    public class RemovePlotMessage {

        public RemovePlotMessage(IPlot plot) {
            Plot = plot;
        }

        public IPlot Plot { get; set; }
    }
}
