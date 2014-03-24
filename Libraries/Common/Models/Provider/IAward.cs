
namespace Frost.Common.Models.Provider {

    public interface IAward : IMovieEntity {

        string Organization { get; set; }

        bool IsNomination { get; set; }

        string AwardType { get; set; }

    }
}