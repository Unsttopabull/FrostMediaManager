﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Frost.Common;
using Frost.Common.Annotations;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Models;

namespace Frost.DetectFeatures {

    /// <summary>Represents how the <see cref="FeatureDetector"/> will used the information in a NFO file if found.</summary>
    public enum NFOPriority {
        /// <summary>Ignore the NFO file completely.</summary>
        Ignore,
        /// <summary>If <see cref="FeatureDetector"/> and NFO both contain informaton about a particular feature use the one in the NFO.</summary>
        OverrideDetected,
        /// <summary>Use NFO information only for things not detected by <see cref="FeatureDetector"/>.</summary>
        OnlyNotDetected
    }

    /// <summary>A class used for detecting file information and features.</summary>
    public class FeatureDetector : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _count;
        private readonly string[] _filePaths;
        private static Regex _mediaFileRegex;
        private static List<string> _videoExtensions;
        private const string REGEX_FORMAT = @"(?!sample).*\.({0})$";

        static FeatureDetector() {
            VideoExtensions = new List<string> {
                "3gp", "asf", "avchd", "avi", "ogg", "amc", "amv", "bik", "bsf", "bup", "dash",
                "dvx", "divx", "dv", "dv-avi", "evo", "f4v", "flv", "gom", "gvi", "h264",
                "hdmov", "hdv", "hkm", "k3g", "jts", "kmv", "m15", "m1v", "m21", "m2t",
                "m2ts", "m2v", "m4e", "m4v", "m75", "mj2", "mjp", "mjpg", "mk3d", "mkv",
                "moov", "mov", "movie", "mp2", "ivf", "mp21", "mp2v", "mp4", "mp4v", "mpe",
                "mpeg", "mpeg1", "mpeg4", "mpg", "mpg2", "mpv", "mpv2", "mts", "mtv", "ogm",
                "ogv", "ogx", "qt", "qtm", "rm", "rmd", "rmvb", "rum", "rv", "trp", "ts",
                "vc1", "vfw", "vid", "vob", "vp3", "vp6", "vp7", "vp8", "webm", "wm", "wmv",
                "xmv", "xvid", "yuv", "iso", "ifo"
            };

            _mediaFileRegex = new Regex(string.Format(REGEX_FORMAT, string.Join("|", VideoExtensions)), RegexOptions.IgnoreCase);
        }

        public static List<string> VideoExtensions {
            get { return _videoExtensions; }
            set {
                _videoExtensions = value;
                _mediaFileRegex = new Regex(string.Format(REGEX_FORMAT, string.Join("|", VideoExtensions)), RegexOptions.IgnoreCase);
            }
        }

