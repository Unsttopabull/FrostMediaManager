namespace Frost.InfoParsers.Models.Subtitles {

    public interface ISubtitleFile {
        string MD5Hash { get; }

        string ID { get; }
    }

}