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
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using Language = Frost.Common.Models.DB.MovieVo.Language;

namespace Frost.DetectFeatures {
    public partial class FileFeatures {
        private DirectoryInfo _directoryInfo;
        private readonly string _directoryRegex;
        private FileNameInfo _fnInfo;
        private readonly MediaListFile _mediaFile;
        private readonly MediaInfoList _mf;
        private readonly string _filePath;
        private readonly NFOPriority _nfoPriority;
        private readonly MovieVoContainer _mvc;
        private readonly FileVo _file;

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

        internal FileFeatures(string filePath, NFOPriority nfoPriority, FeatureDetector fd) {
            _mvc = new MovieVoContainer();

            _filePath = filePath;
            _nfoPriority = nfoPriority;
            _mf = fd.MediaList;

            GetFileNameInfo(fd.FileNameInfos);

            FileVo file = null;
            string directoryPath;
            if (!filePath.EndsWith(".iso")) {
                _mediaFile = _mf.Add(filePath, true, true);

                file = new FileVo(_mediaFile.General.FileInfo.FileName, _mediaFile.General.FileInfo.Extension, _mediaFile.General.FileInfo.FolderPath, _mediaFile.General.FileInfo.FileSize);
                _directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath) ?? "");
                directoryPath = _directoryInfo.FullName;
            }
            else {
                directoryPath = ParseInfoFromPath(filePath, ref file);
            }

            if (file != null) {
                _file = file;
                _file = _mvc.Files.Add(_file);
            }

            _directoryRegex = Regex.Escape(directoryPath.Replace("\\", "/"))
                                   .Replace("/", @"[\\/]");

            Movie = new Movie();
            _mvc.Movies.Add(Movie);
        }

        private void GetFileNameInfo(Dictionary<string, FileNameInfo> fileNameInfos) {
            if (!fileNameInfos.ContainsKey(_filePath)) {
                FileNameParser fnp = new FileNameParser(_filePath);
                _fnInfo = fnp.Parse();

                fileNameInfos.Add(_filePath, _fnInfo);
            }
            else {
                _fnInfo = fileNameInfos[_filePath];
            }
        }

        private string ParseInfoFromPath(string filePath, ref FileVo file) {
            string directoryPath;
            try {
                FileInfo fi = new FileInfo(filePath);

                string withoutExtension = Path.GetFileNameWithoutExtension(fi.Name);
                withoutExtension = string.IsNullOrEmpty(withoutExtension)
                                       ? fi.Name.Substring(0, fi.Name.LastIndexOf('.'))
                                       : withoutExtension;

                file = new FileVo(withoutExtension, fi.Extension.TrimStart('.'), fi.DirectoryName, fi.Length);
                _directoryInfo = fi.Directory;

                directoryPath = _directoryInfo != null
                                    ? _directoryInfo.FullName
                                    : GetFolderPath();
            }
            catch (Exception e) {
                directoryPath = GetFolderPath();
            }
            return directoryPath;
        }

        private string GetFolderPath() {
            return _filePath.Substring(0, _filePath.LastIndexOfAny(new[] {'\\', '/'}));
        }

        public Movie Movie { get; set; }

        internal bool Detect() {
            GetFileNameInfo();
            
            GetSubtitles();
            GetVideoInfo();
            GetAudioInfo();
            GetArtInfo();

            if (_nfoPriority != NFOPriority.Ignore) {
                GetNfoInfo();
            }

            try {
                _mvc.SaveChanges();
                return true;
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
            }
            return false;
        }

        private void GetFileNameInfo() {

            if (_fnInfo.Part != 0) {
                Movie.IsMultipart = true;
                Movie.PartTypes = _fnInfo.PartType;
            }

            Movie.Title = _fnInfo.Title;
            Movie.ReleaseYear = _fnInfo.ReleaseYear.Year;

            AddGenre(_fnInfo.Genre);

            Movie.Edithion = _fnInfo.Edithion;
            Movie.ReleaseGroup = _fnInfo.ReleaseGroup;

            foreach (string special in _fnInfo.Specials) {
                Movie.Specials.Add(_mvc.Specials.FirstOrDefault(s => s.Value == special) ?? new Special(special));
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

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return _file != null ? _file.ToString() : base.ToString();
        }
    }
}