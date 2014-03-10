using Frost.Common.Models.ISO;

namespace Frost.Common.Models {

    public interface ILanguage : IMovieEntity {

        /// <summary>Gets or sets the Id of this language in the database.</summary>
        /// <value>The Id of this language in the database</value>
        long Id { get; set; }

        /// <summary>Gets or sets the name of this language.</summary>
        /// <value>The name of this language.</value>
        string Name { get; set; }

        /// <summary>Gets or sets the ISO639 language codes.</summary>
        /// <value>The ISO639 language codes.</value>
        ISO639 ISO639 { get; set; }
    }

}