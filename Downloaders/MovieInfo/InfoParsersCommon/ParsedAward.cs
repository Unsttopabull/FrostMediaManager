using Frost.InfoParsers.Models;

namespace Frost.InfoParsers {

    public class ParsedAward : IParsedAward {
        public string Organization { get; set; }
        public bool IsNomination { get; set; }
        public string Award { get; set; }
    }

}