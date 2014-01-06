using System;
using System.Threading.Tasks;

namespace Frost.CinemaInfoParsers.Kolosej {

    [Serializable]
    public class KolosejMovie : ICinemaMovie {

        public KolosejMovie() {
            
        }

        /// <summary>Initializes a new instance of the <see cref="KolosejMovie"/> class.</summary>
        public KolosejMovie(string originalName, string sloveneName, string url) {
            OriginalName = originalName;
            SloveneName = sloveneName;
            Url = url;

            MovieInfo = new KolosejMovieInfo();
        }

        public Task ParseMovieInfo() {
            if (!string.IsNullOrEmpty(Url)) {
                return MovieInfo.ParseMoviePage(Url);
            }
            return new Task(() => { });
        }

        public bool MovieInfoAvailable { get { return MovieInfo.IsFinished; } }

        public string OriginalName { get; private set; }

        public string SloveneName { get; private set; }

        public string Url { get; private set; }

        public KolosejMovieInfo MovieInfo { get; private set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} ({1})", OriginalName, SloveneName);
        }
    }
}
