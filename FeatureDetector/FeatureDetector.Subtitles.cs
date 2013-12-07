using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.Files;
using Frost.SharpMediaInfo;
using Frost.SharpMediaInfo.Output;

namespace Frost.DetectFeatures {

    public partial class FeatureDetector {

        private static readonly string[] KnownSubtitleExtensions;
        private static readonly string SubtitleExtensionsRegex;
        private static readonly string[] KnownSubtitleFormats;
        private List<Subtitle> _subtitles;

        public IEnumerable<Subtitle> GetSubtitlesForFile(string fileName) {
            if (string.IsNullOrEmpty(fileName)) {
                throw new ArgumentNullException("fileName");
            }

            //regex from matching files with the same name but with a known subtitle extension
            string regex = string.Format(@"{0}[\\/]{1}{2}", _directoryRegex, Path.GetFileNameWithoutExtension(fileName), SubtitleExtensionsRegex);
            foreach (MediaListFile file in _mf.GetFilesWithPattern(regex)) {
                foreach (MediaText text in file.Text) {
                    Subtitle sub;

                    //if MediaInfo detected a format and it is a known subtitle format
                    string formatName = text.FormatInfo.Name;
                    if (formatName != null && Array.Exists(KnownSubtitleFormats, format => formatName == format)) {
                        sub = new Subtitle(text.Language, text.FormatInfo.Name);
                    }
                    else {
                        //TODO:check format if it is a subtitle
                        sub = new Subtitle(text.Language);
                    }

                    if (string.IsNullOrEmpty(sub.Language.Name)) {
                        Language lang =  TryParseSubtitleLanguage(file.General.FileInfo.FileName);
                        if (lang != null) {
                            sub.Language = lang;
                        }
                    }
                    _subtitles.Add(sub);
                }
            }

            return new List<Subtitle>(_subtitles);
        }

        private Language TryParseSubtitleLanguage(string fileNameWithExtension) {
            return null;
        }
    }

}