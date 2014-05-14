namespace Frost.Common.Models.Provider {

    /// <summary>A base interface for objects that have language information.</summary>
    public interface IHasLanguage {

        /// <summary>Gets or sets the language of the item.</summary>
        /// <value>The language of the item.</value>
        ILanguage Language { get; set; } 
    }
}