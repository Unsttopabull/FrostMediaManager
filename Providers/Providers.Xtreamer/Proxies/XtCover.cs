using Frost.Common;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtCover : IArt {
        private readonly XjbPhpMovie _movie;
        private readonly string _xjbPath;

        public XtCover(XjbPhpMovie movie, string xjbPath) {
            _movie = movie;
            _xjbPath = xjbPath;
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public string Path {
            get {
                return string.IsNullOrEmpty(_movie.CoverPath)
                    ? null
                    : _xjbPath + _movie.CoverPath;
            }
            set { _movie.CoverPath = value; }
        }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview {
            get { return null; }
            set { }
        }

        public ArtType Type {
            get { return ArtType.Cover; }
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
