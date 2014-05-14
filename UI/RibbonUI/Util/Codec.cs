using Frost.RibbonUI.Windows.Edit;

namespace Frost.RibbonUI.Util {

    public class Codec {
        /// <summary>Initializes a new instance of the <see cref="EditAudio"/> class.</summary>
        public Codec(string name, string id) {
            Name = name;
            Id = id;
        }

        public string Name { get; set; }
        public string Id { get; set; }
    }

}