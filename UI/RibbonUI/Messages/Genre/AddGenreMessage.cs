using Frost.Common.Models;

namespace RibbonUI.Messages.Genre {

    internal class AddGenreMessage {

        public AddGenreMessage(IGenre genre) {
            Genre = genre;
        }

        public IGenre Genre { get; set; }
    }

}