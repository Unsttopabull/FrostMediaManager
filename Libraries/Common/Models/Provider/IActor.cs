namespace Frost.Common.Models.Provider {

    /// <summary>Represents the information about an actor that is accessible by the UI.</summary>
    public interface IActor : IPerson {

        /// <summary>Gets the character or role of the actor.</summary>
        /// <value>The character or role of the actor.</value>
        string Character { get; set; }
    }
}