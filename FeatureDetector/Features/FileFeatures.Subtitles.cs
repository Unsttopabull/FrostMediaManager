using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.Models.Frost.DB.Files;
using Frost.SharpCharsetDetector;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using Frost.SharpMediaInfo.Output.Properties.General;
using File = System.IO.File;
using FileVo = Frost.Models.Frost.DB.Files.File;
using Language = Frost.Models.Frost.DB.Language;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private static string _subtitleExtensionsRegex;
        private readonly MD5 _md5 = MD5.Create();

        /// <summary>Subtitle extension to search for when searching for subtitles.</summary>
        public static List<string> KnownSubtitleExtensions { get; set; }

        /// <summary>Known subtitle formats to be detected</summary>
        public static List<string> KnownSubtitleFormats { get; set; }

        private void GetSubtitles(FileVo file) {
            GetSubtitlesInFile(file);


            //regex from matching files with the same name but with a known subtitle extension
            string regex = string.Format(@"{0}{1}", Regex.Escape(Path.GetFileNameWithoutExtension(file.NameWithExtension) ?? ""), _subtitleExtensionsRegex);
            IEnumerable<MediaListFile> mediaFiles = _directoryInfo.EnumerateFilesRegex(regex)
                                                                  .Select(fi => _mf.GetOrOpen(fi.FullName))
                                                                  .Where(mediaFile => mediaFile != null);

            foreach (MediaListFile mediaFile in mediaFiles) {
                GetSideSubtitles(mediaFile, _fnInfos[file.NameWithExtension]);
            }
        }

        private void GetSubtitlesInFile(FileVo file) {
            ISOLanguageCode subLang = null;
            ISOLanguageCode audiolang = null;

            string fileName = file.NameWithExtension;
            if (_fnInfos.ContainsKey(fileName)) {
                FileNameInfo fnInfo = _fnInfos[fileName];
                subLang = fnInfo.SubtitleLanguage;
                audiolang = fnInfo.Language;
            }

            MediaListFile mFile = _mf.GetOrOpen(file.FullPath);
            foreach (MediaText text in mFile.Text) {
                Subtitle sub;
                Language lang = CheckLanguage(GetLanguage(true, text.Language, null, subLang, audiolang));

                string mediaFormat = text.FormatInfo.Name;

                //if MediaInfo detected a format and it is a known subtitle format
                if (mediaFormat != null && KnownSubtitleFormats.BinarySearch(mediaFormat) >= 0) {
                    sub = new Subtitle(file, lang, mediaFormat);
                }
                else {
                    //TODO:check format if its a subtitle
                    sub = new Subtitle(file, lang);

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

                Movie.Subtitles.Add(sub);
            }
        }

        private void GetSideSubtitles(MediaListFile mediaFile, FileNameInfo fnInfo) {
            SubtitleLanguage subLang = GetLanguageAndEncoding(mediaFile.General.FileInfo.FullPath);

            FileVo file = GetFile(mediaFile, fnInfo.FilePath);
            if (file == null) {
                return;
            }

            if (mediaFile.Text.Count > 0) {
                foreach (MediaText text in mediaFile.Text) {
                    Subtitle sub;
                    Language languageToCheck = GetLanguage(true, text.Language, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);
                    Language lang = CheckLanguage(languageToCheck);

                    string mediaFormat = text.FormatInfo.Name;

                    //if MediaInfo detected a format and it is a known subtitle format
                    if (mediaFormat != null && KnownSubtitleFormats.BinarySearch(mediaFormat) >= 0) {
                        sub = new Subtitle(file, lang, mediaFormat);
                    }
                    else {
                        //TODO:check format if its a subtitle
                        sub = new Subtitle(file, lang);
                    }

                    if (subLang.Encoding != null) {
                        sub.Encoding = subLang.Encoding.WebName;
                    }
                    sub.MD5 = subLang.MD5;

                    Movie.Subtitles.Add(sub);
                }
            }
            else {
                Language languageToCheck = GetLanguage(true, null, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);
                Language lang = CheckLanguage(languageToCheck);

                Subtitle sub = new Subtitle(file, lang);

                if (subLang.Encoding != null) {
                    sub.Encoding = subLang.Encoding.WebName;
                }

                if (subLang.Encoding != null) {
                    sub.Encoding = subLang.Encoding.WebName;
                }
                sub.MD5 = subLang.MD5;

                Movie.Subtitles.Add(sub);
            }
        }

        private FileVo GetFile(MediaListFile mediaFile, string filePath) {
            MediaFileInfo mFileInfo = mediaFile.General.FileInfo;
            if (mFileInfo.FileName != null && mFileInfo.Extension != null && mFileInfo.FolderPath != null) {
                return new FileVo(mFileInfo.FileName, mFileInfo.Extension, mFileInfo.FolderPath + "/", mFileInfo.FileSize);
            }

            try {
                FileInfo info = new FileInfo(filePath);
                return new FileVo(info);
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