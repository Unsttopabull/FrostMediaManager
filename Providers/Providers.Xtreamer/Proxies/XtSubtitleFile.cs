using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtSubtitleFile : XtFile {
        private readonly int _index;

        public XtSubtitleFile(XjbPhpMovie movie, int index) : base(movie) {
            _index = index;
        }
    }
}
