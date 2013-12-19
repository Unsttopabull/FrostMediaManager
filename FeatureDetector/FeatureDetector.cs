using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures.Util;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;

namespace Frost.DetectFeatures {

    /// <summary>A class used for detecting file information and features.</summary>
    public partial class FeatureDetector : IDisposable {

        private readonly MediaInfoList _mf;
        private readonly string _filePath;
        private readonly string _directoryRegex;
        private readonly DirectoryInfo _directoryInfo;
        private readonly Dictionary<string, FileNameInfo> _fileNameInfos;

        static FeatureDetector() {
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

        /// <summary>Initializes a new instance of the <see cref="FeatureDetector"/> class.</summary>
        /// <param name="filepath">The filepath of the file to check for features.</param>
        public FeatureDetector(string filepath) {
            if (string.IsNullOrEmpty(filepath)) {
                throw new ArgumentNullException("filepath");
            }

            _filePath = filepath;
            _directoryInfo = new DirectoryInfo(Path.GetDirectoryName(filepath) ?? "");
            string directoryPath = _directoryInfo.FullName.Replace("\\", "/");

            _directoryRegex = Regex.Escape(directoryPath).Replace("/", @"[\\/]");

            FileNameParser fnp = new FileNameParser(_filePath);
            FileNameInfo fileNameInfo = fnp.Parse();
            _fileNameInfos = new Dictionary<string, FileNameInfo> {{_filePath, fileNameInfo}};
            _mf = new MediaInfoList();

            _video = new List<Video>();
            _subtitles = new List<Subtitle>();
        }

        public void Detect() {
            //GetSubtitlesForFile(_filePath);
            GetFileInfo(_filePath);
        }

        private FileNameInfo GetFileNameInfo(string fileName) {
            FileNameInfo fnInfo;
            if (!_fileNameInfos.TryGetValue(fileName, out fnInfo)) {
                FileNameParser fnp = new FileNameParser(fileName);
                fnInfo = fnp.Parse();

                _fileNameInfos.Add(fileName, fnInfo);
            }
            return fnInfo;
        }

        public ICollection<Subtitle> Subtitles {
            get { return new List<Subtitle>(_subtitles); }
        }

        public ICollection<Video> Videos {
            get { return new List<Video>(_video); }
        }

        #region IDisposable

        /// <summary>Gets a value indicating whether this object has already been disposed.</summary>
        /// <value>Is <c>true</c> if this object has already been disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (!IsDisposed) {
                _mf.Close();
                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void IDisposable.Dispose() {
            Dispose();
        }

        ~FeatureDetector() {
            Dispose();
        }
        #endregion

    }

}