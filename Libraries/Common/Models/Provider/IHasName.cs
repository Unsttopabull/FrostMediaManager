namespace Frost.Common.Models.Provider {

/// <summary>A base interface for objects that have name information.</summary>
    public interface IHasName {

        /// <summary>Gets or sets name.</summary>
        /// <value>The name of the entity.</value>
        string Name { get; set; } 
    }

}