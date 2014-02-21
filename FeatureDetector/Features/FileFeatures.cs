using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using File = System.IO.File;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using Language = Frost.Common.Models.DB.MovieVo.Language;
using ScanType = Frost.Common.ScanType;

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

        static FileFeatures() {
            AudioCodecIdMappings = new CodecIdMappingCollection {
                new CodecIdBinding("A_AAC", "AAC"),
                new CodecIdBinding("DD", "dolbydigital"),
                new CodecIdBinding("ogg", "vorbis"),
                new CodecIdBinding("a_vorbis", "vorbis"),
                new CodecIdBinding("dtsma", "dtshd"),
                new CodecIdBinding("dtshr", "dtshd"),
                new CodecIdBinding("AAC LC", "AAC"),
                new CodecIdBinding("AAC LC-SBR", "AAC"),
                new CodecIdBinding("A_MPEG/L3", "MP3"),
                new CodecIdBinding("A_DTS", "DTS"),
                new CodecIdBinding("dca", "DTS"),
                new CodecIdBinding("A_AC3", "AC3"),
                new CodecIdBinding("MPA1L3", "MP3"),
                new CodecIdBinding("MPA2L3", "MP3"),
                new CodecIdBinding("MPA1L2", "MP2"),
                new CodecIdBinding("MPA1L1", "MP1"),
                new CodecIdBinding("MPEG-2A", "mpeg2"),
                new CodecIdBinding("MPEG-1A", "mpeg"),
                new CodecIdBinding("161", "wma")
            };

            VideoCodecIdMappings = new CodecIdMappingCollection {
                new CodecIdBinding("MPEG-1 Video", "mpeg"),
                new CodecIdBinding("MPEG-2 Video", "mpeg2"),
                new CodecIdBinding("Xvid", "Xvid"),
                new CodecIdBinding("V_MPEG4/ISO/AVC", "h264"),
                new CodecIdBinding("pvmm", "mpeg4"),
                new CodecIdBinding("ndx", "mpeg4"),
                new CodecIdBinding("nds", "mpeg4"),
                new CodecIdBinding("mpeg-4", "mpeg4"),
                new CodecIdBinding("m4s2", "mpeg4"),
                new CodecIdBinding("geox", "mpeg4"),
                new CodecIdBinding("dx50", "mpeg4"),
                new CodecIdBinding("dm4v", "mpeg4"),
                new CodecIdBinding("xvix", "xvid"),
                new CodecIdBinding("pim1", "mpeg"),
                new CodecIdBinding("mpeg-2", "mpeg2"),
                new CodecIdBinding("mmes", "mpeg2"),
                new CodecIdBinding("lmp2", "mpeg2"),
                new CodecIdBinding("em2v", "mpeg2"),
                new CodecIdBinding("div6", "divx"),
                new CodecIdBinding("div5", "divx"),
                new CodecIdBinding("div4", "divx"),
                new CodecIdBinding("div3", "divx"),
                new CodecIdBinding("div2", "divx"),
                new CodecIdBinding("div1", "divx"),
                new CodecIdBinding("3ivd", "3ivx"),
                new CodecIdBinding("3iv2", "3ivx"),
                new CodecIdBinding("zygo", "qt"),
                new CodecIdBinding("svq", "qt"),
                new CodecIdBinding("sv10", "qt"),
                new CodecIdBinding("smc", "qt"),
                new CodecIdBinding("rpza", "qt"),
                new CodecIdBinding("rle", "qt"),
                new CodecIdBinding("avrn", "qt"),
                new CodecIdBinding("advj", "qt"),
                new CodecIdBinding("8bps", "qt"),
            };

            //known subtitle extensions already sorted
            KnownSubtitleExtensions = new List<string> {
                "890",
                "aqt",
                "asc",
                "ass",
                "dat",
                "dks",
                "js",
                "jss",
                "lrc",
                "mpl",
                "ovr",
                "pan",
                "pjs",
                "psb",
                "rt",
                "rtf",
                "s2k",
                "sami",
                "sbt",
                "scr",
                "smi",
                "son",
                "srt",
                "ssa",
                "sst",
                "ssts",
                "stl",
                "sub",
                "tts",
                "txt",
                "vkt",
                "vsf",
                "xas",
                "zeg"
            };

            //known subtitle format names already sorted
            KnownSubtitleFormats = new List<string> {
                "Adobe encore DVD",
                "Advanced Substation Alpha",
                "AQTitle",
                "ASS",
                "Captions Inc",
                "Cheeta",
                "Cheetah",
                "CPC Captioning",
                "CPC-600",
                "EBU Subtitling Format",
                "N19",
                "SAMI",
                "Sami Captioning",
                "SSA",
                "SubRip",
                "SubStation Alpha",
                "VobSub"
            };

            _subtitleExtensionsRegex = string.Format(@"\.({0})", string.Join("|", KnownSubtitleExtensions));

            //DetectorFactory.LoadStaticProfiles();
            if (Directory.Exists("LanguageProfiles")) {
                DetectorFactory.LoadProfilesFromFolder("LanguageProfiles");
            }
            else {
                DetectorFactory.LoadStaticProfiles();
            }
        }

        private FileFeatures(NFOPriority nfoPriority) {
            _mvc = new MovieVoContainer(false);
            _nfoPriority = nfoPriority;
            _mf = new MediaInfoList();

            _fnInfos = new Dictionary<string, FileNameInfo>();
        }

        public FileFeatures(NFOPriority nfoPriority, params string[] fileNames) : this(nfoPriority) {
            _files = new FileVo[fileNames.Length];

            foreach (string filenName in fileNames) {
                FileNameParser fnp = new FileNameParser(filenName);
                FileNameInfo nameInfo = fnp.Parse();
                _fnInfos.Add(nameInfo.FileOrFolderName, nameInfo);
            }

            Init(fileNames);
        }

        public FileFeatures(NFOPriority nfoPriority, params FileNameInfo[] fileNameInfos) : this(nfoPriority) {
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
            Movie.DirectoryPath = _directoryInfo.FullName;
        }

        private void InitFile(int idx, string filePath) {
            if (!File.Exists(filePath)) {
                _error = true;
                return;
            }

            string directoryPath = null;
            FileVo file = null;
            if (!filePath.EndsWith(".iso")) {
                MediaListFile mFile = _mf.Add(filePath, true, true);

                if (mFile != null && !string.IsNullOrEmpty(mFile.General.FileInfo.FileName)) {
                    file = new FileVo(mFile.General.FileInfo.FileName, mFile.General.FileInfo.Extension, mFile.General.FileInfo.FolderPath + "/",
                        mFile.General.FileInfo.FileSize);
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
            try {
                FileInfo fi = new FileInfo(filePath);

                if (file == null) {
                    file = ParseFileInfoFromFile(fi);
                }

                _directoryInfo = fi.Directory;
                directoryPath = _directoryInfo != null
                                    ? _directoryInfo.FullName
                                    : filePath.Substring(0, filePath.LastIndexOfAny(new[] { '\\', '/' }));
            }
            catch (Exception) {
                directoryPath = filePath.Substring(0, filePath.LastIndexOfAny(new[] { '\\', '/' }));
                return directoryPath;
            }
            return directoryPath;
        }

        private static FileVo ParseFileInfoFromFile(FileInfo fi) {
            if (fi == null) {
                throw new ArgumentNullException("fi");
            }

            string withoutExtension = Path.GetFileNameWithoutExtension(fi.Name);

            withoutExtension = string.IsNullOrEmpty(withoutExtension)
                                   ? fi.Name.Substring(0, fi.Name.LastIndexOf('.'))
                                   : withoutExtension;

            return new FileVo(withoutExtension, fi.Extension.Substring(1), fi.DirectoryName, fi.Length);
        }

        public Movie Movie { get; private set; }

        public bool Detect() {
            if (_error) {
                return false;
            }

            foreach (FileVo file in _files) {
                DetectFile(file);
            }

            if (!Movie.Runtime.HasValue) {
                Movie.GetVideoRuntimeSum();
            }

            GetGeneralAudioInfo();
            GetGeneralVideoInfo();

            DetectMovieType();

            return Save();
        }

        private void GetGeneralVideoInfo() {
            var mostFrequentVres = Movie.Videos.Where(v => v.Resolution.HasValue)
                                        .GroupBy(v => v.Resolution)
                                        .OrderByDescending(g => g.Count())
                                        .FirstOrDefault();

            if (mostFrequentVres != null) {
                Video video = mostFrequentVres.FirstOrDefault();

                if (video != null) {
                    string resolution = video.Resolution.ToString();
                    switch (video.ScanType) {
                        case ScanType.Interlaced:
                            resolution = resolution + "i";
                            break;
                        case ScanType.Progressive:
                            resolution = resolution + "p";
                            break;
                    }
                    Movie.VideoResolution = resolution;
                }
            }

            var mostFrequent = Movie.Videos.Where(v => v.CodecId != null)
                                    .GroupBy(v => v.CodecId)
                                    .OrderByDescending(g => g.Count())
                                    .FirstOrDefault();

            if (mostFrequent != null) {
                Video video = mostFrequent.FirstOrDefault();

                if (video != null) {
                    Movie.VideoCodec = video.CodecId;
                }
            }
        }

        private void GetGeneralAudioInfo() {
            int numChannels = 0;
            foreach (Audio audio in Movie.Audios) {
                if (audio.NumberOfChannels.HasValue) {
                    int val = audio.NumberOfChannels.Value;
                    if (val > numChannels) {
                        numChannels = val;
                    }
                }
            }

            if (numChannels != 0) {
                Movie.NumberOfAudioChannels = numChannels;
            }

            var mostFrequentAudioCodec = Movie.Audios.Where(v => v.CodecId != null)
                                              .GroupBy(v => v.CodecId)
                                              .OrderByDescending(g => g.Count())
                                              .FirstOrDefault();

            if (mostFrequentAudioCodec != null) {
                Audio audio = mostFrequentAudioCodec.FirstOrDefault();

                if (audio != null) {
                    Movie.AudioCodec = audio.CodecId;
                }
            }
        }

        private bool Save() {
            try {
                if (_mvc.Database.Connection.State == ConnectionState.Closed) {
                    _mvc.Database.Connection.Open();
                }

                _mvc.SaveChanges();
                return true;
            }
            catch (SQLiteException e) {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            catch (InvalidOperationException e) {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            catch (Exception e) {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }

        private void DetectMovieType() {
            if (Movie.Videos.All(v => v.File.Extension.OrdinalEquals("vob") ||
                                      v.File.Extension.OrdinalEquals("ifo") ||
                                      v.File.Extension.OrdinalEquals("bup")) ||
                Movie.Videos.All(v => v.Source.OrdinalEquals("DVD") ||
                                      v.Source.OrdinalEquals("DVDR") ||
                                      v.Source.OrdinalEquals("DVD-R"))) {
                Movie.Type = MovieType.DVD;
            }

            Regex reg = new Regex(@"(Bluray|BlueRay|Blu-ray|BD(5|25|9|50|r)?)$", RegexOptions.IgnoreCase);
            if (_fnInfos.Values.Any(fi => fi.VideoSource != null && reg.IsMatch(fi.VideoSource))) {
                Movie.Type = MovieType.BluRay;
            }
        }

        private void DetectFile(FileVo file) {
            GetFileNameInfo();

            GetSubtitles(file);
            GetVideoInfo(file);
            GetAudioInfo(file);

            if (_nfoPriority != NFOPriority.Ignore) {
                GetNfoInfo(file.Name);
            }

            GetArtInfo();
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
                    Special spec = Movie.Specials.FirstOrDefault(s => s.Value == special);
                    if (spec != null) {
                        continue;
                    }

                    Special dbSpecial = _mvc.Specials.FirstOrDefault(s => s.Value == special);
                    Movie.Specials.Add(dbSpecial ?? new Special(special));
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
                if (_mf != null) {
                    _mf.Close();
                }

                if (_md5 != null) {
                    _md5.Dispose();
                }

                if (_mvc != null) {
                    if (_mvc.Database.Connection.State != ConnectionState.Closed) {
                        _mvc.Database.Connection.Close();
                    }
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