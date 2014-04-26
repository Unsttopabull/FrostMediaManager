using System.Collections;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class SubtitleHashes : XmlRpcStruct, IEnumerable<SubInfo> {
        //contains members with hashes as names and values as subtitle IDs if found

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<SubInfo> GetEnumerator() {
            IDictionaryEnumerator enumerator = base.GetEnumerator();
            while (enumerator.MoveNext()) {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;

                yield return new SubInfo((string) current.Key, current.Value as string);
            }
        }

        public SubInfo GetBySubHash(string subHash) {
            return ContainsKey(subHash)
                ? new SubInfo(subHash, this[subHash] as string)
                : null;
        }
    }

}