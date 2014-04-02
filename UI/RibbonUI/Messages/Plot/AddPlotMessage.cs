using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.Plot {

    internal class AddPlotMessage {
        public AddPlotMessage(IPlot plot) {
            Plot = plot;
        }

        public IPlot Plot { get; private set; }
    }

}