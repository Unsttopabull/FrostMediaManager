namespace Frost.Common.Models {

    public interface IRating : IMovieEntity {
        /// <summary>Gets or sets the name of the critic.</summary>
        /// <value>The name of the critic.</value>
        string Critic { get; set; }

        /// <summary>Gets or sets the value of the rating.</summary>
        /// <value>The rating value</value>
        double Value { get; set; }

        /// <summary>Gets or sets the movie this rating is for.</summary>
        /// <value>The movie this rating is for.</value>
        IMovie Movie { get; set; }
    }

    public interface IRating<TMovie> : IRating where TMovie : IMovie {
        /// <summary>Gets or sets the movie this rating is for.</summary>
        /// <value>The movie this rating is for.</value>
        new TMovie Movie { get; set; }
    }

}