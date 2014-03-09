namespace Frost.Common.Models {

    public interface ISpecial : IMovieEntity {
        ///<summary>Gets or sets special addithions or types</summary>
        ///<value>Special addithions or types</value> 
        ///<example>\eg{ <c>INTERNAL, DUBBED, LIMITED, PROPER, REPACK, RERIP, SUBBED</c>}</example>
        string Value { get; set; }
    }

}