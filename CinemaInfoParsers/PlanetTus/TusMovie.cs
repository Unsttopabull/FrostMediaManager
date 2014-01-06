using System;
using System.Threading.Tasks;

namespace Frost.CinemaInfoParsers.PlanetTus {

    [Serializable]
    public class TusMovie : ICinemaMovie {

        public TusMovie() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="TusMovie"/> class.</summary>
        public TusMovie(string originalName, string sloveneName, string url) {
            OriginalName = originalName;
            SloveneName = sloveneName;
            Url = url;

            MovieInfo = new TusMovieInfo();
        }

        public Task ParseMovieInfo() {
            return !string.IsNullOrEmpty(Url)
                ? MovieInfo.ParseMoviePage(Url)
                : new Task(() => { });
        }

        public bool MovieInfoAvailable { get { return MovieInfo.IsFinished; } }

        public string OriginalName { get; private set; }

        public string SloveneName { get; private set; }

        public string Url { get; private set; }

        public TusMovieInfo MovieInfo { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} ({1})", OriginalName, SloveneName);
        }
    }

}
