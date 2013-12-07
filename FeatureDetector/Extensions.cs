using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Frost.DetectFeatures {

    /// <summary>Contains extension methods to be used in this assembly.</summary>
    public static class Extensions {

        /// <summary>Determines whether this string contains the specified value optionaly ignoring the case.</summary>
        /// <param name="str">The string to search in.</param>
        /// <param name="value">The value to check for.</param>
        /// <param name="ignoreCase">If the case of the letters should be igored while searching.</param>
        /// <returns>Is <c>true</c> if the string contains the value; otherwise <c>false</c>.</returns>
        /// <remarks>The search is done by ordinal comparison ignoring the culture info.</remarks>
        public static bool Contains(this string str, string value, bool ignoreCase) {
            return str.IndexOf(value, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) != -1;
        }

        /// <summary>Determines whether this string contains any of the specified values optionaly ignoring the case.</summary>
        /// <param name="str">The string to search in.</param>
        /// <param name="ignoreCase">If the case of the letters should be igored while searching.</param>
        /// <param name="values">The values to check for.</param>
        /// <returns>Is <c>true</c> if the string contains any of the values; otherwise <c>false</c>.</returns>
        /// <remarks>The search is done by ordinal comparison ignoring the culture info.</remarks>
        public static bool ContainsAny(this string str, bool ignoreCase, params string[] values) {
            return values.Any(val => str.Contains(val, ignoreCase));
        }

        /// <summary>Returns an enumerable collection of files information in this directory or any subdirectory.</summary>
        /// <returns>An enumerable collection of files in this directory or any subdirectory.</returns>
        public static IEnumerable<FileInfo> EnumerateFilesRegex(this DirectoryInfo di) {
            return di.EnumerateFiles("*", SearchOption.AllDirectories);            
        }

        /// <summary>Returns an enumerable collection of files information that matches the specified Regex.</summary>
        /// <param name="di">The directory info from which to get the information from.</param>
        /// <param name="regex">The regular expresion to search with.</param>
        /// <param name="searchOption">A bitwise combination of the enumeration values that modify the regular expression.</param>
        /// <returns>An enumerable collection of files that matches the specified Regex.</returns>
        public static IEnumerable<FileInfo> EnumerateFilesRegex(this DirectoryInfo di, string regex, SearchOption searchOption = SearchOption.AllDirectories) {
            Regex pattern = new Regex(regex);
            return di.EnumerateFiles("*", searchOption)
                     .Where(fi => pattern.IsMatch(fi.Name));
        }

        /// <summary>Returns an enumerable collection of files information that matches the specified Regex.</summary>
        /// <param name="di">The directory info from which to get the information from.</param>
        /// <param name="regex">The regular expresion to search with.</param>
        /// <param name="searchOption">A bitwise combination of the enumeration values that modify the regular expression.</param>
        /// <returns>An enumerable collection of files that matches the specified Regex.</returns>
        public static IEnumerable<FileInfo> EnumerateFilesRegex(this DirectoryInfo di, Regex regex, SearchOption searchOption = SearchOption.AllDirectories) {
            return di.EnumerateFiles("*", searchOption)
                     .Where(fi => regex.IsMatch(fi.Name));
        }

        /// <summary>Returns an enumerable collection of files information that matches the specified Regex in this directory or any subdirectory.</summary>
        /// <returns>An enumerable collection of files that matches the specified Regex in this directory or any subdirectory.</returns>
        public static IEnumerable<FileInfo> EnumerateFilesRecursiveByExtension(this DirectoryInfo di, IEnumerable<string> extensions) {
            return di.EnumerateFiles("*", SearchOption.AllDirectories)
                     .Where(fi => extensions.Contains(fi.Extension));
        }
    }

}