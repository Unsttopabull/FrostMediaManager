using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using Frost.SharpMediaInfo.Options;

namespace Frost.SharpMediaInfo.Output {

    public class MediaFile : IDisposable {

        private readonly MediaInfo _mi;

        public MediaFile(string filePath, bool cacheInfom, MediaInfo medaInfo = null) {
            _mi = medaInfo ?? new MediaInfo();

            IsOpen = _mi.Open(filePath, cacheInfom);

            Menu = new MediaMenu(this);
            Other = new MediaOther(this);
            Image = new MediaImage(this);
            Audio = new MediaAudio(this);
            Video = new MediaVideo(this);
            Text = new MediaText(this);
            General = new MediaGeneral(this);

            if (cacheInfom) {
                CacheInform();
            }
        }

        #region Media Properties

        public LibraryInfo Info {
            get { return _mi.Info; }
        }

        public Settings Options {
            get { return _mi.Options; }
            set { _mi.Options = value; }
        }

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

        /// <summary>Get all details about a file in one string</summary>
        /// <returns>A string containing all details about a file.</returns>
        public string Inform() {
            return _mi.Inform();
        }

        public void Close() {
            _mi.Close();
            IsOpen = false;
        }

        /// <summary>Gets the XML inform and passes it on then restores the original inform type</summary>
        private void CacheInform() {
            string prevInform = _mi.Options.Inform;

            _mi.Options.InformPreset = InformPreset.XML;
            string inform = _mi.Inform();
            ParseInform(inform);

            _mi.Options.Inform = prevInform;
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

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (!IsOpen) {
                _mi.Dispose();
                GC.SuppressFinalize(this);
                IsOpen = true;
            }
        }

        ~MediaFile() {
            Dispose();
        }

        #endregion

        #region Getters

        /// <summary>Get a piece of information about a file (parameter is an integer)</summary>
        /// <param name="streamKind">Kind of stream (general, video, audio...)</param>
        /// <param name="streamNumber">Stream number in Kind of stream (first, second...)</param>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in string format ("Codec", "Width"...) </param>
        /// <param name="kindOfInfo">Kind of information you want about the parameter (the text, the measure, the help...)</param>
        /// <param name="kindOfSearch">Where to look for the parameter</param>
        /// <returns>a string about information you search, an empty string if there is a problem</returns>
        public string Get(StreamKind streamKind, int streamNumber, string parameter, InfoKind kindOfInfo = InfoKind.Text, InfoKind kindOfSearch = InfoKind.Name) {
            if (_mi.MustUseAnsi) {
                IntPtr parameterPtr = Marshal.StringToHGlobalAnsi(parameter);
                string toReturn = Marshal.PtrToStringAnsi(MediaInfo.MediaInfoA_Get(_mi.Handle, (IntPtr) streamKind, (IntPtr) streamNumber, parameterPtr, (IntPtr) kindOfInfo, (IntPtr) kindOfSearch));
                Marshal.FreeHGlobal(parameterPtr);
                return toReturn;
            }
            return Marshal.PtrToStringUni(MediaInfo.MediaInfo_Get(_mi.Handle, (IntPtr) streamKind, (IntPtr) streamNumber, parameter, (IntPtr) kindOfInfo, (IntPtr) kindOfSearch));
        }

        /// <summary>Get a piece of information about a file (parameter is an integer)</summary>
        /// <param name="streamKind">Kind of stream (general, video, audio...)</param>
        /// <param name="streamNumber">Stream number in Kind of stream (first, second...)</param>
        /// <param name="parameter">Parameter you are looking for in the stream (Codec, width, bitrate...), in integer format (first parameter, second parameter...)</param>
        /// <param name="kindOfInfo">Kind of information you want about the parameter (the text, the measure, the help...)</param>
        /// <returns>a string about information you search, an empty string if there is a problem</returns>
        public string Get(StreamKind streamKind, int streamNumber, int parameter, InfoKind kindOfInfo = InfoKind.Text) {
            return _mi.MustUseAnsi
                ? Marshal.PtrToStringAnsi(MediaInfo.MediaInfoA_GetI(_mi.Handle, (IntPtr) streamKind, (IntPtr) streamNumber, (IntPtr) parameter, (IntPtr) kindOfInfo))
                : Marshal.PtrToStringUni(MediaInfo.MediaInfo_GetI(_mi.Handle, (IntPtr) streamKind, (IntPtr) streamNumber, (IntPtr) parameter, (IntPtr) kindOfInfo));
        }

        /// <summary>Count of streams of a stream kind or count of piece of information in this stream.</summary>
        /// <param name="streamKind">Kind of stream (general, video, audio...)</param>
        /// <param name="streamNumber">Stream number in this kind of stream (first, second...)</param>
        /// <returns></returns>
        public int CountGet(StreamKind streamKind, int streamNumber = -1) {
            return (int) MediaInfo.MediaInfo_Count_Get(_mi.Handle, (IntPtr) streamKind, (IntPtr) streamNumber);
        }

        #endregion
    }

}
