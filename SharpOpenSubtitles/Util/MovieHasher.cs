﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Frost.SharpOpenSubtitles.Util {
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

        public static byte[] GetMovieHash(this Stream input) {
            return ComputeMovieHash(input);
        }

        public static string GetMovieHashAsHexString(this Stream input) {
            return ToHexString(ComputeMovieHash(input));
        }

        public static Task<byte[]> GetMovieHashAsync(this Stream input) {
            return Task.Run(() => ComputeMovieHash(input));
        }

        public static Task<string> GetMovieHashAsHexStringAsync(this Stream input) {
            return Task.Run(() => ToHexString(ComputeMovieHash(input)));
        }

        #endregion

        public static Task<byte[]> ComputeMovieHashAsync(string fileName) {
            return Task.Run(() => ComputeMovieHash(fileName));
        }

        public static Task<string> ComputeMovieHashAsHexStringAsync(string fileName) {
            return Task.Run(() => ToHexString(ComputeMovieHash(fileName)));
        }

        public static string ComputeMovieHashAsHexString(string fileName) {
            return ToHexString(ComputeMovieHash(fileName));
        }

        private static byte[] ComputeMovieHash(string filename) {
            byte[] result;
            using (Stream input = File.OpenRead(filename)) {
                result = ComputeMovieHash(input);
            }
            return result;
        }

        public static Task<byte[]> ComputeMovieHashAsync(Stream input) {
            return Task.Run(() => ComputeMovieHash(input));
        }

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