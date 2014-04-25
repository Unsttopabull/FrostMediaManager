using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SharpTrailerAddictAPI.Models {

    [XmlType(AnonymousType = true)]
    [XmlRoot("trailers", Namespace = "", IsNullable = false)]
    public class Trailers : IEnumerable<Trailer> {

        [XmlElement("trailer")]
        public Trailer[] Trailer { get; set; }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<Trailer> GetEnumerator() {
            return (IEnumerator<Trailer>) Trailer.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return Trailer.GetEnumerator();
        }
    }

}
