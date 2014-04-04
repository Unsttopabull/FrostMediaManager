using Frost.Common;
using Frost.Common.Models.Provider;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public abstract class XtArt : ChangeTrackingProxy<XjbPhpMovie>, IArt {
        protected readonly string XjbPath;

        protected XtArt(XjbPhpMovie entity, string xjbPath) : base(entity) {
            XjbPath = xjbPath;
        }

        public long Id { get { return 0; } }

        /// <summary>Gets or sets the path to this art (can be local or network or an URI).</summary>
        /// <value>The path to this art (can be local or network or an URI).</value>
        public abstract string Path { get; set; }

        /// <summary>Gets or sets the path to the preview of the art (a smaller, lower resolution copy).</summary>
        /// <value>The path to the preview of the art (a smaller, lower resolution copy).</value>
        public string Preview {
            get { return null; }
            set { }
        }

        public abstract ArtType Type { get; }

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
