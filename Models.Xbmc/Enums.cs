namespace Frost.Providers.Xbmc {


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