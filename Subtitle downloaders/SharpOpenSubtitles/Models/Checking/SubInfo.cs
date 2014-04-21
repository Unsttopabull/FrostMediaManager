namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class SubInfo {

        public string SubHash { get; set; }
        public int? ID { get; set; }

        public SubInfo(string subHash, string info) {
            SubHash = subHash;

            int id;
            if (int.TryParse(info, out id)) {
                ID = id;
            }
            else {
                ID = null;
            }
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return SubHash;
        }
    }

}