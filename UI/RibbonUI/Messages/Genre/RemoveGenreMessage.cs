using Frost.Common.Models;
using Frost.Common.Models.Provider;

namespace RibbonUI.Messages.Genre {

    internal class RemoveGenreMessage {

        public RemoveGenreMessage(IGenre genre) {
            Genre = genre;
        }

        public IGenre Genre { get; set; }
    }

}