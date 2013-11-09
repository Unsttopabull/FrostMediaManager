namespace Frost.MediaInfo.Output.Properties {
    public class LanguageInfo {
        private readonly Media _media;

        public LanguageInfo(Media media) {
            _media = media;
        }

        /// <summary>Language name (full)</summary>
        public string String { get { return _media[""]; } }

        /// <summary>Language name (full)</summary>
        public string String1 { get { return _media[""]; } }

        /// <summary>Language (2-letter ISO 639-1 if exists, else empty)</summary>
        public string String2 { get { return _media[""]; } }

        /// <summary>Language (3-letter ISO 639-2 if exists, else empty)</summary>
        public string String3 { get { return _media[""]; } }

        /// <summary>Language (2-letter ISO 639-1 if exists with optional ISO 3166-1 country separated by a dash if available, e.g. en, en-us, zh-cn, else empty)</summary>
        public string String4 { get { return _media[""]; } }

        /// <summary>More info about Language (e.g. Director's Comment)</summary>
        public string More { get { return _media[""]; } }
    }
}