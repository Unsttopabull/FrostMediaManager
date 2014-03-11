using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Messages.Subtitles {
    public class RemoveSubtitleMessage {

        public RemoveSubtitleMessage(MovieSubtitle subtitle) {
            Subtitle = subtitle;
        }

        public MovieSubtitle Subtitle { get; private set; }
    }
}
