using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using File = Common.Models.DB.MovieVo.File;

namespace Common {
    public enum DBSystem {
        Xtreamer,
        XBMC,
        Cache
    }

    public enum XmlSystem {
        Xtreamer,
        XBMC
    }

    public static class Extensions {
        public static string TraceSql<T>(this IQueryable<T> query) {
            return ((ObjectQuery<T>) query).ToTraceString();
        }

        public static string WithoutExtension(this string filename) {
            return filename.Substring(0, filename.LastIndexOf('.'));
        }

        public static string ToWinPath(this string fn) {
            if (fn.StartsWith("smb://")) {
                //Win does not recognize samba protocol paths
                //they use double backslash for network paths
                fn = fn.Replace("smb://", @"\\");
            }
            return fn;
        }

        public static string TrimIfNotNull(this string str) {
            return (str != null)
                        ? str.Trim()
                        : null;
        }

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern long StrFormatByteSize(
                long fileSize,
                [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer,
                int bufferSize);

        public static string FormatFileSizeAsString(this long size) {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(size, sb, sb.Capacity);
            return sb.ToString();
        }

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

        public static HashSet<T> ToHashSet<T, T2>(this IEnumerable<T2> enumerable) where T : class {
            return new HashSet<T>(enumerable.Select(a => a as T));
        }

        /// <summary>Check if strings match character by character ignoring case</summary>
        /// <param name="lhs">string to compare</param>
        /// <param name="rhs">string to compare with</param>
        /// <returns></returns>
        public static bool OrdinalEquals(this string lhs, string rhs) {
            return string.Equals(lhs, rhs, StringComparison.OrdinalIgnoreCase);
        }

        public static T CastAs<T>(this object obj) {
            //Will throw an exception if casting fails
            //(there must be a conversion operator defined for custom types)
            return (T) obj; 
        }

        public static TTo[] ConvertArray<TTo, TFrom>(this TFrom[] inArr) {
            if (inArr == null) {
                return null;
            }

            int numAudios = inArr.Length;
            TTo[] outArr = new TTo[numAudios];

            for (int i = 0; i < numAudios; i++) {
                outArr[i] = inArr[i].CastAs<TTo>();
            }
            return outArr;          
        }
    }
}
