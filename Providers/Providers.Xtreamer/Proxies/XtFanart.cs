using Frost.Common;
using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtFanart : IArt {
        private readonly XjbPhpMovie _movie;
        private readonly string _xjbPath;
        private readonly int _index;

        public XtFanart(XjbPhpMovie movie, int index, string xjbPath) {
            _movie = movie;
            _index = index;
            _xjbPath = xjbPath;
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path {
            get {
                if (_movie.Fanart != null && _movie.Fanart.Length >= _index) {
                    return _xjbPath + _movie.Fanart[_index];
                }
                return null;
            }
            set {
                if (_movie.Fanart != null && _movie.Fanart.Length >= _index) {
                    _movie.Fanart[_index] = value;
                }
            }
        }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview {
            get { return null; }
            set { }
        }

        public ArtType Type {
            get { return ArtType.Fanart; }
        }

        public string PreviewOrPath {
            get { return Path; }
        }

        public bool this[string propertyName] {
            get {
                switch (propertyName) {
                    case "Path":
                    case "Type":
                    case "PreviewOrPath":
                        return true;
                    default:
                        return false;
                }
            }
        }
    }
}
