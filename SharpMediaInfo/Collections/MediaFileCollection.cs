using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Linq;

namespace Frost.SharpMediaInfo.Collections {

    internal class MediaFileCollection : KeyedCollection<string, MediaListFile> {

        #region Get

        /// <summary>When implemented in a derived class, extracts the key from the specified element.</summary>
        /// <returns>The key for the specified element.</returns>
        /// <param name="item">The element from which to extract the key.</param>
        protected override string GetKeyForItem(MediaListFile item) {
            return item.General.FileInfo.FullPath;
        }

        public MediaListFile GetFirstFileWithPattern(Regex regex) {
            return Dictionary.FirstOrDefault(kvp => regex.IsMatch(kvp.Key)).Value;
        }

        /// <summary>Gets the file paths of files in the list.</summary>
        /// <returns>Filepaths of files in the list.</returns>
        public IEnumerable<string> GetFilePaths() {
            return Dictionary.Keys;
        }

        public IEnumerable<MediaListFile> GetFilesWithPattern(Regex regex) {
            return Dictionary.Where(kvp => regex.IsMatch(kvp.Key))
                             .Select(kvp => kvp.Value);
        }

        #endregion

        public void Close(int filePos) {
            this[filePos].Close(); //Dispose the file

            RemoveAt(filePos);

            for (int i = filePos; i < Count; i++) {
                this[i].FileIndex--;
            }
        }
    }

}