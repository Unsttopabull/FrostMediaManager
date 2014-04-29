using System;

namespace Frost.InfoParsers.Models {

    public interface IInfoClient {
        bool IsImdbSupported { get; }
        bool IsTmdbSupported { get; }
        bool IsTitleSupported { get; }

        string Name { get; }
        Uri Icon { get; }        
    }

}