using System.Collections.Generic;
using Frost.Common.Models.Provider;

namespace Frost.Common.Models {

    public interface IMovieInfo {
        string Title { get; set; }

        long? ReleaseYear { get; }

        string ImdbID { get; }

        string TmdbID { get; }

        IEnumerable<string> MovieHashes { get; }


        void AddArt(IArt art, bool silent = false);
        void AddPromotionalVideo(IPromotionalVideo video, bool silent = false);

    }

}