using System.Collections.Generic;
using Frost.Common;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtCover : XtArt {

        public XtCover(XjbPhpMovie movie, string xjbPath) : base(movie, xjbPath) {
            OriginalValues = new Dictionary<string, object> {
                {"Path", Entity.CoverPath}
            };
        }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public override string Path {
            get {
                return string.IsNullOrEmpty(Entity.CoverPath)
                    ? null
                    : XjbPath + Entity.CoverPath;
            }
            set {
                TrackChanges(value);
                Entity.CoverPath = value;
            }
        }

        public override ArtType Type {
            get { return ArtType.Cover; }
        }
    }
}
