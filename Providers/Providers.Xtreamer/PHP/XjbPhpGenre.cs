using System;
using Frost.Common.Models.Provider;
using Frost.PHPtoNET.Attributes;

namespace Frost.Providers.Xtreamer.PHP {

    [PHPName("Coretis_VO_Genre")]
    public class XjbPhpGenre : IEquatable<IGenre> {

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

        public XjbPhpGenre(IGenre genre) {
            Name = genre.Name;
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

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(IGenre other) {
            if (other == null) {
                return false;
            }
            if (Id != 0 && other.Id != 0) {
                return other.Id == Id;
            }

            return string.Compare(Name, other.Name, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        public static bool operator ==(XjbPhpGenre phpGenre, IGenre genre) {
            if (ReferenceEquals(null, phpGenre) && ReferenceEquals(null, genre)) {
                return true;
            }
            if (ReferenceEquals(null, phpGenre) || ReferenceEquals(null, genre)) {
                return false;
            }

            return phpGenre.Equals(genre);
        }

        public static bool operator !=(XjbPhpGenre phpGenre, IGenre genre) {
            return !(phpGenre == genre);
        }
    }

}
