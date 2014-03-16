using System.Collections;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.UI {

    public class TranslationInfos : XmlRpcStruct, IEnumerable<TranslationLanguage> {
        //contains members with ISO69 2 letter code name
        //with TranslationLanguage structure

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<TranslationLanguage> GetEnumerator() {
            IDictionaryEnumerator enumerator = base.GetEnumerator();
            while (enumerator.MoveNext()) {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;

                yield return new TranslationLanguage((string) current.Key, (XmlRpcStruct) current.Value);
            }
        }

        public TranslationLanguage GetByISOCode(string isoCode) {
            return ContainsKey(isoCode)
                ? new TranslationLanguage(isoCode, (XmlRpcStruct) this[isoCode])
                : null;
        }
    }

}