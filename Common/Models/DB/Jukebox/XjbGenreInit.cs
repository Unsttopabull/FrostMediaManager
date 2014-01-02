using System.Collections.Generic;

namespace Frost.Common.Models.DB.Jukebox {

    public partial class XjbGenre {
        private static readonly Dictionary<string, string> GenreTags;

        static XjbGenre() {
            GenreTags = new Dictionary<string, string>(186){
                {"acti", "action"},
                {"adve", "adventure"},
                {"anim", "animation"},
                {"biog", "biography"},
                {"come", "comedy"},
                {"crim", "crime"},
                {"disa", "disaster"},
                {"docu", "documentary"},
                {"dram", "drama"},
                {"fami", "family"},
                {"fant", "fantasy"},
                {"noir", "film-noir"},
                {"hist", "history"},
                {"horr", "horror"},
                {"musi", "music"},
                {"musl", "musical"},
                {"myst", "mystery"},
                {"real", "reality-tv"},
                {"roma", "romance"},
                {"scif", "sci-fi"},
                {"spor", "sport"},
                {"talk", "talk-show"},
                {"thri", "thriller"},
                {"war", "war"},
                {"west", "western"},
            };
        }
    }

}