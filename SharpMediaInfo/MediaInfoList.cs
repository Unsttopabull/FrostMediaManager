using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Frost.SharpMediaInfo {

    public class MediaInfoList : IDisposable, IEnumerable<MediaListFile> {

        private readonly IntPtr _handle;
        private List<MediaListFile> _mediaList;
        private bool _isDisposed;

        #region Constructors
        public MediaInfoList() {
            _handle = MediaInfoList_New();
            _mediaList = new List<MediaListFile>();
        }

        public MediaInfoList(string fileOrFolderPath, InfoFileOptions options = InfoFileOptions.Nothing, bool cacheInfom = true, bool allInfoCache = true) {
            _handle = MediaInfoList_New();

            int count = Open(fileOrFolderPath, options);

            InitFiles(count, cacheInfom, allInfoCache);
        }

        public MediaInfoList(IEnumerable<string> filePaths, InfoFileOptions options = InfoFileOptions.Nothing, bool cacheInfom = true, bool allInfoCache = true) {
            _handle = MediaInfoList_New();
            int count = 0;

            //opening multiple files in a single go currently not supported (even if documentation says otherwise)
            foreach (string filePath in filePaths) {
                Open(filePath, options);
                count++;
            }

            InitFiles(count, cacheInfom, allInfoCache);
        }

        private void InitFiles(int count, bool cacheInform, bool allInfoCache) {
            _mediaList = new List<MediaListFile>(count);
            for (int fileNum = 0; fileNum < count; fileNum++) {
                _mediaList.Add(new MediaListFile(_handle, fileNum, cacheInform, allInfoCache));
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

        /// <summary>Checks if the list contains a file with the specified path.</summary>
        /// <param name="filePath">The file path to check for.</param>
        /// <returns>If found returns the file index in list; otherwise returns <c>-1</c>.</returns>
        public int Contains(string filePath) {
            return _mediaList.FindIndex(mlf => mlf.General.FileInfo.FullPath == filePath);
        }

        /// <summary>
        /// Gets the file in the list with the specified path.
        /// If not found it tries to add it and returns <c>null</c> if not succesfull.
        /// </summary>
        /// <param name="fileName">Name of the file to look for.</param>
        /// <param name="cacheInform">
        /// Used if the file was not found in the list.
        /// If set to <c>true</c> caches the Inform() call.
        /// </param>
        /// <param name="allInfoCache">
        /// Used if the file was not found in the list.
        /// If set to <c>true</c> ShowAllInfo ("Complete_Get") is set to <c>true</c> before the Inform() call and reset afterwards.
        /// </param>
        /// <returns></returns>
        public MediaListFile GetOrOpen(string fileName, bool cacheInform = true, bool allInfoCache = true) {
            int idx = Contains(fileName);
            if (idx > -1) {
                return _mediaList[idx];
            }

            return Open(fileName, cacheInform, allInfoCache)
                ? _mediaList[_mediaList.Count - 1]
                : null;
        }

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

        #region Open Files

        /// <summary>Open a file and collect information about it (technical information and tags). The Inform() call can be optionaly cached to reduce further calls to the DLL.</summary>
        /// <param name="fileName">Full name of the file to open.</param>
        /// <param name="cacheInform">If set to <c>true</c> caches the Inform() call.</param>
        /// <param name="allInfoCache">if set to <c>true</c> ShowAllInfo ("Complete_Get") is set to <c>true</c> before the Inform() call and reset afterwards.</param>
        /// <returns>Returns <c>true</c> if sucessfull, otherwise <c>false</c></returns>
        public bool Open(string fileName, bool cacheInform, bool allInfoCache) {
            if (Open(fileName) > 0) {
                _mediaList.Add(new MediaListFile(_handle, _mediaList.Count, cacheInform, allInfoCache));
                return true;
            }
            return false;
        }

        /// <summary>Open a file and collect information about it (technical information and tags). The Inform() call can be optionaly cached to reduce further calls to the DLL.</summary>
        /// <param name="fileName">Full name of the file to open.</param>
        /// <param name="cacheInform">If set to <c>true</c> caches the Inform() call.</param>
        /// <param name="allInfoCache">if set to <c>true</c> ShowAllInfo ("Complete_Get") is set to <c>true</c> before the Inform() call and reset afterwards.</param>
        /// <returns>Returns <c>true</c> if sucessfull, otherwise <c>false</c></returns>
        public Task<bool> OpenAsync(string fileName, bool cacheInform, bool allInfoCache) {
            return Task.Run(() => Open(fileName, cacheInform, allInfoCache));
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
            return _mediaList.Count;
        }

        /// <summary>Gets a value indicating whether any of the files in the list are still open.</summary>
        /// <value>Is <c>true</c> if any of the files in the list are still open; otherwise, <c>false</c>.</value>
        public bool AnyOpen {
            get { return _mediaList.FirstOrDefault(mlf => mlf.IsOpen) != null; } 
        }

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
        private int Open(string pathNames, InfoFileOptions options = InfoFileOptions.Nothing) {
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
            return _mediaList.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="System.Collections.IEnumerator">IEnumerator</see> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }

}