namespace Frost.Common {

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

    /// <summary>The bitrate mode that was used when ecoding.</summary>
    public enum FrameOrBitRateMode : long {

        /// <summary>The bitrate mode is unknown.</summary>
        Unknown,

        /// <summary>The constant/static bit rate.</summary>
        Constant,

        /// <summary>The bitrate that has more bits for complex segments and less for less complex ones. Bitrate therefore varies (is not constant).</summary>
        Variable

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


    /// <summary>The compression mode used.</summary>
    public enum CompressionMode : long {
        /// <summary>An unknown compression mode.</summary>
        Unknown,
        /// <summary>The compression mode that allows perfect reconsturction to an original.</summary>
        Lossless,
        /// <summary>The compression mode that only aproximates the original and the compression can't be reverted.</summary>
        Lossy
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
        Cache

    }

    /// <summary>The xml serialization system.</summary>
    public enum NFOSystem {

        /// <summary>The Xtreamer Movie Jukebox info xml system.</summary>
        Xtreamer,

        /// <summary>The XBMC xml NFO info system.</summary>
        XBMC

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

    /// <summary>The type of the bookmark.</summary>
    public enum BookmarkType : long {

        /// <summary>The standard bookmark.</summary>
        Standard,

        /// <summary>The bookmark is for resuming the video to the position where we stopped the last time.</summary>
        Resume,

        /// <summary>The bookmark is for an episode.</summary>
        Episode

    }

    /// <summary>Represents the XBMC view mode (how the video should be stretched/zoomed).</summary>
    public enum XbmcViewMode : long {

        /// <summary>The default video mode should be used.</summary>
        Normal,

        /// <summary>The video should be zoomed by a specified amount.</summary>
        Zoom,

        /// <summary>The video should be stretched to fit the 4:3 format.</summary>
        Stretch4X3,

        /// <summary>The video should be streched to the closest widescreen format to fill the screen.</summary>
        Widescreen,

        /// <summary>The video should be stretched to fit the 16:9 format.</summary>
        Stretch16X9,

        /// <summary>The video should be played at the original size.</summary>
        OriginalSize,

        /// <summary>The user defined setting should be used.</summary>
        Custom

    }

    /// <summary>The scaling algorithm used when changing the size of the video.</summary>
    public enum XbmcScalingMethod : long {

        /// <remarks>To be used for testing purposes only because of the large amount of artifacts it produces. As a tradeoff it’s the fastest of all.</remarks>
        Nearest = 0,

        /// <summary>Blinear scaling method</summary>
        /// <remarks>Represents a decent quality/speed ratio.</remarks>
        Blinear = 1,

        /// <summary>Bicubic scaling method.</summary>
        Bicubic = 2,

        /// <summary>Lanczos resampling.</summary>
        Lanczos2 = 3,

        /// <summary>Lanczos resampling (4x4).</summary>
        Lanczos3Optimal = 4,

        /// <summary>Lanczos resampling (6x6).</summary>
        Lanczos3 = 5,

        /// <summary>A Sinc8 Zoom algorithm.</summary>
        /// <remarks>This scaler has <b>not</b> yet been implemented.</remarks>
        Sinc8 = 6,

        /// <summary>NEDI (New Edge-Directed Interpolation) algorithm.</summary>
        /// <remarks>This scaler has <b>not</b> yet been implemented.</remarks>
        Nedi = 7,

        /// <remarks>This scaler has <b>not</b> yet been implemented.</remarks>
        BicubicSoftware = 8,

        /// <remarks>This scaler has <b>not</b> yet been implemented.</remarks>
        LanczosSoftware = 9,

        /// <remarks>This scaler has <b>not</b> yet been implemented.</remarks>
        SincSoftware = 10,

        /// <summary>Video Decode and Presentation API for Unix.</summary>
        VdpauHardware = 11,

        /// <summary>DirectX Hardware accelerated scaling method.</summary>
        DxvaHardware = 12,

        /// <summary>Automaticaly chooses a scaling method.</summary>
        Automatic = 13,

        /// <summary>The Spline36 scaler (4x4).</summary>
        Spline36Optimal = 14,

        /// <summary>The Spline36 scaler (6x6).</summary>
        Spline36 = 15,

    }

    /// <summary>The deinterlacing mode used.</summary>
    public enum XbmcDeinterlaceMode : long {

        /// <summary>The video should not be deinterlaced.</summary>
        Disabled,

        /// <summary>XBMC should determine if it should deinterlace the movie.</summary>
        Automatic,

        /// <summary>Force the video to be deinterlaced.</summary>
        Force

    }

}
