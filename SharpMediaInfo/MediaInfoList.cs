using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Frost.SharpMediaInfo {

    public class MediaInfoList : IDisposable, IEnumerable<MediaListFile> {

        private readonly IntPtr _handle;
        private MediaListFile[] _mediaList;
        private bool _isDisposed;
        private readonly int _count;

        #region Constructors
        private MediaInfoList() {
            _handle = MediaInfoList_New();
        }

        public MediaInfoList(string fileOrFolderPath, InfoFileOptions options = InfoFileOptions.Nothing, bool cacheInfom = true, bool allInfoCache = true) : this() {
            _count = Open(fileOrFolderPath, options);

            InitFiles(cacheInfom, allInfoCache);
        }

        public MediaInfoList(IEnumerable<string> filePaths, InfoFileOptions options = InfoFileOptions.Nothing, bool cacheInfom = true, bool allInfoCache = true) : this() {
            //opening multiple files in a single go currently not supported (even if documentation says otherwise)
            foreach (string filePath in filePaths) {
                if (Open(filePath, options) > 0) {
                    _count++;
                }
            }

            InitFiles(cacheInfom, allInfoCache);
        }

        private void InitFiles(bool cacheInfom, bool allInfoCache) {
            _mediaList = new MediaListFile[_count];
            for (int fileNum = 0; fileNum < _count; fileNum++) {
                _mediaList[fileNum] = new MediaListFile(_handle, fileNum, cacheInfom, allInfoCache);
            }
        }

        #endregion

        #region P/Invoke C Functions

        //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoList_Open(IntPtr handle, [MarshalAs(UnmanagedType.LPWStr)] string fileName, IntPtr options);

        [DllImport("MediaInfo.dll")]
        private static extern IntPtr MediaInfoList_New();

        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfoList_Delete(IntPtr handle);

        [DllImport("MediaInfo.dll")]
        private static extern void MediaInfoList_Close(IntPtr handle, IntPtr filePos);

        #endregion

        #region Find files
        public MediaListFile GetFirstFileWithPattern(string regex) {
            return _mediaList.FirstOrDefault(mlf => Regex.IsMatch(mlf.General.FileInfo.FullPath, regex));
        }

        public MediaListFile GetFirstFileWithPattern(Regex regex) {
            return _mediaList.FirstOrDefault(mlf => regex.IsMatch(mlf.General.FileInfo.FullPath));
        }

        public IEnumerable<MediaListFile> GetFilesWithPattern(string regex) {
            return _mediaList.Where(mlf => Regex.IsMatch(mlf.General.FileInfo.FullPath, regex));
        }

        public IEnumerable<MediaListFile> GetFilesWithPattern(Regex regex) {
            return _mediaList.Where(mlf => regex.IsMatch(mlf.General.FileInfo.FullPath));
        }
        #endregion

        #region Properties / Methods

        /// <summary>Gets the file paths of files in the list.</summary>
        /// <returns>Filepaths of files in the list.</returns>
        public IEnumerable<string> GetFilePaths() {
            return _mediaList.Select(ml => ml.General.FileInfo.FullPath);
        }

        /// <summary>Gets the number of files in this list.</summary>
        /// <returns>The number of files in this list.</returns>
        public int Count() {
            return _count;
        }

        /// <summary>Gets a value indicating whether any of the files in the list are still open.</summary>
        /// <value>Is <c>true</c> if any of the files in the list are still open; otherwise, <c>false</c>.</value>
        public bool AnyOpen { get { return NumberOfOpenFiles > 0; } }

        /// <summary>Gets the number of open files in a list.</summary>
        /// <value>The number of open files in a list.</value>
        public int NumberOfOpenFiles {
            get { return _mediaList.Count(mlf => mlf.IsOpen); }
        }
        #endregion

        #region Indexers
        public MediaListFile this[int filePos] {
            get { return _mediaList[filePos]; }
        }

        public MediaListFile this[string fullPath] {
            get { return _mediaList.FirstOrDefault(mlf => mlf.IsOpen && mlf.General.FileInfo.FullPath == fullPath); }
        }

        #endregion

        #region MediaInfo List Interop

        /// <summary>Open one or more files and collect information about them (technical information and tags).</summary>
        /// <param name="pathNames">Full name of file(s) to open or Full name of folder(s) to open (if multiple names, names must be separated by "|").</param>
        /// <param name="options">FileOption_Recursive = Recursive mode for folders FileOption_Close = Close all already opened files before.</param>
        /// <returns>Number	of files successfuly added.</returns>
        private int Open(String pathNames, InfoFileOptions options = InfoFileOptions.Nothing) {
            return (int) MediaInfoList_Open(_handle, pathNames, (IntPtr) options);
        }

        #endregion

        #region IDisposable

        public bool IsDisposed {
            get { return _isDisposed; }
            private set { _isDisposed = value; }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void IDisposable.Dispose() {
            Close();
        }

        /// <summary>Closes all files in the list and disposes all allocated resources.</summary>
        public void Close() {
            if (IsDisposed) {
                const int ALL_FILES = -1;
                MediaInfoList_Close(_handle, (IntPtr) ALL_FILES);

                MediaInfoList_Delete(_handle);
                IsDisposed = true;

                GC.SuppressFinalize(this);
            }
        }

        ~MediaInfoList() {
            Close();
        }

        #endregion

        #region IEnumerable

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1">IEnumerator&lt;out T&gt;</see> that can be used to iterate through the collection.</returns>
        public IEnumerator<MediaListFile> GetEnumerator() {
            return ((IEnumerable<MediaListFile>) _mediaList).GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="System.Collections.IEnumerator">IEnumerator</see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }

}