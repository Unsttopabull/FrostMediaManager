using System.Collections;
using System.Collections.Generic;
using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Checking {

    public class MovieResults : XmlRpcStruct, IEnumerable<MovieInfo> {
        //contains many MovieInfo members with video file hash as names;

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
        public new IEnumerator<MovieInfo> GetEnumerator() {
            IDictionaryEnumerator enumerator = base.GetEnumerator();
            while (enumerator.MoveNext()) {
                DictionaryEntry current = (DictionaryEntry) enumerator.Current;

                yield return new MovieInfo(current.Key as string, current.Value as XmlRpcStruct);
            }
        }

        public MovieInfo GetByMovieHash(string movieHash) {
            return ContainsKey(movieHash)
                ? new MovieInfo(movieHash, (XmlRpcStruct) this[movieHash])
                : null;
        }
    }

}