using System.Collections.Generic;
using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {
    public class ParsedArts : IParsedArts {

        public IEnumerable<IParsedArt> Covers { get; set; }
        public IEnumerable<IParsedArt> Fanart { get; set; }
    }

}
