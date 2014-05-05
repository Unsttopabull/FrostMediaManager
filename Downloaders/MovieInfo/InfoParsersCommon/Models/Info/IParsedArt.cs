namespace Frost.InfoParsers.Models.Info {

    /// <summary>The type of the image.</summary>
    public enum ParsedArtType : long {

        /// <summary>An unknown type of the image.</summary>
        Unknown,

        /// <summary>The image is a cover art.</summary>
        Cover,

        /// <summary>The image is a poster.</summary>
        Poster,

        /// <summary>The image is a fanart (backround / backdrop).</summary>
        Fanart

    }

    public interface IParsedArt {
        string Preview { get; }
        string FullPath { get; }
        ParsedArtType Type { get; }
    }

}