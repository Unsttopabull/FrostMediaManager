namespace Frost.InfoParsers.Models {

    public interface IParsedArt {
        string Preview { get; }
        string FullUrl { get; }
        string Type { get; }
    }

}