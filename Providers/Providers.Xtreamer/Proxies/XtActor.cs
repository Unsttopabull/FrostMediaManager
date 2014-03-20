using Frost.Common.Models;
using Frost.Providers.Xtreamer.PHP;

namespace Frost.Providers.Xtreamer.Proxies {

    public class XtActor : XtPerson, IActor {

        public XtActor(XjbPhpPerson person) : base(person) {

        }

        public string Character {
            get { return Person.Character; }
            set { Person.Character = value; }
        }

        public override bool this[string propertyName] {
            get {
                if (propertyName == "Character") {
                    return true;
                }
                return base[propertyName];
            }
        }
    }
}
