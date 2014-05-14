namespace Frost.Common {

    /// <summary>Represents the known promotional video clip type</summary>
    public enum PromotionalVideoType {
        /// <summary>Unknown type or Other</summary>
        Unknown,
        /// <summary>Clip is a movie trailer</summary>
        Trailer,
        /// <summary>Clip is an interview about the movie</summary>
        Interview,
        /// <summary>Clip is a movie featurette</summary>
        Featurete,
        /// <summary>Clip is about behind the scenes action.</summary>
        BehindTheScenes,
        /// <summary>Clip is TvSpot or advert for the movie.</summary>
        TvSpot,
        /// <summary>Clip is a review of a movie.</summary>
        Review,
        /// <summary>Clip is a movie fragment or other.</summary>
        Clip
    }

    /// <summary>The type of the media stream or file.</summary>
    public enum MediaType {

        /// <summary>An audio stream.</summary>
        Audio,

        /// <summary>A video stream.</summary>
        Video,

        /// <summary>A subtitles stream.</summary>
        Subtitles

    }

    /// <summary>The type of the database system.</summary>
    public enum DBSystem {

        /// <summary>The Xtreamer Movie Jukebox database.</summary>
        Xtreamer,

        /// <summary>The XBMC Movie database.</summary>
        XBMC,

        /// <summary>The Frost Media Manager cache database.</summary>
        Frost

    }

    /// <summary>The compression mode used.</summary>
    public enum CompressionMode : long {
        /// <summary>An unknown compression mode.</summary>
        Unknown,
        /// <summary>The compression mode that allows perfect reconsturction to an original.</summary>
        Lossless,
        /// <summary>The compression mode that only aproximates the original and the compression can't be reverted.</summary>
        Lossy
    }

    /// <summary>The bitrate mode that was used when ecoding.</summary>
    public enum FrameOrBitRateMode : long {

        /// <summary>The bitrate mode is unknown.</summary>
        Unknown,

        /// <summary>The constant/static bit rate.</summary>
        Constant,

        /// <summary>The bitrate that has more bits for complex segments and less for less complex ones. Bitrate therefore varies (is not constant).</summary>
        Variable

    }

    /// <summary>Represents the DVD Region this movie is set to or originates from.</summary>
    public enum DVDRegion {
        /// <summary>Unknown region</summary>
        Unknown,
        /// <summary>Region 0</summary>
        R0,
        /// <summary>Region 0</summary>
        R1,
        /// <summary>Region 2</summary>
        R2,
        /// <summary>Region 3</summary>
        R3,
        /// <summary>Region 4</summary>
        R4,
        /// <summary>Region 5</summary>
        R5,
        /// <summary>Region 6</summary>
        R6,
        /// <summary>Region 7</summary>
        R7,
        /// <summary>Region 8</summary>
        R8,
    }

    /// <summary>Represents the type of the movie</summary>
    public enum MovieType {
        /// <summary>Unknown type or Other</summary>
        Unknown,
        /// <summary>Movie is a DVD</summary>
        DVD,
        /// <summary>Movie is a BluRay</summary>
        BluRay,
        /// <summary>Movie is a BluRay</summary>
        HDDVD,
        /// <summary>Movie packed into an ISO image file</summary>
        ISO
    }

    /// <summary>The way of displaying/drawing video frames on the screen.</summary>
    public enum ScanType : long {
        /// <summary>An Unknown scan type.</summary>
        Unknown,

        /// <summary>The intelaced scan type. Has two fields (odd and even rows). Each frame only one is shown (they are alternating).</summary>
        /// <remarks>Has to be deinterlaced on non CRT/Plasma screens.</remarks>
        Interlaced,

        /// <summary>The progressive (noninterlaced) scan type. All rows of the drawn for each frame.</summary>
        /// <remarks>Supported on all screens.</remarks>
        Progressive,

        /// <summary>The MBAFF Scan Type</summary>
        MBAFF,

        /// <summary>The mixed Scan Type</summary>
        Mixed
    }

    /// <summary>The type of the image.</summary>
    public enum ArtType : long {

        /// <summary>An unknown type of the image.</summary>
        Unknown,

        /// <summary>The image is a cover art.</summary>
        Cover,

        /// <summary>The image is a poster.</summary>
        Poster,

        /// <summary>The image is a fanart (backround / backdrop).</summary>
        Fanart

    }
}
