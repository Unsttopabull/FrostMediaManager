namespace Frost.InfoParsers.Models {

    public interface IParsedVideo {
        VideoType Type { get; set; }
        string Title { get; set; }
        string Url { get; set; }
        string Duration { get; set; }
        string Language { get; set; }
        string SubtitleLanguage { get; set; }
    }

}