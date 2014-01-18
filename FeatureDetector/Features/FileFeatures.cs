using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.Util;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using File = System.IO.File;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using Language = Frost.Common.Models.DB.MovieVo.Language;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private DirectoryInfo _directoryInfo;
        private string _directoryRegex;
        private readonly Dictionary<string, FileNameInfo> _fnInfos;
        private readonly MediaInfoList _mf;
        private readonly NFOPriority _nfoPriority;
        private readonly MovieVoContainer _mvc;
        private readonly FileVo[] _files;
        private bool _error;
        private bool _manualParse;

        static FileFeatures() {
            //known subtitle extensions already sorted
            KnownSubtitleExtensions = new[] {
                "890", "aqt", "asc", "ass", "dat", "dks", "js", "jss", "lrc", "mpl", "ovr", "pan",
                "pjs", "psb", "rt", "rtf", "s2k", "sami", "sbt", "scr", "smi", "son", "srt", "ssa",
                "sst", "ssts", "stl", "sub", "tts", "txt", "vkt", "vsf", "xas", "zeg"
            };

            //known subtitle format names already sorted
            KnownSubtitleFormats = new[] {
                "Adobe encore DVD", "Advanced Substation Alpha", "AQTitle",
                "ASS", "Captions Inc", "Cheeta", "Cheetah", "CPC Captioning",
                "CPC-600", "EBU Subtitling Format", "N19", "SAMI",
                "Sami Captioning", "SSA", "SubRip", "SubStation Alpha"
            };

            SubtitleExtensionsRegex = string.Format(@"\.({0})", string.Join("|", KnownSubtitleExtensions));

            //DetectorFactory.LoadStaticProfiles();
            DetectorFactory.LoadProfilesFromFolder("LanguageProfiles");
        }

        private FileFeatures(NFOPriority nfoPriority) {
            _mvc = new MovieVoContainer();
            _nfoPriority = nfoPriority;
            _mf = new MediaInfoList();

            _fnInfos = new Dictionary<string, FileNameInfo>();
        }

        internal FileFeatures(NFOPriority nfoPriority, params string[] fileNames) : this(nfoPriority) {
            _files = new FileVo[fileNames.Length];

            foreach (string filenName in fileNames) {
                FileNameParser fnp = new FileNameParser(filenName);
                FileNameInfo nameInfo = fnp.Parse();
                _fnInfos.Add(nameInfo.FileOrFolderName, nameInfo);
            }

            Init(fileNames);
        }

        internal FileFeatures(NFOPriority nfoPriority, params FileNameInfo[] fileNameInfos) : this(nfoPriority){
            _files = new FileVo[fileNameInfos.Length];

            foreach (FileNameInfo nameInfo in fileNameInfos) {
                _fnInfos.Add(nameInfo.FileOrFolderName, nameInfo);
            }

            Init(fileNameInfos.Select(fnInfo => fnInfo.FilePath).ToArray());
        }

        private void Init(string[] filePaths) {
            int length = filePaths.Length;

            for (int i = 0; i < length && !_error; i++) {
                InitFile(i, filePaths[i]);
            }

            Movie = _mvc.Movies.Add(new Movie());            
        }

        private void InitFile(int idx, string filePath) {
            if (!File.Exists(filePath)) {
                _error = true;
                return;
            }

            _manualParse = false;
            string directoryPath = null;
            FileVo file = null;
            if (!filePath.EndsWith(".iso")) {
                MediaListFile mFile = _mf.Add(filePath, true, true);

                if (mFile != null && !string.IsNullOrEmpty(mFile.General.FileInfo.FileName)) {
                    file = new FileVo(mFile.General.FileInfo.FileName, mFile.General.FileInfo.Extension, mFile.General.FileInfo.FolderPath + "/", mFile.General.FileInfo.FileSize);
                }
                else {
                    directoryPath = ParseInfoFromPath(filePath, ref file);
                }

                if (_directoryInfo == null) {
                    _directoryInfo = new DirectoryInfo(directoryPath ?? Path.GetDirectoryName(filePath) ?? "");
                    directoryPath = _directoryInfo.FullName;
                }
            }

            if (_directoryRegex == null) {
                if (directoryPath == null) {
                    directoryPath = ParseInfoFromPath(filePath, ref file);
                }

                _directoryRegex = Regex.Escape(directoryPath.Replace("\\", "/"))
                                       .Replace("/", @"[\\/]");

                if (_directoryInfo == null) {
                    _directoryInfo = new DirectoryInfo(directoryPath);
                }
            }

            if (file != null && file.ToString() != ".") {
                _files[idx] = _mvc.Files.Add(file);
            }
            else if (File.Exists(filePath)) {

            }
            else {
            }
        }

        private string ParseInfoFromPath(string filePath, ref FileVo file) {
            string directoryPath;
            FileInfo fi; 
            try {
                fi = new FileInfo(filePath);

                _directoryInfo = fi.Directory;
                directoryPath = _directoryInfo != null
                                    ? _directoryInfo.FullName
                                    : filePath.Substring(0, filePath.LastIndexOfAny(new[] { '\\', '/' }));
            }
            catch (Exception e) {
                directoryPath = filePath.Substring(0, filePath.LastIndexOfAny(new[] { '\\', '/' }));
                return directoryPath;
            }

            if(file == null) {
                _manualParse = true;
                string withoutExtension = Path.GetFileNameWithoutExtension(fi.Name);
                withoutExtension = string.IsNullOrEmpty(withoutExtension)
                                           ? fi.Name.Substring(0, fi.Name.LastIndexOf('.'))
                                           : withoutExtension;

                file = new FileVo(withoutExtension, fi.Extension.Substring(1), fi.DirectoryName, fi.Length);
            }
            return directoryPath;
        }

        public Movie Movie { get; private set; }

        internal bool Detect() {
            if (_error) {
                return false;
            }

            foreach (FileVo file in _files) {
                DetectFile(file);
            }

            try {
                _mvc.SaveChanges();
                return true;
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }

        private void DetectFile(FileVo file) {
            GetFileNameInfo();

            GetSubtitles(file);
            GetVideoInfo(file);
            GetAudioInfo(file);
            GetArtInfo();

            if (_nfoPriority != NFOPriority.Ignore) {
                GetNfoInfo(file.Name);
            }
        }

        private void GetFileNameInfo() {
            foreach (FileNameInfo fnInfo in _fnInfos.Values) {
                if (fnInfo.Part != 0) {
                    Movie.IsMultipart = true;
                    Movie.PartTypes = fnInfo.PartType;
                }

                if (string.IsNullOrEmpty(Movie.Title)) {
                    Movie.Title = fnInfo.Title;
                }

                if (!Movie.ReleaseYear.HasValue) {
                    Movie.ReleaseYear = fnInfo.ReleaseYear.Year;
                }

                if (!string.IsNullOrEmpty(fnInfo.Genre) && !Movie.Genres.Contains(fnInfo.Genre)) {
                    AddGenre(fnInfo.Genre);
                }

                if (string.IsNullOrEmpty(Movie.Edithion)) {
                    Movie.Edithion = fnInfo.Edithion;
                }

                if (string.IsNullOrEmpty(Movie.ReleaseGroup)) {
                    Movie.ReleaseGroup = fnInfo.ReleaseGroup;
                }

                foreach (string special in fnInfo.Specials) {
                    Special item = new Special(special);
                    if (!Movie.Specials.Contains(item)) {
                        Movie.Specials.Add(_mvc.Specials.FirstOrDefault(s => s.Value == special) ?? item);
                    }
                }
            }
        }

        private Language GetLanguage(bool subtitles, string mediaInfoLang, ISOLanguageCode detectedLangCode, ISOLanguageCode subLangCode, ISOLanguageCode langCode) {
            if (mediaInfoLang != null) {
                return Language.FromISO639(mediaInfoLang);
            }

            if (detectedLangCode != null) {
                return new Language(detectedLangCode);
            }

            if (subtitles && subLangCode != null) {
                return new Language(subLangCode);
            }

            if (langCode != null) {
                return new Language(langCode);
            }
            return null;
        }

        #region IDisposable

        /// <summary>Gets a value indicating whether this object has already been disposed.</summary>
        /// <value>Is <c>true</c> if this object has already been disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (!IsDisposed) {
                _mf.Close();

                if (_md5 != null) {
                    _md5.Dispose();
                }

                if (_mvc != null) {
                    _mvc.Dispose();
                }

                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void IDisposable.Dispose() {
            Dispose();
        }

        ~FileFeatures() {
            Dispose();
        }

        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            FileNameInfo fnInfo = _fnInfos.Values.FirstOrDefault();
            return fnInfo != null
                ? fnInfo.ToString()
                : base.ToString();
        }
    }

}