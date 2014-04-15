using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using log4net;
using File = System.IO.File;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FileFeatures));

        private DirectoryInfo _directoryInfo;
        private string _directoryRegex;
        private readonly Dictionary<string, FileNameInfo> _fnInfos;
        private readonly MediaInfoList _mf;
        private readonly NFOPriority _nfoPriority;
        private readonly FileDetectionInfo[] _files;
        private bool _error;

        static FileFeatures() {
            #region Audio CodecId Mappings

            AudioCodecIdMappings = new CodecIdMappingCollection {
                { "A_AAC", "AAC" },
                { "DD", "dolbydigital" },
                { "ogg", "vorbis" },
                { "a_vorbis", "vorbis" },
                { "dtsma", "dtshd" },
                { "dtshr", "dtshd" },
                { "AAC LC", "AAC" },
                { "AAC LC-SBR", "AAC" },
                { "A_MPEG/L3", "MP3" },
                { "A_DTS", "DTS" },
                { "dca", "DTS" },
                { "A_AC3", "AC3" },
                { "MPA1L3", "MP3" },
                { "MPA2L3", "MP3" },
                { "MPA1L2", "MP2" },
                { "MPA1L1", "MP1" },
                { "MPEG-2A", "mpeg2" },
                { "MPEG-1A", "mpeg" },
                { "161", "wma" }
            };

            #endregion

            #region Video CodecID mappings

            VideoCodecIdMappings = new CodecIdMappingCollection {
                { "MPEG-1 Video", "mpeg" },
                { "MPEG-2 Video", "mpeg2" },
                { "Xvid", "Xvid" },
                { "V_MPEG4/ISO/AVC", "h264" },
                { "pvmm", "mpeg4" },
                { "ndx", "mpeg4" },
                { "nds", "mpeg4" },
                { "mpeg-4", "mpeg4" },
                { "m4s2", "mpeg4" },
                { "geox", "mpeg4" },
                { "dx50", "mpeg4" },
                { "dm4v", "mpeg4" },
                { "xvix", "xvid" },
                { "pim1", "mpeg" },
                { "mpeg-2", "mpeg2" },
                { "mmes", "mpeg2" },
                { "lmp2", "mpeg2" },
                { "em2v", "mpeg2" },
                { "div6", "divx" },
                { "div5", "divx" },
                { "div4", "divx" },
                { "div3", "divx" },
                { "div2", "divx" },
                { "div1", "divx" },
                { "3ivd", "3ivx" },
                { "3iv2", "3ivx" },
                { "zygo", "qt" },
                { "svq", "qt" },
                { "sv10", "qt" },
                { "smc", "qt" },
                { "rpza", "qt" },
                { "rle", "qt" },
                { "avrn", "qt" },
                { "advj", "qt" },
                { "8bps", "qt" }
            };

            #endregion

            #region Known subtitle extensions already sorted

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

            #endregion

            #region Known subtitle format names already sorted

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

            #endregion

            _subtitleExtensionsRegex = string.Format(@"\.({0})", string.Join("|", KnownSubtitleExtensions));

            try {
                if (Directory.GetFiles("LanguageProfiles").Length > 0) {
                    DetectorFactory.LoadProfilesFromFolder("LanguageProfiles");
                }
                else {
                    DetectorFactory.LoadStaticProfiles();
                }
            }
            catch {
                DetectorFactory.LoadStaticProfiles();
            }
        }

        private FileFeatures(NFOPriority nfoPriority) {
            _nfoPriority = nfoPriority;
            _mf = new MediaInfoList();

            _fnInfos = new Dictionary<string, FileNameInfo>();
        }

        public FileFeatures(NFOPriority nfoPriority, params string[] fileNames) : this(nfoPriority) {
            _files = new FileDetectionInfo[fileNames.Length];

            foreach (string filenName in fileNames) {
                FileNameParser fnp = new FileNameParser(filenName);
                FileNameInfo nameInfo = fnp.Parse();
                _fnInfos.Add(nameInfo.FileOrFolderName, nameInfo);
            }

            Init(fileNames);
        }

        public FileFeatures(NFOPriority nfoPriority, params FileNameInfo[] fileNameInfos) : this(nfoPriority) {
            _files = new FileDetectionInfo[fileNameInfos.Length];

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

            Movie = new MovieInfo { DirectoryPath = _directoryInfo.FullName };
            Movie.FileInfos.AddRange(_files);
        }

        private void InitFile(int idx, string filePath) {
            if (!File.Exists(filePath)) {
                _error = true;
                return;
            }

            string directoryPath = null;
            FileDetectionInfo file = null;
            if (!filePath.EndsWith(".iso")) {
                MediaListFile mFile = _mf.Add(filePath, true, true);

                if (mFile != null && !string.IsNullOrEmpty(mFile.General.FileInfo.FileName)) {
                    file = new FileDetectionInfo(
                        mFile.General.FileInfo.FileName,
                        mFile.General.FileInfo.Extension,
                        mFile.General.FileInfo.FolderPath + Path.DirectorySeparatorChar,
                        mFile.General.FileInfo.FileSize
                        );

                    if (mFile.General.FileInfo.CreatedDate.HasValue) {
                        file.CreateTime = mFile.General.FileInfo.CreatedDate.Value;
                    }
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
                //_files[idx] = _mvc.Files.Add(file);
                _files[idx] = file;
            }
            else if (File.Exists(filePath)) {
            }
            else {
            }
        }

        private string ParseInfoFromPath(string filePath, ref FileDetectionInfo file) {
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
            catch (Exception e) {
                if (Log.IsWarnEnabled) {
                    Log.Warn(string.Format("Failed to get FileInfo for path: {0}", file), e);
                }

                directoryPath = filePath.Substring(0, filePath.LastIndexOfAny(new[] { '\\', '/' }));
                return directoryPath;
            }
            return directoryPath;
        }

        private static FileDetectionInfo ParseFileInfoFromFile(FileInfo fi) {
            if (fi == null) {
                throw new ArgumentNullException("fi");
            }

            string withoutExtension = Path.GetFileNameWithoutExtension(fi.Name);

            withoutExtension = string.IsNullOrEmpty(withoutExtension)
                                   ? fi.Name.Substring(0, fi.Name.LastIndexOf('.'))
                                   : withoutExtension;

            FileDetectionInfo fd = new FileDetectionInfo(withoutExtension, fi.Extension.Substring(1), fi.DirectoryName, fi.Length) {
                LastAccessTime = fi.LastAccessTime,
                CreateTime = fi.CreationTime
            };

            return fd;
        }

        public MovieInfo Movie { get; private set; }

        public bool Detect() {
            if (_error) {
                return false;
            }

            foreach (FileDetectionInfo file in _files) {
                DetectFile(file);
            }

            if (!Movie.Runtime.HasValue) {
                Movie.CalculateVideoRuntimeSum();
            }

            GetGeneralAudioInfo();
            GetGeneralVideoInfo();

            DetectMovieType();

            bool iso = Movie.FileInfos.Any(f => f.Extension.Equals("iso", StringComparison.OrdinalIgnoreCase));
            if (iso) {
                Movie.Type = MovieType.ISO;
            }

            //return Save();
            return true;
        }

        private void GetGeneralVideoInfo() {
            var mostFrequentVres = Movie.FileInfos.SelectMany(f => f.Videos)
                                        .Where(v => v.Resolution.HasValue)
                                        .GroupBy(v => v.Resolution)
                                        .OrderByDescending(g => g.Count())
                                        .FirstOrDefault();

            if (mostFrequentVres != null) {
                VideoDetectionInfo video = mostFrequentVres.FirstOrDefault();

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

            var mostFrequent = Movie.FileInfos.SelectMany(f => f.Videos)
                                    .Where(v => v.CodecId != null)
                                    .GroupBy(v => v.CodecId)
                                    .OrderByDescending(g => g.Count())
                                    .FirstOrDefault();

            if (mostFrequent != null) {
                VideoDetectionInfo video = mostFrequent.FirstOrDefault();

                if (video != null) {
                    Movie.VideoCodec = video.CodecId;
                }
            }
        }

        private void GetGeneralAudioInfo() {
            int numChannels = 0;
            foreach (AudioDetectionInfo audio in Movie.FileInfos.SelectMany(f => f.Audios)) {
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

            var mostFrequentAudioCodec = Movie.FileInfos.SelectMany(f => f.Audios)
                                              .Where(v => v.CodecId != null)
                                              .GroupBy(v => v.CodecId)
                                              .OrderByDescending(g => g.Count())
                                              .FirstOrDefault();

            if (mostFrequentAudioCodec != null) {
                AudioDetectionInfo audio = mostFrequentAudioCodec.FirstOrDefault();

                if (audio != null) {
                    Movie.AudioCodec = audio.CodecId;
                }
            }
        }

        private void DetectMovieType() {
            //bool dvdSource = Movie.FileInfos.SelectMany(f => f.Videos)
            //                                .All(v => v.Source.OrdinalEquals("DVD") ||
            //                                          v.Source.OrdinalEquals("DVDR") ||
            //                                          v.Source.OrdinalEquals("DVD-R")
            //                                );

            if (Movie.FileInfos.All(f => f.Extension.OrdinalEquals("vob") ||
                                         f.Extension.OrdinalEquals("ifo") ||
                                         f.Extension.OrdinalEquals("bup"))) {
                Movie.Type = MovieType.DVD;
            }

            Regex reg = new Regex(@"(Bluray|BlueRay|Blu-ray|BD(5|25|9|50|r)?)$", RegexOptions.IgnoreCase);
            if (_fnInfos.Values.Any(fi => fi.VideoSource != null && reg.IsMatch(fi.VideoSource))) {
                Movie.Type = MovieType.BluRay;
            }
        }

        private void DetectFile(FileDetectionInfo file) {
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

                if (!Movie.ReleaseYear.HasValue && fnInfo.ReleaseYear.Year > 1800 ||
                    Movie.ReleaseYear < 1800 && fnInfo.ReleaseYear.Year > 1800) {
                    Movie.ReleaseYear = fnInfo.ReleaseYear.Year;
                }

                if (!string.IsNullOrEmpty(fnInfo.Genre) && !Movie.Genres.Contains(fnInfo.Genre)) {
                    Movie.Genres.Add(fnInfo.Genre);
                }

                if (string.IsNullOrEmpty(Movie.Edithion)) {
                    Movie.Edithion = fnInfo.Edithion;
                }

                if (string.IsNullOrEmpty(Movie.ReleaseGroup)) {
                    Movie.ReleaseGroup = fnInfo.ReleaseGroup;
                }

                foreach (string special in fnInfo.Specials) {
                    if (Movie.Specials.Contains(special)) {
                        continue;
                    }
                    Movie.Specials.Add(special);
                }
            }
        }

        private ISOLanguageCode GetLanguage(bool subtitles, string mediaInfoLang, ISOLanguageCode detectedLangCode, ISOLanguageCode subLangCode, ISOLanguageCode langCode) {
            if (mediaInfoLang != null) {
                return ISOLanguageCodes.Instance.GetByISOCode(mediaInfoLang);
            }

            if (detectedLangCode != null) {
                return detectedLangCode;
            }

            if (subtitles && subLangCode != null) {
                return subLangCode;
            }

            if (langCode != null) {
                return langCode;
            }
            return null;
        }

        #region IDisposable

        /// <summary>Gets a value indicating whether this object has already been disposed.</summary>
        /// <value>Is <c>true</c> if this object has already been disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool finalizer) {
            if (IsDisposed) {
                return;
            }

            if (_mf != null) {
                _mf.Close();
            }

            if (_md5 != null) {
                _md5.Dispose();
            }

            if (!finalizer) {
                GC.SuppressFinalize(this);
            }
            IsDisposed = true;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Dispose(false);
        }

        ~FileFeatures() {
            Dispose(true);
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