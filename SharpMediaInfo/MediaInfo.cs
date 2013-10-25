using System;
using System.Runtime.InteropServices;
using SharpMediaInfo.Options;
using SharpMediaInfo.Output;

namespace SharpMediaInfo {
    public class MediaInfo {
        //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        internal readonly IntPtr Handle;
        public readonly bool MustUseAnsi;

        public MediaInfo() {
            Handle = MediaInfo_New();
            Options = new Settings(this);
            Info = new Info(this);

            MustUseAnsi = Environment.OSVersion.ToString().IndexOf("Windows", StringComparison.Ordinal) == -1;
        }

        public Info Info { get; private set; }

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_New();

        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfo_Delete(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open(IntPtr handle, IntPtr fileName);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open_Buffer_Init(IntPtr handle, Int64 fileSize, Int64 fileOffset);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open(IntPtr handle, Int64 fileSize, Int64 fileOffset);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open_Buffer_Continue(IntPtr handle, IntPtr buffer, IntPtr bufferSize);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open_Buffer_Continue(IntPtr handle, Int64 fileSize, byte[] buffer, IntPtr bufferSize);

        [DllImport("MediaInfo.dll")]
        internal static extern Int64 MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern Int64 MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern void MediaInfo_Close(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Inform(IntPtr handle, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Inform(IntPtr handle, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Option(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string option, [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfoA_Option(IntPtr handle, IntPtr option, IntPtr value);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_State_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        internal static extern IntPtr MediaInfo_Count_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber);

        //MediaInfo class
        ~MediaInfo() {
            MediaInfo_Delete(Handle);
        }

        public Settings Options { get; private set; }

        /// <summary>Open a file and collect information about it (technical information and tags)</summary>
        /// <param name="fileName">Full name of the file to open.</param>
        /// <returns>Returns true if sucessfull, otherwise false</returns>
        public MediaFile Open(string fileName) {
            return Open(fileName, false);
        }

        /// <summary>Open a file and collect information about it (technical information and tags)</summary>
        /// <param name="fileName">Full name of the file to open.</param>
        /// <param name="cacheInform">
        /// <para>If should we parse inform data, and return cached values on string indexers and properties when available</para>
        /// <para>so we don't call to the DLL if we don't need to.</para>
        /// </param>
        /// <returns>Returns true if sucessfull, otherwise false</returns>
        public MediaFile Open(string fileName, bool cacheInform) {
            if (MustUseAnsi) {
                IntPtr fileNamePtr = Marshal.StringToHGlobalAnsi(fileName);
                int toReturn = (int)MediaInfoA_Open(Handle, fileNamePtr);
                Marshal.FreeHGlobal(fileNamePtr);

                return toReturn == 1
                               ? new MediaFile(this, cacheInform)
                               : null;
            }
            return ((int)MediaInfo_Open(Handle, fileName)) == 1 ? new MediaFile(this, cacheInform) : null;
        }

        ///// <summary>Open a stream (Init)</summary>
        ///// <param name="fileSize">Estimated file size</param>
        ///// <param name="fileOffset">Offset of the file (if we don't have the beginning of the file)</param>
        ///// <returns></returns>
        //public int OpenBufferInit(Int64 fileSize, Int64 fileOffset) {
        //    return (int) MediaInfo_Open_Buffer_Init(_handle, fileSize, fileOffset);
        //}

        ///// <summary>Open a stream (Continue)</summary>
        ///// <param name="buffer">Pointer to the stream</param>
        ///// <param name="bufferSize">Number of bytes to read.</param>
        ///// <returns></returns>
        //public int OpenBufferContinue(IntPtr buffer, IntPtr bufferSize) {
        //    return (int) MediaInfo_Open_Buffer_Continue(_handle, buffer, bufferSize);
        //}

        ///// <summary>Open a stream (Get the needed file Offset)</summary>
        ///// <returns>the needed offset of the file. File size if no more bytes are needed</returns>
        //public Int64 OpenBufferContinueGoToGet() {
        //    return MediaInfo_Open_Buffer_Continue_GoTo_Get(_handle);
        //}

        ///// <summary>Open a stream (Finalize)</summary>
        ///// <returns></returns>
        //public int OpenBufferFinalize() {
        //    return (int) MediaInfo_Open_Buffer_Finalize(_handle);
        //}

        /// <summary>Close a file opened before with Open() (without saving)</summary>
        public void Close() {
            MediaInfo_Close(Handle);
        }

        /// <summary>Get all details about a file in one string</summary>
        /// <returns>string with all the file details</returns>
        /// <remarks>You can change default presentation with Inform_Set()</remarks>
        public string Inform() {
            return MustUseAnsi
                           ? Marshal.PtrToStringAnsi(MediaInfoA_Inform(Handle, (IntPtr) 0))
                           : Marshal.PtrToStringUni(MediaInfo_Inform(Handle, (IntPtr) 0));
        }

        /// <summary>Configure or get information about MediaInfoLib</summary>
        /// <param name="option">The option.</param>
        /// <param name="value">The value of option</param>
        /// <returns>Depend of the option: by default "" (nothing) means No, other means Yes</returns>
        public string Option(string option, string value) {
            if (MustUseAnsi) {
                IntPtr optionPtr = Marshal.StringToHGlobalAnsi(option);
                IntPtr valuePtr = Marshal.StringToHGlobalAnsi(value);
                string toReturn = Marshal.PtrToStringAnsi(MediaInfoA_Option(Handle, optionPtr, valuePtr));
                Marshal.FreeHGlobal(optionPtr);
                Marshal.FreeHGlobal(valuePtr);
                return toReturn;
            }
            return Marshal.PtrToStringUni(MediaInfo_Option(Handle, option, value));
        }

        /// <summary>Configure or get information about MediaInfoLib</summary>
        /// <param name="option">The option.</param>
        /// <returns>Depend of the option: by default "" (nothing) means No, other means Yes</returns>
        public string Option(string option) {
            return Option(option, "");
        }

        /// <summary>(NOT IMPLEMENTED YET) Get the state of the library</summary>
        /// <returns></returns>
        public int StateGet() {
            return (int) MediaInfo_State_Get(Handle);
        }


    }
}