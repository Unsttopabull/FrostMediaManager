using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Frost.Common;
using Frost.Common.Models.DB.MovieVo.Arts;
using Frost.Common.Models.DB.MovieVo.People;
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

            DirectoryInfo[] actorArt = _directoryInfo.GetDirectories(".actors");
            if (actorArt.Length > 0) {
                GetActorsThumbs(actorArt[0]);
            }
        }

        private void GetActorsThumbs(DirectoryInfo actorsDir) {
            foreach (FileInfo file in actorsDir.EnumerateFiles("*.jpg")) {
                string actorName = Path.GetFileNameWithoutExtension(file.Name).Replace('_', ' ');

                //Check if person has already been added to the movie's actor list
                MovieActor actor = Movie.ActorsLink.FirstOrDefault(al => al.Person.Name == actorName);
                if (actor != null) {
                    //if the actor is missing a thumbnails add it
                    if (string.IsNullOrEmpty(actor.Person.Thumb)) {
                        actor.Person.Thumb = file.FullName;
                    }
                }
                else {
                    //check if a person with the same name already exists in the DB
                    Person person = _mvc.People.FirstOrDefault(p => p.Name == actorName);
                    if (person != null) {
                        //if it does check for missing thumbnail
                        if (string.IsNullOrEmpty(person.Thumb)) {
                            person.Thumb = file.FullName;
                        }
                        //add the person to the movie as actor
                        Movie.ActorsLink.Add(new MovieActor(Movie, person, null));
                    }
                    else {
                        //add new person to the DB and add it to the movie's list of actors
                        Movie.ActorsLink.Add(new MovieActor(Movie, new Person(actorName, file.FullName), null));
                    }
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