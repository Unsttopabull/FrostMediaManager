
namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about an award that is accessible by the UI.</summary>
    public interface IAward : IMovieEntity {

        /// <summary>Gets or sets the organization that awards this award (e.g Oscars).</summary>
        /// <value>The organization that awards this award (e.g Oscars)</value>
        string Organization { get; set; }

        /// <summary>Gets or sets a value indicating whether this award is a nomination.</summary>
        /// <value><c>true</c> if this award is a nomination only; otherwise, <c>false</c>.</value>
        bool IsNomination { get; set; }

        /// <summary>Gets or sets the award name or detail (eg. Best leading male role).</summary>
        /// <value>The award name or detail (eg. Best leading male role).</value>
        string AwardType { get; set; }

    }
}