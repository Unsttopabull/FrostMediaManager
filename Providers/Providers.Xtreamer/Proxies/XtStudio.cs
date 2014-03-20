using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtStudio : IStudio {
        private readonly XjbPhpMovie _movie;

        public XtStudio(XjbPhpMovie movie) {
            _movie = movie;
        }

        public long Id {
            get { return 0; }
        }

        public string Name {
            get { return _movie.Studio; }
            set { _movie.Studio = value; }
        }

        public bool this[string propertyName] {
            get {
                return propertyName == "Name";
            }
        }
    }
}
