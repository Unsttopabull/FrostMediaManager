using RibbonUI.UserControls;

namespace RibbonUI.Messages {
    public class SelectRibbonMessage {

        public SelectRibbonMessage(RibbonTabs selectRibbon) {
            RibbonTab = selectRibbon;
        }

        public RibbonTabs RibbonTab { get; private set; }
    }
}
