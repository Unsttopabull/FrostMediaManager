namespace Frost.SharpOpenSubtitles.Models.Movies {

    public class ImdbPerson {

        public ImdbPerson(string imdbId, string name) {
            ImdbId = imdbId.StartsWith("_") ? imdbId.Substring(1) : imdbId;
            Name = name;
        }

        public string ImdbId { get; set; }
        public string Name { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return string.Format("{0} ({1})", Name, ImdbId);
        }
    }

}