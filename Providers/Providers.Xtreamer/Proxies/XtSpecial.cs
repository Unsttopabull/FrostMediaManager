using System;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {
    public class XtSpecial : ISpecial {
        private readonly XjbPhpMovie _movie;
        private string _special;

        public XtSpecial(XjbPhpMovie movie, string special) {
            _movie = movie;
            _special = special;
        }

        public long Id {
            get { return 0; }
        }

        public string Name {
            get { return _special; }
            set {
                if (value != null && value.Equals(_special, StringComparison.CurrentCultureIgnoreCase)) {
                    return;
                }

                if (value != null) {
                    _movie.RemoveSpecial(_special);
                    _movie.AddSpecial(value);
                }
                else {
                    _movie.RemoveSpecial(_special);
                }
                _special = value;
            }
        }

        public bool this[string propertyName] {
            get { return propertyName == "Name"; }
        }
    }
}
