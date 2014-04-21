using System.Threading.Tasks;

namespace Frost.MovieInfoParsers {

    public interface IParsedMovie<T> {
        Task<T> ParseMovieInfo();
    }

}