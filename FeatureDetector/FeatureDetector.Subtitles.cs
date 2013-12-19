using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.Util;
using Frost.SharpCharsetDetector;
using Frost.SharpLanguageDetect;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;

using File = System.IO.File;
using MFileInfo = Frost.SharpMediaInfo.Output.Properties.General.FileInfo;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using Language = Frost.Common.Models.DB.MovieVo.Language;

namespace Frost.DetectFeatures {

    public partial class FeatureDetector {
        private static readonly string[] KnownSubtitleExtensions;
        private static readonly string SubtitleExtensionsRegex;
        private static readonly string[] KnownSubtitleFormats;
        private readonly List<Subtitle> _subtitles;

        public List<Subtitle> GetSubtitlesForFile(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException("fileName");
            }

            FileNameInfo fnInfo = GetFileNameInfo(fileName);

            //regex from matching files with the same name but with a known subtitle extension
            //string regex = string.Format(@"{0}[\\/]{1}{2}", _directoryRegex, fileNameRegex, SubtitleExtensionsRegex);
            string regex = string.Format(@"{0}{1}", Regex.Escape(Path.GetFileNameWithoutExtension(fileName)), SubtitleExtensionsRegex);
            IEnumerable<MediaListFile> mediaFiles = _directoryInfo.EnumerateFilesRegex(regex)
                                                                  .Select(fi => _mf.GetOrOpen(fi.FullName))
                                                                  .Where(mediaFile => mediaFile != null);

            //if (filesWithPattern.Count > 0) {
              foreach (MediaListFile mediaFile in mediaFiles) {
                  GetSubtitlesInFile(mediaFile, fnInfo);
              }
            //}
            //else {
            //    OutputError(fileName, fileNameRegex);
            //}

            return _subtitles;
        }

        private void OutputError(string fileName, string fileNameRegex) {
            Console.Error.WriteLine("#region " + fileName);
            Console.Error.WriteLine("Matching files not found!");
            Console.Error.WriteLine("Searched for: {0}[\\/]{1}", _directoryRegex, fileNameRegex);

            Console.Error.WriteLine();
            Console.Error.WriteLine("Listing files in the directory:");

            try {
                foreach (string file in _directoryInfo.EnumerateFiles().Select(fi => fi.Name)) {
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

        private int GetSubtitlesInFile(MediaListFile mediaFile, FileNameInfo fnInfo) {
            SubtitleLanguage subLang = GetLanguageAndEncoding(mediaFile.General.FileInfo.FullPath);

            MFileInfo fi = mediaFile.General.FileInfo;
            FileVo file = new FileVo(fi.FileName, fi.Extension, fi.FolderPath, fi.FileSize);

            int num = 0;
            if (mediaFile.Text.Count > 0) {
                foreach (MediaText text in mediaFile.Text) {
                    Language lang = GetLanguage(text.Language, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);

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
                    _subtitles.Add(sub);
                    num++;
                }
            }
            else {
                Language lang = GetLanguage(null, subLang.Language, fnInfo.SubtitleLanguage, fnInfo.Language);
                Subtitle sub = new Subtitle(file, lang);

                if (subLang.Encoding != null) {
                    sub.Encoding = subLang.Encoding.WebName;
                }

                //Console.WriteLine(sub);
                _subtitles.Add(sub);
                num++;
            }
            return num;
        }

        private static Language GetLanguage(string mediaInfoLang, ISOLanguageCode detectedLangCode, ISOLanguageCode subLangCode, ISOLanguageCode langCode) {
            if (mediaInfoLang != null) {
                return Language.FromISO639(mediaInfoLang);
            }

            if (detectedLangCode != null) {
                return new Language(detectedLangCode);
            }

            if (subLangCode != null) {
                return new Language(subLangCode);
            }

            if (langCode != null) {
                return new Language(langCode);
            }
            return null;
        }

        private SubtitleLanguage GetLanguageAndEncoding(string path) {
            Detector detector = DetectorFactory.Create();
            int maxTextLength = detector.MaxTextLength;
            if (maxTextLength < 1024) {
                maxTextLength = 1024;
            }

            int numRead;
            byte[] data = new byte[maxTextLength];
            using (FileStream fs = File.OpenRead(path)) {
                numRead = fs.Read(data, 0, maxTextLength);
            }

            Encoding enc = DetectEncoding(data, numRead) ?? Encoding.UTF8;

            detector.Append(enc.GetString(data));
            string detectedLang = null;
            try {
                detectedLang = detector.Detect();
            }
            catch (LangDetectException) {
            }

            return new SubtitleLanguage(enc, detectedLang);
        }

        private Encoding DetectEncoding(byte[] data, int count) {
            int numBytes = (count < 1024) ? count : 1024;

            UniversalDetector ud = new UniversalDetector();
            ud.Feed(data, 0, numBytes);
            ud.DataEnd();

            try {
                return ud.IsSupportedEncoding
                    ? ud.DetectedEncoding
                    : null;
            }
            catch (Exception e) {
                Console.Error.WriteLine("ERROR: "+e.Message);
            }
            return null;
        }
    }

}