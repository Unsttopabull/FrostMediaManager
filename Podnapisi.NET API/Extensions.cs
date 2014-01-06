using System.Collections.Generic;
using System.Text;

namespace Frost.PodnapisiNET {

    public static class Extensions {

        /// <summary>Converts a byte array hash to its string representation.</summary>
        /// <param name="hash">The hash as byte array.</param>
        /// <returns>A string representation of a byte array hash where each byte is represented in hexadecimal.</returns>
        public static string ToHexString(this IEnumerable<byte> hash) {
            StringBuilder sb = new StringBuilder(16);
            foreach (byte b in hash) {
                sb.Append(string.Format("{0:x2}", b));
            }
            return sb.ToString();
        }         
    }

}