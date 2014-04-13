using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Frost.Common.Util;

namespace Frost.Common {

    /// <summary>Contains extension methods to be used in this assembly.</summary>
    public static class Extensions {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        /// <summary>Converts the date &amp; time to a UNIX timestamp (number of seconds since 1.1.1970)</summary>
        /// <param name="dt">The <see cref="DateTime"/> to convert.</param>
        /// <returns>Returns the number of seconds since 1. 1. 1970. </returns>
        public static long ToUnixTimestamp(this DateTime dt) {
            return (long) dt.Subtract(Epoch).TotalSeconds;
        }

        /// <summary>Gets the culture invariant representation of a nullable <see cref="IConvertible"/> or <c>null</c> if the <see cref="IConvertible"/> has no value.</summary>
        /// <typeparam name="T">The type of the <see cref="IConvertible"/></typeparam>
        /// <param name="nullable">The nullable to convert.</param>
        /// <returns>Returns the culture invariant representation of a nullable <see cref="IConvertible"/> or <c>null</c> if the <see cref="IConvertible"/> has no value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToInvariantString<T>(this T? nullable) where T : struct, IConvertible {
            return nullable.HasValue ? nullable.Value.ToString(CultureInfo.InvariantCulture) : null;
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
            int bufferSize
        );

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

        /// <summary>Converts the argument to a culture invariant string.</summary>
        /// <typeparam name="T">The type of the object from which to get a string represetation from.</typeparam>
        /// <param name="obj">An IConvertible to get a string representation from.</param>
        /// <returns>A string representation of the object in a culture invariant form.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToInvariantString<T>(this T obj) where T : IConvertible {
            return obj.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">The delimiters to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] SplitWithoutEmptyEntries(this string str, params string[] delimiters) {
            return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiter">The delimiter to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] SplitWithoutEmptyEntries(this string str, string delimiter) {
            return str.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">The delimiters to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> SplitWithoutEmptyEntries(this string str, IEnumerable<string> delimiters) {
            return str.Split(delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">The delimiters to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> SplitWithoutEmptyEntries(this string str, IEnumerable<char> delimiters) {
            return str.Split(delimiters.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>Splits the string on specified delimiters and removes empty entires.</summary>
        /// <param name="str">The string to split.</param>
        /// <param name="delimiters">The delimiters to split on.</param>
        /// <returns>Array of strings that resulted in the split without empty entries.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] SplitWithoutEmptyEntries(this string str, params char[] delimiters) {
            return str.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<TTo> ToHashSet<TTo, TFrom>(this IEnumerable<TFrom> enumerable) where TTo : class {
            return new HashSet<TTo>(enumerable.Select(a => a as TTo));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ObservableHashSet<TTo> ToObservableHashSet<TTo, TFrom>(this IEnumerable<TFrom> enumerable) where TTo : class {
            return new ObservableHashSet<TTo>(enumerable.Select(a => a as TTo));
        }

        /// <summary>Check if strings match character by character ignoring case.</summary>
        /// <param name="lhs">string to compare.</param>
        /// <param name="compareTo">string to compare to.</param>
        /// <returns>Returns <b>true</b> if strings match, <b>false</b> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OrdinalEquals(this string lhs, string compareTo) {
            return String.Equals(lhs, compareTo, StringComparison.OrdinalIgnoreCase);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TTo> ConvertIEnumerable<TTo, TFrom>(this IEnumerable<TFrom> enumerable) where TTo : class {
            return enumerable.Select(e => e as TTo);
        }
    }

}
