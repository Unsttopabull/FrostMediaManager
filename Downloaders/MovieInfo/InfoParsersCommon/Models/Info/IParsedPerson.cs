namespace Frost.InfoParsers.Models.Info {

    public interface IParsedPerson {

        /// <summary>Gets or sets the full name of the person.</summary>
        /// <value>The full name of the person.</value>
        string Name { get; set; }

        /// <summary>Gets or sets the persons thumbnail image.</summary>
        /// <value>The thumbnail image.</value>
        string Thumb { get; set; }

        /// <summary>Gets or sets the Persons imdb identifier.</summary>
        /// <value>The imdb identifier of the person.</value>
        string ImdbID { get; set; }         
    }

}