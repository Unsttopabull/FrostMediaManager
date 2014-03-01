using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.DetectFeatures.Models;

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

            DirectoryInfo[] actorArt = _directoryInfo.GetDirectories(".actors");
            if (actorArt.Length > 0) {
                GetActorsThumbs(actorArt[0]);
            }
        }

        private void GetActorsThumbs(DirectoryInfo actorsDir) {
            foreach (FileInfo file in actorsDir.EnumerateFiles("*.jpg")) {
                string actorName = Path.GetFileNameWithoutExtension(file.Name).Replace('_', ' ');

                //Check if person has already been added to the movie's actor list
                ActorInfo actor = Movie.Actors.FirstOrDefault(al => al.Name == actorName);
                if (actor != null) {
                    //if the actor is missing a thumbnails add it
                    if (string.IsNullOrEmpty(actor.Thumb)) {
                        actor.Thumb = file.FullName;
                    }
                }
                else {
                    //add new person to the DB and add it to the movie's list of actors
                    Movie.Actors.Add(new ActorInfo(actorName, file.FullName));
                }
            }
        }

        private void AddXjbArt(FileInfo artFile, ArtType type) {
            string pathFull = artFile.Name + "_full" + artFile.Extension;

            ArtInfo art = GetArtType(artFile.FullName, type, File.Exists(pathFull) ? pathFull : null);
            if (art != null) {
                Movie.Art.Add(art);
            }
        }

        private static ArtInfo GetArtType(string path, ArtType type, string pathFull) {
            string preview = null;

            if (!string.IsNullOrEmpty(pathFull)) {
                preview = path;
                path = pathFull;
            }

            return new ArtInfo(type, path, preview);
        }
    }
}