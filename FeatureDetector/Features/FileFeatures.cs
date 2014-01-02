using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.Util;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;

using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using Language = Frost.Common.Models.DB.MovieVo.Language;

namespace Frost.DetectFeatures {
    public partial class FileFeatures {
        private readonly DirectoryInfo _directoryInfo;
        private readonly string _directoryRegex;
        private readonly FileVo _file;
        private readonly FileNameInfo _fnInfo;
        private readonly MediaListFile _mediaFile;
        private readonly MediaInfoList _mf;
        private readonly Dictionary<string, FileNameInfo> _fileNameInfos;
        private readonly string _filePath;
        private readonly NFOPriority _nfoPriority;

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
            DetectorFactory.LoadProfilesFromFolder("profiles");
        }

        internal FileFeatures(string filePath, NFOPriority nfoPriority, FeatureDetector fd) {
            Movie = new Movie();

            _filePath = filePath;
            _nfoPriority = nfoPriority;
            _mf = fd.MediaList;
            _fileNameInfos = fd.FileNameInfos;

            bool contains;
            lock (_fileNameInfos) {
              contains = _fileNameInfos.ContainsKey(_filePath);
            }

            if (!contains) {
                FileNameParser fnp = new FileNameParser(_filePath);
                _fnInfo = fnp.Parse();

                lock (_fileNameInfos) {
                  _fileNameInfos.Add(_filePath, _fnInfo);
                }
            }
            else {
                lock (_fileNameInfos) {
                  _fnInfo = _fileNameInfos[_filePath];
                }
            }

            string directoryPath;
            if (!filePath.EndsWith(".iso")) {
                lock (_mf) {
                  _mediaFile = _mf.Add(filePath, true, true);
                }

                _file = new FileVo(_mediaFile.General.FileInfo.FileName, _mediaFile.General.FileInfo.Extension, _mediaFile.General.FileInfo.FolderPath, _mediaFile.General.FileInfo.FileSize);
                _directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filePath) ?? "");
                directoryPath = _directoryInfo.FullName;
            }
            else {
                try {
                    FileInfo fi = new FileInfo(filePath);

                    string withoutExtension = Path.GetFileNameWithoutExtension(fi.Name);
                    withoutExtension = string.IsNullOrEmpty(withoutExtension)
                                           ? fi.Name.Substring(0, fi.Name.LastIndexOf('.'))
                                           : withoutExtension;

                    _file = new FileVo(withoutExtension, fi.Extension.TrimStart('.'), fi.DirectoryName, fi.Length);
                    _directoryInfo = fi.Directory;

                    directoryPath = _directoryInfo != null
                        ? _directoryInfo.FullName
                        : GetFolderPath();
                }
                catch (Exception e) {
                    directoryPath = GetFolderPath();
                }
            }

            _directoryRegex = Regex.Escape(directoryPath.Replace("\\", "/"))
                                   .Replace("/", @"[\\/]");
        }

        private string GetFolderPath() {
            return _filePath.Substring(0, _filePath.LastIndexOfAny(new[] {'\\', '/'}));
        }

        public Movie Movie { get; private set; }

        internal void Detect() {
            GetFileNameInfo();

            GetSubtitles();
            GetVideoInfo();
            GetAudioInfo();

            if (_nfoPriority != NFOPriority.Ignore) {
                GetNfoInfo();
            }
        }

        internal Task<Movie> DetectAsync() {
            GetFileNameInfo();

            Task subs = Task.Run(() => GetSubtitles());
            Task video = Task.Run(() => GetVideoInfo());
            Task audio = Task.Run(() => GetAudioInfo());

            if (_nfoPriority != NFOPriority.Ignore) {
                Task nfo = Task.Run(() => GetNfoInfo());
                return Task.WhenAll(subs, video, audio, nfo).ContinueWith(tsk => Movie);
            }

            return Task.WhenAll(subs, video, audio).ContinueWith(tsk => Movie);
        }

        private void GetFileNameInfo() {

            if (_fnInfo.Part != 0) {
                Movie.IsMultipart = true;
                Movie.PartTypes = _fnInfo.PartType;
            }

            Movie.Title = _fnInfo.Title;
            Movie.ReleaseYear = _fnInfo.ReleaseYear.Year;

            if (!string.IsNullOrEmpty(_fnInfo.Genre)) {
                Movie.Genres.Add(_fnInfo.Genre);
            }

            Movie.Edithion = _fnInfo.Edithion;
            Movie.ReleaseGroup = _fnInfo.ReleaseGroup;
            Movie.Specials.UnionWith(Special.FromValues(_fnInfo.Specials));
        }

        private static Language GetLanguage(bool subtitles, string mediaInfoLang, ISOLanguageCode detectedLangCode, ISOLanguageCode subLangCode, ISOLanguageCode langCode) {
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
            return _file.Name + "." + _file.Extension;
        }
    }
}