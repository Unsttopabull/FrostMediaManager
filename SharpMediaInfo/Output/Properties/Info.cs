namespace Frost.MediaInfo.Output.Properties {
    public enum InfoType {
        ScanType,
        ScanOrder,
        PixelAspectRatio,
        DisplayAspectRatio
    }

    public class Info {
        private readonly Media _media;
        private readonly string[] propNames;

        public Info(Media mediaVideo, InfoType type) {
            _media = mediaVideo;

            switch (type) {
                case InfoType.ScanType:
                    propNames = new[] { "", "", "" };
                    break;
                case InfoType.ScanOrder:
                    propNames = new[] { "", "", "" };
                    break;
                case InfoType.PixelAspectRatio:
                    propNames = new[] { "", "", "" };
                    break;
                case InfoType.DisplayAspectRatio:
                    propNames = new[] { "", "", "" };
                    break;
            }
        }

        public string String { get { return _media[propNames[0]]; } }
        public string Original { get { return _media[propNames[1]]; } }
        public string OriginalString { get { return _media[propNames[2]]; } }
    }
}