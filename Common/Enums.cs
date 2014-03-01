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

}
