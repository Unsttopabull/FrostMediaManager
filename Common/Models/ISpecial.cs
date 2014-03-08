using System.Collections.Generic;

namespace Frost.Common.Models {

    public interface ISpecial : IMovieEntity {
        ///<summary>Gets or sets special addithions or types</summary>
        ///<value>Special addithions or types</value> 
        ///<example>\eg{ <c>INTERNAL, DUBBED, LIMITED, PROPER, REPACK, RERIP, SUBBED</c>}</example>
        string Value { get; set; }

        /// <summary>Gets or sets the movies that this special applies to</summary>
        /// <value>The movies this special applies to.</value>
        ICollection<IMovie> Movies { get; }
    }

    public interface ISpecial<TMovie> : ISpecial where TMovie : IMovie {

        /// <summary>Gets or sets the movies that this special applies to</summary>
        /// <value>The movies this special applies to.</value>
        new ICollection<TMovie> Movies { get; }        
    }

}