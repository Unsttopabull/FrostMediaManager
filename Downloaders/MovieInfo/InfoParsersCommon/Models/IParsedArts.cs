using System.Collections.Generic;

namespace Frost.InfoParsers.Models {

    public interface IParsedArts {
        IEnumerable<IParsedArt> Covers { get; }
        IEnumerable<IParsedArt> Fanart { get; }
    }

}