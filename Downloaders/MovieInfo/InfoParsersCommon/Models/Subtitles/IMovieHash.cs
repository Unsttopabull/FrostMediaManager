namespace Frost.InfoParsers.Models.Subtitles {

    public interface IMovieHash {
        string MovieHashDigest { get; } 
        long FileByteSize { get; }
    }

}