using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.DetectFeatures.Util;
using Frost.SharpCharsetDetector;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;
using File = System.IO.File;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using FileInfo = Frost.SharpMediaInfo.Output.Properties.General.FileInfo;
using Language = Frost.Common.Models.DB.MovieVo.Language;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private static readonly string[] KnownSubtitleExtensions;
        private static readonly string SubtitleExtensionsRegex;
        private static readonly string[] KnownSubtitleFormats;
        private readonly MD5 _md5 = MD5.Create();

        private void GetSubtitles(FileVo file) {
            //regex from matching files with the same name but with a known subtitle extension
            string regex = string.Format(@"{0}{1}", Regex.Escape(Path.GetFileNameWithoutExtension(file.NameWithExtension) ?? ""), SubtitleExtensionsRegex);
            IEnumerable<MediaListFile> mediaFiles = _directoryInfo.EnumerateFilesRegex(regex)
                                                                  .Select(fi => _mf.GetOrOpen(fi.FullName))
                                                                  .Where(mediaFile => mediaFile != null);

            foreach (MediaListFile mediaFile in mediaFiles) {
                GetSubtitlesInFile(file, mediaFile, _fnInfos[file.NameWithExtension]);
            }
        }

        private void GetSubtitlesInFile(FileVo file, MediaListFile mediaFile, FileNameInfo fnInfo) {
            SubtitleLanguage subLang = GetLanguageAndEncoding(mediaFile.General.FileInfo.FullPath);

            FileInfo fi = mediaFile.General.FileInfo;

            if (mediaFile.Text.Count > 0) {
                foreach (MediaText text in mediaFile.Text) {
                    Subtitle sub;
                    Language languageToCheck = GetLanguage(true, text.Language, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);
                    Language lang = CheckLanguage(languageToCheck);

                    string mediaFormat = text.FormatInfo.Name;

                    //if MediaInfo detected a format and it is a known subtitle format
                    if (mediaFormat != null && Array.BinarySearch(KnownSubtitleFormats, mediaFormat) >= 0) {
                        sub = new Subtitle(null, lang, mediaFormat);
                        sub.File = file;
                    }
                    else {
                        //TODO:check format if its a subtitle
                        sub = new Subtitle(null, lang);
                        sub.File = file;
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

                Subtitle sub = new Subtitle(null, lang);
                sub.File = file;

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