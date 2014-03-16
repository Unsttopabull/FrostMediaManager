using Frost.Common.Models;

namespace RibbonUI.Messages.Plot {

    internal class AddPlotMessage {
        public AddPlotMessage(IPlot plot) {
            Plot = plot;
        }

        public IPlot Plot { get; private set; }
    }

}