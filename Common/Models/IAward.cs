using System.Collections.Generic;

namespace Frost.Common.Models {

    public interface IAward : IMovieEntity {
        string Organization { get; set; }
        bool IsNomination { get; set; }
        string AwardType { get; set; }
        ICollection<IMovie> Movies { get; }
    }

    public interface IAward<TMovie> : IAward where TMovie : IMovie {
        new ICollection<TMovie> Movies { get; }
    }

}