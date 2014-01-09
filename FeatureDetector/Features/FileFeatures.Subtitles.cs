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

using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using FileInfo = Frost.SharpMediaInfo.Output.Properties.General.FileInfo;
using Language = Frost.Common.Models.DB.MovieVo.Language;

namespace Frost.DetectFeatures {
    public partial class FileFeatures : IDisposable {
        private static readonly string[] KnownSubtitleExtensions;
        private static readonly string SubtitleExtensionsRegex;
        private static readonly string[] KnownSubtitleFormats;
        private static readonly MD5 Md5 = MD5.Create();

        private void GetSubtitles() {
            //regex from matching files with the same name but with a known subtitle extension
            string regex = string.Format(@"{0}{1}", Regex.Escape(_file.Name), SubtitleExtensionsRegex);
            IEnumerable<MediaListFile> mediaFiles = _directoryInfo.EnumerateFilesRegex(regex)
                                                                  .Select(fi => {
                                                                      lock(_mf) {
                                                                        return _mf.GetOrOpen(fi.FullName);
                                                                      }
                                                                  })
                                                                  .Where(mediaFile => mediaFile != null);

            //if (filesWithPattern.Count > 0) {
            foreach (MediaListFile mediaFile in mediaFiles) {
                GetSubtitlesInFile(mediaFile, _fnInfo);
            }
            //}
            //else {
            //    OutputError(fileInfo, fileName, fileNameRegex);
            //}
        }

        private void OutputError(FileFeatures fileFeatures, string fileName, string fileNameRegex) {
            Console.Error.WriteLine("#region " + fileName);
            Console.Error.WriteLine("Matching files not found!");
            Console.Error.WriteLine("Searched for: {0}[\\/]{1}", fileFeatures._directoryRegex, fileNameRegex);

            Console.Error.WriteLine();
            Console.Error.WriteLine("Listing files in the directory:");

            try {
                foreach (string file in fileFeatures._directoryInfo.EnumerateFiles().Select(fi => fi.Name)) {
                    Console.Error.WriteLine("\t" + file);
                }
            }
            catch (IOException e) {
                Console.Error.WriteLine("ERROR: " + e.Message);
            }
            Console.Error.WriteLine();
            Console.Error.WriteLine("#endregion");
            Console.Error.WriteLine();
        }

        private void GetSubtitlesInFile(MediaListFile mediaFile, FileNameInfo fnInfo) {
            SubtitleLanguage subLang = GetLanguageAndEncoding(mediaFile.General.FileInfo.FullPath);

            FileInfo fi = mediaFile.General.FileInfo;
            FileVo file = new FileVo(fi.FileName, fi.Extension, fi.FolderPath, fi.FileSize);

            if (mediaFile.Text.Count > 0) {
                foreach (MediaText text in mediaFile.Text) {
                    Language lang = GetLanguage(true, text.Language, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);

                    Subtitle sub;
                    string mediaFormat = text.FormatInfo.Name;

                    //if MediaInfo detected a format and it is a known subtitle format
                    if (mediaFormat != null && Array.BinarySearch(KnownSubtitleFormats, mediaFormat) >= 0) {
                        sub = new Subtitle(file, lang, mediaFormat);
                    }
                    else {
                        //TODO:check format if its a subtitle
                        sub = new Subtitle(file, lang);
                    }

                    if (subLang.Encoding != null) {
                        sub.Encoding = subLang.Encoding.WebName;
                    }

                    //Console.WriteLine(sub);
                    Movie.Subtitles.Add(sub);
                }
            }
            else {
                Language lang = GetLanguage(true, null, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);
                Subtitle sub = new Subtitle(file, lang);

                if (subLang.Encoding != null) {
                    sub.Encoding = subLang.Encoding.WebName;
                }

                //Console.WriteLine(sub);
                Movie.Subtitles.Add(sub);
            }
        }

        private SubtitleLanguage GetLanguageAndEncoding(string path) {
            Detector detector = DetectorFactory.Create();
            int maxTextLength = detector.MaxTextLength;
            if (maxTextLength < 1024) {
                maxTextLength = 1024;
            }

            int numRead;
            string md5;
            byte[] data = new byte[maxTextLength];
            using (FileStream fs = System.IO.File.OpenRead(path)) {
                numRead = fs.Read(data, 0, maxTextLength);

                fs.Seek(0, SeekOrigin.Begin);
                md5 = Md5.ComputeHash(fs).Aggregate("", (str, b) => str + b.ToString("x2"));
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

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            Md5.Dispose();
        }
    }
}