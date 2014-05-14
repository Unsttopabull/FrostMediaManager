using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Frost.Common.Util {
    /// <summary>Simple movie hasher for movie file indentification</summary>
    public static class MovieHasher {

        #region Extensions

        /// <summary>Converts a byte array hash to its string representation.</summary>
        /// <param name="hash">The hash as byte array.</param>
        /// <returns>A string representation of a byte array hash where each byte is represented in hexadecimal.</returns>
        public static string ToHexString(this IEnumerable<byte> hash) {
            StringBuilder sb = new StringBuilder(16);
            foreach (byte b in hash) {
                sb.Append(string.Format("{0:x}", b));
            }
            return sb.ToString();
        }

        /// <summary>Gets the movie hash from the stream.</summary>
        /// <param name="input">The input stream from which to calculate the hash.</param>
        /// <returns>The OpenSubtitle.org MovieHash as a byte array.</returns>
        public static byte[] GetMovieHash(this Stream input) {
            return ComputeMovieHash(input);
        }

        /// <summary>Gets the movie hash from the stream as a hexadecimal string digest.</summary>
        /// <param name="input">The input stream from which to calculate the hash.</param>
        /// <returns>The OpenSubtitle.org MovieHash as hexadecimal string digest.</returns>
        public static string GetMovieHashAsHexString(this Stream input) {
            return ToHexString(ComputeMovieHash(input));
        }

        #endregion

        /// <summary>Computes the movie hash and converts it as hexadecimal string.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Returns the hexadecimal representation of the computed hash.</returns>
        public static string ComputeMovieHashAsHexString(string fileName) {
            return ToHexString(ComputeMovieHash(fileName));
        }

        /// <summary>Gets the movie hash from the file.</summary>
        /// <param name="filename">The path to the file for which to calcluate the movie hash.</param>
        /// <returns>The OpenSubtitle.org MovieHash as a byte array.</returns>
        private static byte[] ComputeMovieHash(string filename) {
            byte[] result;
            using (Stream input = File.OpenRead(filename)) {
                result = ComputeMovieHash(input);
            }
            return result;
        }

        /// <summary>Gets the movie hash from the stream.</summary>
        /// <param name="input">The input stream from which to calculate the hash.</param>
        /// <returns>The OpenSubtitle.org MovieHash as a byte array.</returns>
        public static byte[] ComputeMovieHash(Stream input) {
            long streamsize = input.Length;
            ulong lhash = (ulong) streamsize;

            long i = 0;
            byte[] buffer = new byte[sizeof(long)];
            input.Position = 0;
            while (i < 65536/sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0)) {
                i++;
                unchecked {
                    lhash += BitConverter.ToUInt64(buffer, 0);
                }
            }

            input.Position = Math.Max(0, streamsize - 65536);
            i = 0;
            while (i < 65536/sizeof(long) && (input.Read(buffer, 0, sizeof(long)) > 0)) {
                i++;
                unchecked {
                    lhash += BitConverter.ToUInt64(buffer, 0);
                }
            }

            byte[] result = BitConverter.GetBytes(lhash);
            Array.Reverse(result);

            return result;
        }
    }
}