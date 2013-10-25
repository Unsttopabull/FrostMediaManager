using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Common.Models.DB.MovieVo;
using Common.Models.XML.XBMC;

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

        public static Audio[] ToAudioArray(this XbmcXmlAudioInfo[] arr) {
            return ConvertArray<Audio, XbmcXmlAudioInfo>(arr);
        }

        public static Video[] ToVideoArray(this XbmcXmlVideoInfo[] arr) {
            return ConvertArray<Video, XbmcXmlVideoInfo>(arr);
        }

        public static Subtitle[] ToSubtitleArray(this XbmcXmlSubtitleInfo[] arr) {
            return ConvertArray<Subtitle, XbmcXmlSubtitleInfo>(arr);
        }

        public static Certification[] ToCertificationArray(this XbmcXmlCertification[] arr) {
            return ConvertArray<Certification, XbmcXmlCertification>(arr);
        }

        public static T CastAs<T>(this object obj) {
            //Will throw an exception if casting fails
            //(there must be a conversion operator defined for custom types)
            return (T) obj; 
        }

        private static TTo[] ConvertArray<TTo, TFrom>(this TFrom[] inArr) {
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
