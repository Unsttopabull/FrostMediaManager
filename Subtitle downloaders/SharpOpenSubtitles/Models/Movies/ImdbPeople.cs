using System.Collections;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Movies {

    public class ImdbPeople : XmlRpcStruct, IEnumerable<ImdbPerson> {
        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<ImdbPerson> GetEnumerator() {
            IDictionaryEnumerator enumerator = base.GetEnumerator();
            while (enumerator.MoveNext()) {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;

                yield return new ImdbPerson((string) current.Key, current.Value as string);
            }
        }

        public ImdbPerson GetByImdbId(string imdbId) {
            string key = "_" + imdbId;
            return ContainsKey(key)
                       ? new ImdbPerson(imdbId, this[imdbId] as string)
                       : null;
        }
    }

}