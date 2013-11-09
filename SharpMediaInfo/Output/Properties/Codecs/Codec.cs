﻿namespace Frost.MediaInfo.Output.Properties.Codecs {
    public class Codec {
        protected readonly Media MediaStream;

        public Codec(Media mediaMenu) {
            MediaStream = mediaMenu;
        }

        public string Name { get { return MediaStream["Codec"]; } }
        public string String { get { return MediaStream["Codec/String"]; } }
        public string Info { get { return MediaStream["Codec/Info"]; } }
        public string Url { get { return MediaStream["Codec/Url"]; } }
    }
}