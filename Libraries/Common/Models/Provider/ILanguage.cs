using Frost.Common.Models.Provider.ISO;

namespace Frost.Common.Models.Provider {

    public interface ILanguage : IMovieEntity {

        /// <summary>Gets or sets the name of this language.</summary>
        /// <value>The name of this language.</value>
        string Name { get; set; }

        /// <summary>Gets or sets the ISO639 language codes.</summary>
        /// <value>The ISO639 language codes.</value>
        ISO639 ISO639 { get; set; }
    }

}