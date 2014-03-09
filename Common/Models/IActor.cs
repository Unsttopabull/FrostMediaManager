namespace Frost.Common.Models {

    public interface IActor : IPerson, IMovieEntity {
        string Character { get; set; }

        //IMovie Movie { get; set; }
    }
}