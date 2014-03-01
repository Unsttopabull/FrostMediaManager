namespace Frost.Common {

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
        Cache

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

    public enum DVDRegion {
        Unknown,
        R0,
        R1,
        R2,
        R3,
        R4,
        R5,
        R6,
        R7,
        R8,
    }

    public enum MovieType {
        Unknown,
        DVD,
        BluRay,
        HDDVD,
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
        MBAFF,
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
