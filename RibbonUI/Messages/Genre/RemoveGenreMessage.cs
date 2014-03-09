using Frost.Common.Models;

namespace RibbonUI.ViewModels.UserControls {

    internal class RemoveGenreMessage {

        public RemoveGenreMessage(IGenre genre) {
            Genre = genre;
        }

        public IGenre Genre { get; set; }
    }

}