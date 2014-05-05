using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers {

    public class ParsedAward : IParsedAward {
        public string Organization { get; set; }
        public bool IsNomination { get; set; }
        public string Award { get; set; }
    }

}