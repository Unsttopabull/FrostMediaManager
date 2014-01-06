using System.Threading.Tasks;

namespace Frost.CinemaInfoParsers {

    public interface ICinemaMovie {
        Task ParseMovieInfo();
    }

}