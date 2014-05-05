namespace Frost.InfoParsers.Models.Subtitles {

    public interface ISubtitleUploadInfo {
        string MovieImdbId { get; }
        string LanguageAlpha3 { get; }
        string ReleaseName { get; }
        string Aliases { get; }
        string Comment { get; }

        string SubtitleHash { get; }
        string MovieHash { get; }

        long MovieSizeInBytes { get; }

        long DurationInMs { get; }
        long DurationInFrames { get; }

        double FPS { get; }

        string SubtitleName { get; }

        byte[] SubtitleFile { get; }
    }

}