using Frost.PHPtoNET.Attributes;

namespace Frost.Providers.Xtreamer.PHP {

    [PHPName("Coretis_VO_Genre")]
    public class XjbPhpGenre {

        /// <summary>Initializes a new instance of the <see cref="XjbPhpGenre"/> class.</summary>
        public XjbPhpGenre() {
        }

        /// <summary>Initializes a new instance of the <see cref="XjbPhpGenre"/> class.</summary>
        /// <param name="name">The genre name.</param>
        public XjbPhpGenre(string name) {
            Name = name;
        }

        /// <summary>Initializes a new instance of the <see cref="XjbPhpGenre"/> class.</summary>
        /// <param name="id">The database identifier.</param>
        /// <param name="name">The genre name.</param>
        public XjbPhpGenre(int id, string name) : this(name) {
            Id = id;
        }

        /// <summary>The id for this row in DB</summary>
        /// <remarks>id in DB</remarks>
        [PHPName("id")]
        public int Id { get; set; }

        /// <summary>Gets or sets the name of the genre</summary>
        /// <value>The name of the genre</value>
        /// <example>\eg{ ''<c>horror</c>'', ''<c>comedy</c>''}</example>
        [PHPName("name")]
        public string Name { get; set; }
    }

}
