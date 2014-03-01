using System.Collections.Generic;
using Frost.Models.Xtreamer.DB;

namespace Frost.Models.Frost.DB {

    public partial class Genre {

        private static readonly Dictionary<string, string> GenreTags;
        private static readonly Dictionary<string, string> GenreAbbreviations;

        static Genre() {
            GenreAbbreviations = new Dictionary<string, string>(27){
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

            GenreTags = new Dictionary<string, string>(136) {
                {"action", "acti"},
                {"adventure", "adve"},
                {"animation", "anim"},
                {"biography", "biog"},
                {"comedy", "come"},
                {"crime", "crim"},
                {"disaster", "disa"},
                {"documentary", "docu"},
                {"drama", "dram"},
                {"family", "fami"},
                {"fantasy", "fant"},
                {"film-noir", "noir"},
                {"history", "hist"},
                {"horror", "horr"},
                {"music", "musi"},
                {"musical", "musl"},
                {"mystery", "myst"},
                {"reality-tv", "real"},
                {"romance", "roma"},
                {"sci-fi", "scif"},
                {"science fiction", "scif"},
                {"sport", "spor"},
                {"talk-show", "talk"},
                {"thriller", "thri"},
                {"war", "war"},
                {"western", "west"},
                {"akcja", "acti"},
                {"przygodowy", "adve"},
                {"animowany", "anim"},
                {"biograficzny", "biog"},
                {"komedia", "come"},
                {"kryminał", "crim"},
                {"dokumentalny", "docu"},
                {"dramat", "dram"},
                {"familijny", "fami"},
                {"historyczny", "hist"},
                {"muzyczny", "musi"},
                {"romans", "roma"},
                {"sportowy", "spor"},
                {"액션", "acti"},
                {"어드벤처", "adve"},
                {"애니메이션", "anim"},
                {"전기", "biog"},
                {"코미디", "come"},
                {"범죄", "crim"},
                {"재앙", "disa"},
                {"다큐멘터리", "docu"},
                {"드라마", "dram"},
                {"가족", "fami"},
                {"공상", "fant"},
                {"역사", "hist"},
                {"공포", "horr"},
                {"음악", "musi"},
                {"뮤지컬", "musl"},
                {"신비", "myst"},
                {"로맨스", "roma"},
                {"공상 과학 소설", "scif"},
                {"스포츠", "spor"},
                {"스릴러", "thri"},
                {"서부", "west"},
                {"abenteuer", "adve"},
                {"amateur", "adul"},
                {"erotik", "adul"},
                {"sex", "adul"},
                {"hardcore", "adul"},
                {"anime", "anim"},
                {"biographie", "biog"},
                {"komödie", "come"},
                {"krimi", "crim"},
                {"katastrophen", "disa"},
                {"dokumentation", "docu"},
                {"mondo", "docu"},
                {"tierfilm", "docu"},
                {"eastern", "east"},
                {"kampfsport", "east"},
                {"experimentalfilm", "expe"},
                {"kinder-/familienfilm", "fami"},
                {"historienfilm", "hist"},
                {"heimatfilm", "home"},
                {"grusel", "horr"},
                {"splatter", "horr"},
                {"musikfilm", "musi"},
                {"liebe/romantik", "roma"},
                {"science-fiction", "scif"},
                {"kurzfilm", "shor"},
                {"sportfilm", "spor"},
                {"trash", "tras"},
                {"krieg", "war"},
                {"erotiek", "adul"},
                {"aventure", "adve"},
                {"biopic", "biog"},
                {"comédie", "come"},
                {"documentaire", "docu"},
                {"drame", "dram"},
                {"comédie dramatique", "dram"},
                {"arts martiaux", "east"},
                {"famille", "fami"},
                {"fantastique", "fant"},
                {"film noir", "noir"},
                {"historique", "hist"},
                {"epouvante-horreur", "horr"},
                {"comédie musicale", "musl"},
                {"mystère", "myst"},
                {"informations", "news"},
                {"tv réalité", "real"},
                {"espionnage", "thri"},
                {"policier", "thri"},
                {"guerre", "war"},
                {"actie", "acti"},
                {"avontuur", "adve"},
                {"animatie", "anim"},
                {"biografie", "biog"},
                {"komedie", "come"},
                {"misdaad", "crim"},
                {"familie", "fami"},
                {"historisch", "hist"},
                {"musiek", "musi"},
                {"mysterie", "myst"},
                {"romantiek", "roma"},
                {"oorlog", "war"},
                {"azione", "acti"},
                {"avventura", "adve"},
                {"animazione", "anim"},
                {"biografia", "biog"},
                {"commedia", "come"},
                {"crimine", "crim"},
                {"disastri", "disa"},
                {"documentario", "docu"},
                {"drammatico", "dram"},
                {"famiglia", "fami"},
                {"storico", "hist"},
                {"musica", "musi"},
                {"giallo", "myst"},
                {"romanzo", "roma"},
                {"fantascienza", "scif"},
                {"guerra", "war"}
            };
        }

        /// <summary>Converts a <see cref="XjbGenre"/> instance to an instance of <see cref="Genre">Genre</see>.</summary>
        /// <param name="genre">The genre to convert.</param>
        /// <returns>An instance of <see cref="Genre">Genre</see> converted from <see cref="XjbGenre"/>.</returns>
        public static explicit operator Genre(XjbGenre genre) {
            return GenreAbbreviations.ContainsKey(genre.Name)
                    ? new Genre(GenreTags[genre.Name])
                    : new Genre(genre.Name);
        }

        /// <summary>Converts an Xtreamer Movie Jukebox genre abbreviation to the genre name.</summary>
        /// <param name="genreAbbreviation">Genre abbreviation to convert.</param>
        /// <returns>Returns a <see cref="Genre"/> instance converted from Xtreamer Movie Jukebox genre abbreviation or <c>null</c> if the abbreviation is unknown.</returns>
        public static Genre FromGenreAbbreviation(string genreAbbreviation) {
            string genreName;
            GenreAbbreviations.TryGetValue(genreAbbreviation, out genreName);

            return !string.IsNullOrEmpty(genreName)
                ? genreName
                : null;
        }
    }

}