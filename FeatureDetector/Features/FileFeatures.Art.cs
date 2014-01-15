using System;
using System.Collections.Generic;
using System.IO;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo.Arts;
using FileVo = Frost.Common.Models.DB.MovieVo.Files.File;
using File = System.IO.File;

namespace Frost.DetectFeatures {

    public partial class FileFeatures  : IDisposable {
        //private const string XJB_ART_REGEX = "_xjb_(cover|fanart|poster).jpg";
        private const string XJB_ART_REGEX = "(_xjb_)?(cover|fanart|poster).(jpg|bmp|png)";

        private void GetArtInfo() {
            //XJB Cover is always 300x450

            //XJB Fanart is 1280x720 (720p), Resized to Height 720 or width 1280 perserving aspect ratio
            //XJB Fanart Full is 1920x1080 (1080p) or 1280x720 (720p) or 1658x933, Resized to Height 720 or width 1280 perserving aspect ratio

            IEnumerable<FileInfo> arts = _directoryInfo.EnumerateFilesRegex(XJB_ART_REGEX);
            foreach (FileInfo art in arts) {
                if (art.Name.Contains("cover")) {
                    AddXjbArt(art, ArtType.Cover);
                    continue;
                }

                if (art.Name.Contains("fanart")) {
                    AddXjbArt(art, ArtType.Fanart);
                    continue;
                }

                if (art.Name.Contains("poster")) {
                    AddXjbArt(art, ArtType.Poster);
                }
            }
        }

        private void AddXjbArt(FileInfo artFile, ArtType type) {
            string pathFull = artFile.Name + "_full" + artFile.Extension;

            ArtBase art = GetArtType(artFile.FullName, type, File.Exists(pathFull) ? pathFull : null);
            if (art != null) {
                Movie.Arts.Add(art);
            }
        }

        private static ArtBase GetArtType(string path, ArtType type, string pathFull) {
            ArtBase art = null;

            string preview = null;
            if (!string.IsNullOrEmpty(pathFull)) {
                preview = path;
                path = pathFull;
            }

            switch (type) {
                case ArtType.Unknown:
                    art = new Art(path, preview);
                    break;
                case ArtType.Cover:
                    art = new Cover(path, preview);
                    break;
                case ArtType.Poster:
                    art = new Poster(path, preview);
                    break;
                case ArtType.Fanart:
                    art = new Fanart(path, preview);
                    break;
            }
            return art;
        }
    }
}