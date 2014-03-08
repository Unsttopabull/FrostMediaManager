namespace Frost.Common.Models {

    public interface IActor : IPerson, IMovieEntity {
        string Character { get; set; }

        IMovie Movie { get; set; }
    }

    public interface IActor<TMovie, TActor> : IActor, IPerson<TMovie, TActor> where TMovie : IMovie where TActor : IActor {
        
        new TMovie Movie { get; set; }
    }
}