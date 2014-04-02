namespace Frost.Common.Models.Provider {

    /// <summary>Represents an interface for required base movie entity properties.</summary>
    public interface IMovieEntity {

        /// <summary>Unique identifier.</summary>
        long Id { get; }

        /// <summary>Gets the value whether the property is editable.</summary>
        /// <value>The <see cref="System.Boolean"/> if the value is editable.</value>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <returns>Returns <c>true</c> if property is editable, otherwise <c>false</c> (Not implemented or read-only).</returns>
        bool this[string propertyName] { get; }
    }

}