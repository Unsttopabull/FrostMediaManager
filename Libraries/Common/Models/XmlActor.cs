namespace Frost.Common.Models {

    /// <summary>Represents a serialized actor in a movie</summary>
    public interface IXmlActor {

        /// <summary>Gets or sets the actors full name.</summary>
        /// <value>The actors full name.</value>
        string Name { get; set; }

        /// <summary>Gets or sets the role or character the actor is portraying..</summary>
        /// <value>The role or character the actor is portraying.</value>
        string Role { get; set; }

        /// <summary>Gets or sets the actors thumbnail.</summary>
        /// <value>The actors thumbnail (small picture)</value>
        string Thumb { get; set; }
    }
}
