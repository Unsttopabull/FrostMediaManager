using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Frost.Common.Models;

namespace Frost.Providers.Xtreamer.DB {

    /// <summary>Represents a Xtreamer Movie Jukebox genre.</summary>
    [Table("genres")]
    public partial class XjbGenre : IEquatable<XjbGenre> {
        private static readonly Dictionary<string, string> GenreAbbreviations;
        private static readonly Dictionary<string, string> GenreTags;

        static XjbGenre() {
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

        /// <summary>Initializes a new instance of the <see cref="XjbGenre"/> class.</summary>
        /// <param name="name">The name of the genre abbreviation.</param>
        public XjbGenre(string name) {
            Name = name;
        }

        internal XjbGenre(IGenre genre) {
            string genreName = genre.Name.ToLower();

            Name = GenreTags.ContainsKey(genreName)
                ? GenreTags[genreName]
                : genre.Name;
        }

        /// <summary>Gets or sets the Id of this option in the database.</summary>
        /// <value>The Id of this option in the database</value>
        [Key]
        [Column("id")]
        public long Id { get; set; }

        /// <summary>Gets or sets the genre 4 letter abbreviation.</summary>
        /// <value>The name of the genre 4 letter abbreviation.</value>
        /// <example>\eg{ <c>"docu"</c> for documentary, <c>"come"</c> for comedy.}</example>
        [Column("name")]
        public string Name { get; set; }

        /// <summary>Gets or sets the movies of this genre in Xtreamer Movie Jukebox library.</summary>
        /// <value>The movies of this genre in Xtreamer Movie Jukebox library</value>
        public virtual HashSet<XjbMovie> Movies { get; set; }

        /// <summary>Converts an Xtreamer Movie Jukebox genre abbreviation to the genre name.</summary>
        /// <param name="genreAbbreviation">Genre abbreviation to convert.</param>
        /// <returns>Returns an english genre name from Xtreamer Movie Jukebox genre abbreviation or <c>null</c> if the abbreviation is unknown.</returns>
        public static string GenreNameFromAbbreviation(string genreAbbreviation) {
            string genreName;
            GenreAbbreviations.TryGetValue(genreAbbreviation, out genreName);

            return !string.IsNullOrEmpty(genreName)
                ? genreName
                : null;
        }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XjbGenre other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return Name == other.Name;
        }

        internal class Configuration : EntityTypeConfiguration<XjbGenre> {

            public Configuration() {
                HasMany(g => g.Movies)
                    .WithMany(m => m.Genres)
                    .Map(m => {
                        m.ToTable("movies_genres");
                        m.MapLeftKey("genre_id");
                        m.MapRightKey("movie_id");
                    });
            }

        }

    }

}
