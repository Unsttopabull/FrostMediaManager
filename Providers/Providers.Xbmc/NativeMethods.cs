using System;
using System.Runtime.InteropServices;

namespace Frost.Providers.Xbmc {

    public static class NativeMethods {
        private const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        private const uint GENERIC_READ = 0x80000000;
        private const uint FILE_SHARE_READ = 0x1;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;
        private const int INVALID_HANDLE_VALUE = -1;
        private const uint OPEN_EXISTING = 3;

        [StructLayout(LayoutKind.Sequential)]
        private struct FILETIME {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr SecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetFileTime(
            IntPtr hFile,
            IntPtr lpCreationTime,
            IntPtr lpLastAccessTime,
            ref FILETIME lpLastWriteTime
            );

        [DllImport("kernel32.dll")]
        private static extern bool FileTimeToLocalFileTime([In] ref FILETIME lpFileTime, out FILETIME lpLocalFileTime);

        public static long GetLocalLastWriteFileTime(string fileName) {
            IntPtr ptr = IntPtr.Zero;
            FILETIME ftLastWriteTime = new FILETIME();
            try {
                ptr = CreateFile(fileName, GENERIC_READ, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL | FILE_FLAG_BACKUP_SEMANTICS, IntPtr.Zero);
                if (ptr.ToInt32() == INVALID_HANDLE_VALUE) {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
                if (GetFileTime(ptr, IntPtr.Zero, IntPtr.Zero, ref ftLastWriteTime) != true) {
                    Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
                }
                FILETIME ftLocalWriteTime;
                FileTimeToLocalFileTime(ref ftLastWriteTime, out ftLocalWriteTime);

                return (((long) ftLocalWriteTime.dwHighDateTime) << 32) | ((uint) ftLocalWriteTime.dwLowDateTime);
            }
            catch (Exception e) {
                throw (e);
            }
            finally {
                if (ptr != IntPtr.Zero && ptr.ToInt32() != INVALID_HANDLE_VALUE) {
                    CloseHandle(ptr);
                }
            }
        }
    }

}