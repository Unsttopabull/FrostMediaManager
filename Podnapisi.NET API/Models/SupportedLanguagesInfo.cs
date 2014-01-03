using System.Collections;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET.Models {
    public class SupportedLanguagesInfo : StatusInfo, IEnumerable<SupportedLanguage> {

        [XmlRpcMember("languages")]
        public object[][] Languages;

        public SupportedLanguage[] GetSuppotedLanguages() {
            SupportedLanguage[] arr = new SupportedLanguage[Languages.Length];
            for (int i = 0; i < Languages.Length; i++) {
                arr[i] = this[i];
            }

            return arr;
        }

        private SupportedLanguage this[int i] {
            get {
                return new SupportedLanguage((int)Languages[i][0], (string)Languages[i][1]);
            }
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public IEnumerator<SupportedLanguage> GetEnumerator() {
            for (int i = 0; i < Languages.Length; i++) {
                yield return this[i];
            }
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

}
