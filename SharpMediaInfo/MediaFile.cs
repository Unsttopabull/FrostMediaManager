using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Frost.SharpMediaInfo.Options;
using Frost.SharpMediaInfo.Output;

namespace Frost.SharpMediaInfo {

    public class MediaFile : IDisposable {

        private readonly IntPtr _handle;
        private readonly bool _mustUseAnsi;

        public MediaFile(string filePath, bool cacheInfom, bool allInfoInform = true) {
            _handle = MediaInfo_New();
            _mustUseAnsi = Environment.OSVersion.ToString().IndexOf("Windows", StringComparison.Ordinal) == -1;            

            Info = new LibraryInfo(this);
            Options = new Settings(this);

            IsOpen = Open(filePath);

            Menu = new MediaMenu(this);
            Other = new MediaOther(this);
            Image = new MediaImage(this);
            Audio = new MediaAudio(this);
            Video = new MediaVideo(this);
            Text = new MediaText(this);
            General = new MediaGeneral(this);

            if (cacheInfom) {
                CacheInform(allInfoInform);
            }
        }

        #region P/Invoke C funtions

        //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_New();

        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfo_Delete(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Open(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Open(IntPtr handle, IntPtr fileName);

        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfo_Close(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Inform(IntPtr handle, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Inform(IntPtr handle, IntPtr reserved);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Option(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string option, [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoA_Option(IntPtr handle, IntPtr option, IntPtr value);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_State_Get(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfo_Count_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber);

        #endregion

        #region Options & Info
        public LibraryInfo Info { get; private set; }

        public Settings Options { get; private set; }
        #endregion

        #region Media Properties

        /// <summary>General info about the media file</summary>
        public MediaGeneral General { get; private set; }

        /// <summary>Info about subtitles and other text elements</summary>
        public MediaText Text { get; private set; }

        /// <summary>Info about video format, codec, compression ...</summary>
        public MediaVideo Video { get; private set; }

        /// <summary>Info about audio format, codec, channels, positions</summary>
        public MediaAudio Audio { get; private set; }

        /// <summary>Info about image resolution, format, width, height, ...</summary>
        public MediaImage Image { get; private set; }

        /// <summary>Other info that couldn't be put in other categories</summary>
        public MediaOther Other { get; private set; }

        /// <summary>Info about menues and chapters</summary>
        public MediaMenu Menu { get; private set; }

        #endregion

        /// <summary>Gets the XML inform and passes it on then restores the original inform type</summary>
        private void CacheInform(bool allInfoInform) {
            string prevInform = Options.Inform;

            bool showAllInfo = false;
            if (allInfoInform) {
                showAllInfo = Options.ShowAllInfo;
                Options.ShowAllInfo = true;
            }

            Options.InformPreset = InformPreset.XML;

            ParseInform(Inform());

            if (allInfoInform) {
                Options.ShowAllInfo = showAllInfo;
            }
            Options.Inform = prevInform;
        }

        private void ParseInform(string inform) {
            IEnumerable<XNode> xNodes = ((XElement) XDocument.Load(new StringReader(inform)).FirstNode).Nodes();
            int[] stevci = new int[7];

            foreach (XElement track in xNodes) {
                XAttribute trackType = track.FirstAttribute;
                if (trackType == null) {
                    continue;
                }

                string type = trackType.Value;

                StreamKind streamKind;
                if (!Enum.TryParse(type, true, out streamKind)) {
                    continue;
                }

                switch (streamKind) {
                    case StreamKind.General:
                        General.ParseInform(track, stevci[(int) StreamKind.General]++);
                        break;
                    case StreamKind.Video:
                        Video.ParseInform(track, stevci[(int) StreamKind.Video]++);
                        break;
                    case StreamKind.Audio:
                        Audio.ParseInform(track, stevci[(int) StreamKind.Audio]++);
                        break;
                    case StreamKind.Text:
                        Text.ParseInform(track, stevci[(int) StreamKind.Text]++);
                        break;
                    case StreamKind.Other:
                        Other.ParseInform(track, stevci[(int) StreamKind.Other]++);
                        break;
                    case StreamKind.Image:
                        Image.ParseInform(track, stevci[(int) StreamKind.Image]++);
                        break;
                    case StreamKind.Menu:
                        Menu.ParseInform(track, stevci[(int) StreamKind.Menu]++);
                        break;
                }
            }
        }

        #region IDisposable

        public bool IsOpen { get; private set; }

        public void Close() {
            if (!IsOpen) {
                MediaInfo_Close(_handle);
                MediaInfo_Delete(_handle);
                GC.SuppressFinalize(this);
                IsOpen = false;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void IDisposable.Dispose() {
            Close();
        }

        ~MediaFile() {
            Close();
        }

        #endregion

        #region MediaInfo Interop

        /// <summary>Open a file and collect information about it (technical information and tags)</summary>
        /// <param name="fileName">Full name of the file to open.</param>
        /// <returns>Returns true if sucessfull, otherwise false</returns>
        private bool Open(string fileName) {
            if (_mustUseAnsi) {
                IntPtr fileNamePtr = Marshal.StringToHGlobalAnsi(fileName);
                int toReturn = (int)MediaInfoA_Open(_handle, fileNamePtr);
                Marshal.FreeHGlobal(fileNamePtr);

                return toReturn == 1;
            }
            return (int)MediaInfo_Open(_handle, fileName) == 1;
        }

        /// <summary>Get all details about a file in one string</summary>
        /// <returns>string with all the file details</returns>
        /// <remarks>You can change default presentation with Inform_Set()</remarks>
        public string Inform() {
            return _mustUseAnsi
                ? Marshal.PtrToStringAnsi(MediaInfoA_Inform(_handle, (IntPtr)0))
                : Marshal.PtrToStringUni(MediaInfo_Inform(_handle, (IntPtr)0));
        }

        /// <summary>Configure or get information about MediaInfoLib</summary>
        /// <param name="option">The option.</param>
        /// <param name="value">The value of option</param>
        /// <returns>Depend of the option: by default "" (nothing) means No, other means Yes</returns>
        internal string Option(string option, string value = "") {
            if (_mustUseAnsi) {
                IntPtr optionPtr = Marshal.StringToHGlobalAnsi(option);
                IntPtr valuePtr = Marshal.StringToHGlobalAnsi(value);

                string toReturn = Marshal.PtrToStringAnsi(MediaInfoA_Option(_handle, optionPtr, valuePtr));

                Marshal.FreeHGlobal(optionPtr);
                Marshal.FreeHGlobal(valuePtr);
                return toReturn;
            }
            return Marshal.PtrToStringUni(MediaInfo_Option(_handle, option, value));
        }

        /// <summary>Get the state of the library (NOT IMPLEMENTED YET).</summary>
        /// <returns>The state of the library.</returns>
        public int StateGet() {
            return (int)MediaInfo_State_Get(_handle);
        }

        /// <summary>Get a piece of information about a file (parameter is an integer)</summary>
        /// <param name="streamKind">Kind of stream (general, video, audio...)</param>
        /// <param name="streamNumber">Stream number in Kind of stream (first, second...)</param>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in string format ("Codec", "Width"...) </param>
        /// <param name="kindOfInfo">Kind of information you want about the parameter (the text, the measure, the help...)</param>
        /// <param name="kindOfSearch">Where to look for the parameter</param>
        /// <returns>a string about information you search, an empty string if there is a problem</returns>
        public string Get(StreamKind streamKind, int streamNumber, string parameter, InfoKind kindOfInfo = InfoKind.Text, InfoKind kindOfSearch = InfoKind.Name) {
            if (_mustUseAnsi) {
                IntPtr parameterPtr = Marshal.StringToHGlobalAnsi(parameter);
                string toReturn = Marshal.PtrToStringAnsi(MediaInfoA_Get(_handle, (IntPtr)streamKind, (IntPtr)streamNumber, parameterPtr, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
                Marshal.FreeHGlobal(parameterPtr);
                return toReturn;
            }
            return Marshal.PtrToStringUni(MediaInfo_Get(_handle, (IntPtr)streamKind, (IntPtr)streamNumber, parameter, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
        }

        /// <summary>Get a piece of information about a file (parameter is an integer)</summary>
        /// <param name="streamKind">Kind of stream (general, video, audio...)</param>
        /// <param name="streamNumber">Stream number in Kind of stream (first, second...)</param>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in integer format (first parameter, second parameter...)</param>
        /// <param name="kindOfInfo">Kind of information you want about the parameter (the text, the measure, the help...)</param>
        /// <returns>a string about information you search, an empty string if there is a problem</returns>
        public string Get(StreamKind streamKind, int streamNumber, int parameter, InfoKind kindOfInfo = InfoKind.Text) {
            return _mustUseAnsi
                ? Marshal.PtrToStringAnsi(MediaInfoA_GetI(_handle, (IntPtr)streamKind, (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo))
                : Marshal.PtrToStringUni(MediaInfo_GetI(_handle, (IntPtr)streamKind, (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo));
        }

        /// <summary>Count of streams of a stream kind or count of piece of information in this stream.</summary>
        /// <param name="streamKind">Kind of stream (general, video, audio...)</param>
        /// <param name="streamNumber">Stream number in this kind of stream (first, second...)</param>
        /// <returns></returns>
        public int CountGet(StreamKind streamKind, int streamNumber = -1) {
            return (int)MediaInfo_Count_Get(_handle, (IntPtr)streamKind, (IntPtr)streamNumber);
        }
        #endregion
    }

}
