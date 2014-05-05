using System.Collections.Generic;
using Frost.InfoParsers.Models.Art;
using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers {
    public class ParsedArts : IParsedArts {

        public IEnumerable<IParsedArt> Covers { get; set; }
        public IEnumerable<IParsedArt> Fanart { get; set; }
    }

}
