using System;

namespace Frost.MovieInfoParsers {
    public static class Extensions {

        public static bool ContainsIgnoreCase(this string s, string value) {
            return s.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
