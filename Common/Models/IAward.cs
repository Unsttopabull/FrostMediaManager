
namespace Frost.Common.Models {

    public interface IAward : IMovieEntity {

        string Organization { get; set; }

        bool IsNomination { get; set; }

        string AwardType { get; set; }

    }
}