        public int Count {
            get { return _count; }
            set {
                if (value == _count) return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public FeatureDetector(params string[] filePaths) {
            _filePaths = filePaths;
        }

        public IEnumerable<MovieInfo> Search() {
            Count = 0;
            Task<IEnumerable<MovieInfo>>[] arr = new Task<IEnumerable<MovieInfo>>[_filePaths.Length];

            for (int i = 0; i < _filePaths.Length; i++) {
                string filePath = _filePaths[i];
                if (Directory.Exists(filePath)) {
                    arr[i] = SearchDir(new DirectoryInfo(filePath), true, true);                    
                }
            }

            Task.WaitAll(arr);

            List<MovieInfo> files = new List<MovieInfo>();
            foreach (Task<IEnumerable<MovieInfo>> task in arr) {
                if (task.IsCompleted) {
                    files.AddRange(task.Result);
                }
            }

            return files;
        }

        private async Task<IEnumerable<MovieInfo>> SearchDir(DirectoryInfo directory, bool recursive = true, bool fullDirName = false) {
            //Debug.WriteLine(fullDirName ? directory.FullName : directory.Name, "DIRECTORY");
            //Debug.Indent();

            List<MovieInfo> mf = new List<MovieInfo>();

            FileInfo[] mediaFiles;
            try {
                mediaFiles = directory.EnumerateFilesRegex(_mediaFileRegex, SearchOption.TopDirectoryOnly).ToArray();
            }
            catch (DirectoryNotFoundException e) {
                OutputDirError(e);
                return null;
            }
            catch (SecurityException e) {
                OutputDirError(e);
                return null;
            }

            //foreach (FileInfo fileInfo in mediaFiles) {
            //    Debug.WriteLine(fileInfo.Name, "FILE");
            //}

            if (mediaFiles.Length > 0) {
                //if files are in DVD format (ifo, vob, bup)
                if (mediaFiles.Any(f => f.Extension.OrdinalEquals(".vob") || f.Extension.OrdinalEquals(".ifo") || f.Extension.OrdinalEquals(".bup"))) {
                    mf.Add(await Task.Run(() => DetectDvdMovie(mediaFiles)));
                }
                else {
                    mf.AddRange(await Task.Run(() => DetectMultipartMovie(mediaFiles)));
                }
            }

            if (recursive) {
                //int ident = Debug.IndentLevel;
                try {
                    foreach (DirectoryInfo directoryInfo in directory.EnumerateDirectories()) {
                        mf.AddRange(await SearchDir(directoryInfo));
                    }
                }
                catch (DirectoryNotFoundException e) {
                    OutputDirError(e);
                    //Debug.IndentLevel = ident;
                    return null;
                }
                catch (SecurityException e) {
                    OutputDirError(e);
                    //Debug.IndentLevel = ident;
                    return null;
                }
            }
            //Debug.Unindent();

            //if (fullDirName) {
            //    Debug.WriteLine("");
            //}

            return mf;
        }

        private MovieInfo DetectDvdMovie(FileInfo[] mediaFiles) {
            FileNameInfo[] fnInfos = new FileNameInfo[mediaFiles.Length];
            for (int i = 0; i < mediaFiles.Length; i++) {
                fnInfos[i] = new FileNameParser(mediaFiles[i].FullName, true).Parse();
            }

            Count++;
            return Detect(fnInfos);
        }

        private static void OutputDirError(Exception e) {
            Console.Error.WriteLine("Directory not found or inaccessible.");
            //Debug.WriteLine(string.Format("Directory '{0}' not found or inaccessible.", "PATH"), "ERROR");
            //Debug.Unindent();
        }

        private MovieInfo[] DetectMultipartMovie(FileInfo[] mediaFiles) {
            FileNameParser fnp = new FileNameParser(mediaFiles[0].FullName);
            FileNameInfo info = fnp.Parse();

            if (info.Part != 0) {
                FileNameInfo[] fniArr = new FileNameInfo[mediaFiles.Length];
                fniArr[0] = info;

                for (int i = 1; i < mediaFiles.Length; i++) {
                    fnp = new FileNameParser(mediaFiles[i].FullName);
                    fniArr[i] = fnp.Parse();
                }

                Count++;
                return new[] { Detect(fniArr) };
            }

            MovieInfo[] movies = new MovieInfo[mediaFiles.Length];

            movies[0] = Detect(info);
            Count++;

            for (int i = 1; i < mediaFiles.Length; i++) {
                fnp = new FileNameParser(mediaFiles[i].FullName);
                movies[i] = Detect(fnp.Parse());
                Count++;
            }
            return movies;
        }

        #region Detect

        /// <param name="fnInfos">The file name information of the files to check for features.</param>
        /// <param name="nfoPriority">How to handle information in a NFO file if found.</param>
        public MovieInfo Detect(FileNameInfo[] fnInfos, NFOPriority nfoPriority = NFOPriority.OnlyNotDetected) {
            if (fnInfos == null || (fnInfos != null && fnInfos.Any(fnInfo => fnInfo == null))) {
                throw new ArgumentNullException("fnInfos");
            }

            //Debug.WriteLine(Count + ": " + fnInfos[0].Title, "MOVIE");

            using (FileFeatures file = new FileFeatures(nfoPriority, fnInfos)) {
                return file.Detect() ? file.Movie : null;
            }
        }

        /// <param name="fnInfo">The file name information of the file to check for features.</param>
        /// <param name="nfoPriority">How to handle information in a NFO file if found.</param>
        public MovieInfo Detect(FileNameInfo fnInfo, NFOPriority nfoPriority = NFOPriority.OnlyNotDetected) {
            if (fnInfo == null) {
                throw new ArgumentNullException("fnInfo");
            }

            //Debug.WriteLine(Count + ": " + fnInfo.Title, "MOVIE");

            using (FileFeatures file = new FileFeatures(nfoPriority, fnInfo)) {
                return file.Detect() ? file.Movie : null;
            }
        }

        ///// <param name="filePath">The filepath of the file to check for features.</param>
        ///// <param name="nfoPriority">How to handle information in a NFO file if found.</param>
        //public Movie Detect(string filePath, NFOPriority nfoPriority = NFOPriority.OnlyNotDetected) {
        //    if (string.IsNullOrEmpty(filePath)) {
        //        throw new ArgumentNullException("filePath");
        //    }

        //    Debug.WriteLine(++_count + ": " + filePath, "MOVIE");

        //    using (FileFeatures file = new FileFeatures(nfoPriority, filePath)) {
        //        return file.Detect() ? file.Movie : null;
        //    }
        //}

        ///// <param name="filePaths">The filepaths of the files to check for features.</param>
        ///// <param name="nfoPriority">How to handle information in a NFO file if found.</param>
        //public Movie Detect(string[] filePaths, NFOPriority nfoPriority = NFOPriority.OnlyNotDetected) {
        //    if (filePaths == null || (filePaths != null && filePaths.Any(fnInfo => fnInfo == null))) {
        //        throw new ArgumentNullException("filePaths");
        //    }

        //    Debug.WriteLine(++_count + ": " + filePaths[0], "MOVIE");

        //    using (FileFeatures file = new FileFeatures(nfoPriority, filePaths)) {
        //        return file.Detect() ? file.Movie : null;
        //    }
        //}
        #endregion

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}