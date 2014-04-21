using System.Threading.Tasks;

namespace Frost.InfoParsers.Models {

    public interface IParsedMovie {

        bool MovieInfoAvailable { get; }

        string OriginalName { get;  }

        string SloveneName { get; }

        string Url { get; }

        IParsedMovieInfo MovieInfo { get;  }

        Task<IParsedMovieInfo> ParseMovieInfo();
    }
}