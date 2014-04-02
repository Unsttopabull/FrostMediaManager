using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Messages.Subtitles {

    internal class AddSubtitleMessage {

        public AddSubtitleMessage(MovieSubtitle subtitle) {
            Subtitle = subtitle;
        }

        public MovieSubtitle Subtitle { get; set; }
    }

}