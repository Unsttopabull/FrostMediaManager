using System.Collections.Generic;
using System.Text;

namespace Frost.PodnapisiNET {

    public static class Extensions {
        public static string ToHexString(this IEnumerable<byte> hash) {
            StringBuilder sb = new StringBuilder(16);
            foreach (byte b in hash) {
                sb.Append(string.Format("{0:X}", b));
            }
            return sb.ToString();
        }         
    }

}