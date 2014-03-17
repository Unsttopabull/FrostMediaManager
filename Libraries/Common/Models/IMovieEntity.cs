namespace Frost.Common.Models {

    public interface IMovieEntity {

        long Id { get; }

        bool this[string propertyName] { get; }
    }

}