using System;

namespace Frost.Common {

    public interface IMovieSaver : IDisposable {
        void Save();
    }

}