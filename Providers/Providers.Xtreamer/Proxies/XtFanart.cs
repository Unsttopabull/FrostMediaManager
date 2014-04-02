using System.Collections.Generic;
using Frost.Common;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtFanart : XtArt {
        private readonly int _index;

        public XtFanart(XjbPhpMovie movie, int index, string xjbPath) : base(movie, xjbPath) {
            _index = index;

            OriginalValues = new Dictionary<string, object> {
                { "Path", Path }
            };
        }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public override string Path {
            get {
                if (Entity.Fanart != null && Entity.Fanart.Length >= _index) {
                    return XjbPath + Entity.Fanart[_index];
                }
                return null;
            }
            set {
                if (Entity.Fanart != null && Entity.Fanart.Length >= _index) {
                    Entity.Fanart[_index] = value;
                    TrackChanges(value);
                }
            }
        }

        public override ArtType Type {
            get { return ArtType.Fanart; }
        }
    }

}