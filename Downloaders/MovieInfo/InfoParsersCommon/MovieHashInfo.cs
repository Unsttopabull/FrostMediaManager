using Frost.InfoParsers.Models.Subtitles;

namespace Frost.InfoParsers {
    public class MovieHash : IMovieHash {

        /// <summary>Initializes a new instance of the <see cref="T:System.Object"/> class.</summary>
        public MovieHash(string movieHash, long fileByteSize) {
            MovieHashDigest = movieHash;
            FileByteSize = fileByteSize;
        }
        
        public string MovieHashDigest { get; private set; }
        public long FileByteSize { get; private set; }
    }
}
