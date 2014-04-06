using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.SharpCharsetDetector;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using Frost.SharpMediaInfo.Output.Properties.General;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private static string _subtitleExtensionsRegex;
        private static List<string> _knownSubtitleExtensions;
        private readonly MD5 _md5 = MD5.Create();

        /// <summary>Subtitle extension to search for when searching for subtitles.</summary>
        public static List<string> KnownSubtitleExtensions {
            get { return _knownSubtitleExtensions; }
            set {
                _knownSubtitleExtensions = value;
                _subtitleExtensionsRegex = string.Format(@"\.({0})", string.Join("|", KnownSubtitleExtensions));
            }
        }

        /// <summary>Known subtitle formats to be detected</summary>
        public static List<string> KnownSubtitleFormats { get; set; }

        private void GetSubtitles(FileDetectionInfo file) {
            GetSubtitlesInFile(file);


            //regex from matching files with the same name but with a known subtitle extension
            string regex = string.Format(@"{0}{1}", Regex.Escape(Path.GetFileNameWithoutExtension(file.NameWithExtension) ?? ""), _subtitleExtensionsRegex);
            var mediaFiles = _directoryInfo.EnumerateFilesRegex(regex)
                                           .Select(fi => new { FullPath = fi.FullName, MediaFile = _mf.GetOrOpen(fi.FullName) })
                                           .Where(sub => sub.MediaFile != null);

            foreach (var mediaFile in mediaFiles) {
                GetSideSubtitles(mediaFile.MediaFile, mediaFile.FullPath);
            }
        }

        private void GetSubtitlesInFile(FileDetectionInfo file) {
            ISOLanguageCode subLang = null;
            ISOLanguageCode audiolang = null;

            string fileName = file.NameWithExtension;
            if (_fnInfos.ContainsKey(fileName)) {
                FileNameInfo fnInfo = _fnInfos[fileName];
                subLang = fnInfo.SubtitleLanguage;
                audiolang = fnInfo.Language;
            }

            MediaListFile mFile = _mf.GetOrOpen(file.FullPath);
            if (mFile == null) {
                return;
            }

            foreach (MediaText text in mFile.Text) {
                SubtitleDetectionInfo sub;
                ISOLanguageCode lang = GetLanguage(true, text.Language, null, subLang, audiolang);

                string mediaFormat = text.FormatInfo.Name;

                //if MediaInfo detected a format and it is a known subtitle format
                if (mediaFormat != null && KnownSubtitleFormats.BinarySearch(mediaFormat) >= 0) {

                    sub = new SubtitleDetectionInfo(lang, mediaFormat);
                }
                else {
                    //TODO:check format if its a subtitle
                    sub = new SubtitleDetectionInfo(lang);

                    switch (mediaFormat) {
                        case "UTF-8":
                            sub.Encoding = "UTF-8";
                            break;
                        case "RLE":
                            sub.Encoding = "RLE";
                            break;
                    }
                }
                sub.EmbededInVideo = true;

                file.Subtitles.Add(sub);
            }
        }

        private void GetSideSubtitles(MediaListFile mediaFile, string fullPath) {
            SubtitleLanguage subLang = GetLanguageAndEncoding(fullPath);

            FileNameInfo fnInfo = null;
            if (_fnInfos.ContainsKey(fullPath)) {
                string fileNameWithExt = Path.GetFileName(fullPath);
                if (!string.IsNullOrEmpty(fileNameWithExt)) {
                    fnInfo = _fnInfos[fileNameWithExt];
                }
            }
            
            if(fnInfo == null){
                fnInfo = GetFileNameInfo(mediaFile.General.FileInfo.FullPath);
            }

            FileDetectionInfo file = GetFile(mediaFile, fullPath);
            if (file == null) {
                return;
            }

            Movie.FileInfos.Add(file);
            if (mediaFile.Text.Count > 0) {
                foreach (MediaText text in mediaFile.Text) {
                    ISOLanguageCode lang = GetLanguage(true, text.Language, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);

                    string mediaFormat = text.FormatInfo.Name;

                    //if MediaInfo detected a format and it is a known subtitle format
                    SubtitleDetectionInfo sub;
                    if (mediaFormat != null && KnownSubtitleFormats.BinarySearch(mediaFormat) >= 0) {
                        sub = new SubtitleDetectionInfo(lang, mediaFormat);
                    }
                    else {
                        //TODO:check format if its a subtitle
                        sub = new SubtitleDetectionInfo(lang);
                    }

                    if (subLang.Encoding != null) {
                        sub.Encoding = subLang.Encoding.WebName;
                    }
                    sub.MD5 = subLang.MD5;

                    file.Subtitles.Add(sub);
                }
            }
            else {
                ISOLanguageCode lang = GetLanguage(true, null, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);

                SubtitleDetectionInfo sub = new SubtitleDetectionInfo(lang);

                if (subLang.Encoding != null) {
                    sub.Encoding = subLang.Encoding.WebName;
                }

                if (subLang.Encoding != null) {
                    sub.Encoding = subLang.Encoding.WebName;
                }
                sub.MD5 = subLang.MD5;

                file.Subtitles.Add(sub);
            }
        }

        private FileNameInfo GetFileNameInfo(string fullPath) {
            if (File.Exists(fullPath)) {
                FileNameParser fnp = new FileNameParser(fullPath);
                return fnp.Parse();
            }
            return new FileNameInfo(fullPath, Path.GetFileName(fullPath), new List<string>());
        }

        private FileDetectionInfo GetFile(MediaListFile mediaFile, string filePath) {
            MediaFileInfo mFileInfo = mediaFile.General.FileInfo;
            if (mFileInfo.FileName != null && mFileInfo.Extension != null && mFileInfo.FolderPath != null) {
                return new FileDetectionInfo(mFileInfo.FileName, mFileInfo.Extension, mFileInfo.FolderPath + "/", mFileInfo.FileSize);
            }

            try {
                FileInfo info = new FileInfo(filePath);
                return new FileDetectionInfo(info);
            }
            catch {
                return null;
            }
        }

        private SubtitleLanguage GetLanguageAndEncoding(string path) {
            if (string.IsNullOrEmpty(path)) {
                return new SubtitleLanguage(null, null);
            }

            Detector detector = DetectorFactory.Create();
            int maxTextLength = detector.MaxTextLength;
            if (maxTextLength < 1024) {
                maxTextLength = 1024;
            }

            int numRead;
            string md5;
            byte[] data = new byte[maxTextLength];
            using (FileStream fs = File.OpenRead(path)) {
                numRead = fs.Read(data, 0, maxTextLength);

                fs.Seek(0, SeekOrigin.Begin);
                md5 = _md5.ComputeHash(fs).Aggregate("", (str, b) => str + b.ToString("x2"));
            }

            Encoding enc = DetectEncoding(data, numRead) ?? Encoding.UTF8;

            detector.Append(enc.GetString(data));
            string detectedLang = null;
            try {
                detectedLang = detector.Detect();
            }
            catch (LangDetectException) {
            }

            return new SubtitleLanguage(enc, detectedLang, md5);
        }

        private Encoding DetectEncoding(byte[] data, int count) {
            //if number of bytes is less than 1kB use all; otherwise use 1kB
            int numBytes = (count < 1024) ? count : 1024;

            UniversalDetector ud = new UniversalDetector();
            ud.Feed(data, 0, numBytes);
            ud.DataEnd();

            return ud.IsSupportedEncoding
                       ? ud.DetectedEncoding
                       : null;
        }
    }

}