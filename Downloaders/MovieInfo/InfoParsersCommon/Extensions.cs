using System;
using System.Globalization;

namespace Frost.InfoParsers {
    public static class Extensions {

        public static bool ContainsIgnoreCase(this string s, string value) {
            return s.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        /// <summary>Tries to parse a long using an invariant culture. If parsing fails or the string is empty or null returns null.</summary>
        /// <param name="stringVal">The string value to parse.</param>
        /// <returns>Returns the long value or null if the <paramref name="stringVal"/> is null or empty.</returns>
        public static long? TryGetLong(this string stringVal) {
            if (string.IsNullOrEmpty(stringVal)) {
                return null;
            }

            long value;
            if (long.TryParse(stringVal, NumberStyles.Integer, CultureInfo.InvariantCulture, out value)) {
                return value;
            }
            return null;
        }

        /// <summary>Tries to parse an integer using an invariant culture. If parsing fails or the string is empty or null returns null.</summary>
        /// <param name="stringVal">The string value to parse.</param>
        /// <returns>Returns the integer value or null if the <paramref name="stringVal"/> is null or empty.</returns>
        public static int? TryGetInt(this string stringVal) {
            if (string.IsNullOrEmpty(stringVal)) {
                return null;
            }

            int value;
            if (int.TryParse(stringVal, NumberStyles.Integer, CultureInfo.InvariantCulture, out value)) {
                return value;
            }
            return null;
        }

        /// <summary>Tries to parse a double using an invariant culture. If parsing fails or the string is empty or null returns null.</summary>
        /// <param name="stringVal">The string value to parse.</param>
        /// <returns>Returns the double value or null if the <paramref name="stringVal"/> is null or empty.</returns>
        public static double? TryGetDouble(this string stringVal) {
            if (string.IsNullOrEmpty(stringVal)) {
                return null;
            }

            double value;
            if (double.TryParse(stringVal, NumberStyles.Float, CultureInfo.InvariantCulture, out value)) {
                return value;
            }
            return null;
        }

        /// <summary>Tries to parse a boolean using an invariant culture. If parsing fails or the string is empty or null returns null.</summary>
        /// <param name="stringVal">The string value to parse.</param>
        /// <returns>Returns the boolean value or null if the <paramref name="stringVal"/> is null or empty.</returns>
        public static bool? TryGetBool(this string stringVal) {
            if (string.IsNullOrEmpty(stringVal)) {
                return null;
            }

            if (stringVal == "0") {
                return false;
            }

            if (stringVal == "1") {
                return true;
            }

            bool value;
            if (bool.TryParse(stringVal, out value)) {
                return value;
            }
            return null;
        }

        public static string ToInvariantString(this int value) {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
