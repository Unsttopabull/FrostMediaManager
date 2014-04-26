using System;
using System.Globalization;

namespace Frost.InfoParsers {
    public static class Extensions {

        public static bool ContainsIgnoreCase(this string s, string value) {
            return s.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        public static int? TryParseInt(this string str) {
            int value;
            if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out value)) {
                return value;
            }
            return null;
        }
    }
}
