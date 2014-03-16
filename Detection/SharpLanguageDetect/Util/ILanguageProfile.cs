using System.Collections.Generic;

namespace Frost.SharpLanguageDetect.Util {

    public interface ILanguageProfile {
        Dictionary<string, int> Frequency { get; }
        string Name { get; }

        int[] NWords { get; }
    }

}