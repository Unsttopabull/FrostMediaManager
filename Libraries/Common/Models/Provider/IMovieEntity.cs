namespace Frost.Common.Models.Provider {

    public interface IMovieEntity {

        long Id { get; }

        bool this[string propertyName] { get; }
    }

}