using System;

namespace Frost.Tester {

    public interface IMovieSaver : IDisposable {
        void Save();
    }

}