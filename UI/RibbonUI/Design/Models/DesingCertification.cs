using Frost.Common.Models.Provider;

namespace Frost.RibbonUI.Design.Models {
    public class DesingCertification : ICertification {

        public DesingCertification(string rating, ICountry country) {
            Rating = rating;
            Country = country;
        }

        /// <summary>Unique identifier.</summary>
        public long Id { get; private set; }

        /// <summary>Gets the value whether the property is editable.</summary>
        /// <value>The <see cref="System.Boolean"/> if the value is editable.</value>
        /// <param name="propertyName">Name of the property to check.</param>
        /// <returns>Returns <c>true</c> if property is editable, otherwise <c>false</c> (Not implemented or read-only).</returns>
        public bool this[string propertyName] {
            get { return true; }
        }

        /// <summary>Gets or sets the rating in the specified county.</summary>
        /// <value>The rating in the specified country.</value>
        public string Rating { get; set; }

        /// <summary>Gets or sets the coutry this certification applies to.</summary>
        /// <value>The coutry this certification applies to.</value>
        public ICountry Country { get; set; }
    }
}
