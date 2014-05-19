using System.Collections.Generic;
using Frost.InfoParsers.Models.Info;

namespace Frost.InfoParsers.Models.Art {

    public interface IParsedArts {
        IEnumerable<IParsedArt> Covers { get; }
        IEnumerable<IParsedArt> Fanart { get; }
    }

}