using System;
using Frost.Common;

namespace Frost.Providers.Xbmc.DB.Art {
    public class XbmcMovieArt : XbmcArt {

        public XbmcMovieArt() {
        }

        public XbmcMovieArt(ArtType type, string url) {
            switch (type) {
                case ArtType.Fanart:
                    Type = FANART;
                    break;
                case ArtType.Poster:
                case ArtType.Cover:
                    Type = POSTER;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            Url = url;
        }
    }
}
