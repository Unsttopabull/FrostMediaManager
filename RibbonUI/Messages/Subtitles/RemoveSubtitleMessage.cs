using Frost.Common.Models;

namespace RibbonUI.Messages.Subtitles {
    public class RemoveSubtitleMessage {

        public RemoveSubtitleMessage(ISubtitle subtitle) {
            Subtitle = subtitle;
        }

        public ISubtitle Subtitle { get; private set; }
    }
}
