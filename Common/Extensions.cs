using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using File = Common.Models.DB.MovieVo.Files.File;

namespace Common {

    /// <summary>Contains extension methods to be used in this assembly.</summary>
    public static class Extensions {

        /// <summary>Removes the file extension from the filename.</summary>
        /// <param name="filename">The filename to remove extension from.</param>
        /// <returns>A filename without an extension.</returns>
        public static string WithoutExtension(this string filename) {
            //get the last occurence of '.' character
            int extSeparatorIndex = filename.LastIndexOf('.');

            //if '.' was in string return everthing left of it
            //otherwise return the same filename
            return extSeparatorIndex != -1
                ? filename.Substring(0, extSeparatorIndex)
                : filename;
        }

        /// <summary>Checks if the file is a SAMBA path and converts it to Windows compatible path.</summary>
        /// <param name="fn">The filename to convert to Windows compatible path.</param>
        /// <returns>SAMBA path converted to Windows compatible path. If it starts with <c>"smb://"</c> otherwise returns the same string.</returns>
        public static string ToWinPath(this string fn) {
            if (fn.StartsWith("smb://")) {
                //Win does not recognize samba protocol paths
                //they use double backslash for network paths
                fn = fn.Replace("smb://", @"\\");
            }
            return fn;
        }

        /// <summary>Trims the specified string if not null.</summary>
        /// <param name="str">The string to trim.</param>
        /// <returns>The trimmed string or <c>null</c> if the parameter <c><paramref name="str"/></c> was <c>null</c>.</returns>
        public static string TrimIfNotNull(this string str) {
            return (str != null)
                ? str.Trim()
                : null;
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long StrFormatByteSize(
            long fileSize,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
            int bufferSize);

        /// <summary>Formats the file size in bytes as a pretty printed string.</summary>
        /// <param name="size">The size in bytes.</param>
        /// <param name="capacity">The string length.</param>
        /// <returns>The filesize pretty printed.</returns>
        /// <example>\eg{ <c>1024</c> is <c>1 Kb</c>.}</example>
        public static string FormatFileSizeAsString(this long size, int capacity = 11) {
            StringBuilder sb = new StringBuilder(capacity);
            StrFormatByteSize(size, sb, sb.Capacity);
            return sb.ToString();
        }

        /// <summary>To Culture Invariant string.</summary>
        /// <typeparam name="T">The type of the object from which to get a string represetation from.</typeparam>
        /// <param name="obj">An IConvertible to get a string representation from.</param>
        /// <returns>A string representation of the object in a culture invariant form.</returns>
        public static string ToICString<T>(this T obj) where T : IConvertible {
            return obj.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">The delimiters to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        public static string[] SplitWithoutEmptyEntries(this string str, params string[] delimiters) {
            return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">The delimiters to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        public static string[] SplitWithoutEmptyEntries(this string str, params char[] delimiters) {
            return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>Adds the file info as an instance <see cref="Models.DB.MovieVo.Files.File">File</see> to the collection if the file with specified filename it exists.</summary>
        /// <param name="files">The files collection to add to.</param>
        /// <param name="filename">The filename to check.</param>
        /// <returns>Returns <b>true</b> if the fille exist and there was no error retrieving its info; otherwise <b>false</b>.</returns>
        public static bool AddFile(this ICollection<File> files, string filename) {
            try {
                FileInfo fi = new FileInfo(filename);

                files.Add(new File(fi.Name, fi.Extension, fi.FullName, fi.Length));
            }
            catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Converts the specified <see cref="IEnumerable{TFrom}"/> to the <see cref="HashSet{TTo}"/>
        /// by casting each element from <b>TFrom</b> to <b>TTo</b>.
        /// </summary>
        /// <typeparam name="TTo">The type to convert from.</typeparam>
        /// <typeparam name="TFrom">The type to convert to.</typeparam>
        /// <param name="enumerable">The enumerable to convert.</param>
        /// <returns>A <see cref="HashSet{TTo}"/> with items casted from <b>TFrom</b> to <b>TTo</b>. Elements which fail the cast will be <b>null</b>.</returns>
        /// <remarks>Will throw an exception if the specified cast does not exist.</remarks>
        /// <exception cref="InvalidCastException">Thrown when the specified conversion is not available (eg. the conversion operators are not defined).</exception>
        public static HashSet<TTo> ToHashSet<TTo, TFrom>(this IEnumerable<TFrom> enumerable) where TTo : class {
            return new HashSet<TTo>(enumerable.Select(a => a as TTo));
        }

        /// <summary>Check if strings match character by character ignoring case.</summary>
        /// <param name="lhs">string to compare.</param>
        /// <param name="compareTo">string to compare to.</param>
        /// <returns>Returns <b>true</b> if strings match, <b>false</b> otherwise.</returns>
        public static bool OrdinalEquals(this string lhs, string compareTo) {
            return string.Equals(lhs, compareTo, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Casts an object to the generic type.</summary>
        /// <typeparam name="T">Type to cast to.</typeparam>
        /// <param name="obj">The object to convert.</param>
        /// <returns>An instance of <b>T</b> if cast succeeds.</returns>
        /// <exception cref="InvalidCastException">Thrown when the specified conversion is not available (eg. the conversion operators are not defined).</exception>
        public static T CastAs<T>(this object obj) {
            //Will throw an exception if casting fails
            //(there must be a conversion operator defined for custom types)
            return (T) obj;
        }

        /// <summary>
        /// Converts the specified <see cref="IEnumerable{TFrom}"/> to the <see cref="IEnumerable{TTo}"/>
        /// by casting each element from <b>TFrom</b> to <b>TTo</b>.
        /// </summary>
        /// <typeparam name="TTo">The type to convert from.</typeparam>
        /// <typeparam name="TFrom">The type to convert to.</typeparam>
        /// <param name="enumerable">The enumerable to convert.</param>
        /// <returns>A <see cref="IEnumerable{TTo}"/> with items casted from <b>TFrom</b> to <b>TTo</b>. Elements which fail the cast will be <b>null</b>.</returns>
        /// <remarks>Will throw an exception if the specified cast does not exist.</remarks>
        /// <exception cref="InvalidCastException">Thrown when the specified conversion is not available (eg. the conversion operators are not defined).</exception>
        public static IEnumerable<TTo> ConvertIEnumerable<TTo, TFrom>(this IEnumerable<TFrom> enumerable) where TTo : class {
            return enumerable.Select(e => e as TTo);
        }

    }

}
