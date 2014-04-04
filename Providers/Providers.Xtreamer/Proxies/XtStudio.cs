using Frost.Common.Models.Provider;
using Frost.Common.Proxies;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtStudio : Proxy<XjbPhpMovie>, IStudio {

        public XtStudio(XjbPhpMovie movie) : base(movie) {
        }

        public long Id {
            get { return 0; }
        }

        public string Name {
            get { return Entity.Studio; }
            set {
                Entity.Studio = value;
            }
        }

        public bool this[string propertyName] {
            get {
                return propertyName == "Name";
            }
        }
    }
}
