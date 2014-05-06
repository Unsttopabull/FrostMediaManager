using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Frost.Common.Models.FeatureDetector;
using Frost.Common.Models.Provider;

namespace Frost.DetectFeatures {

    public partial class FileFeatures : IDisposable {
        private void GetNfoInfo(string fileNameWithoutExt) {
            FileInfo[] xbmcNfo = _directoryInfo.EnumerateFiles("*.nfo").ToArray();
            if (xbmcNfo.Length > 0) {
                GetXbmcNfoInfo(fileNameWithoutExt, xbmcNfo);
                return;
            }

            FileInfo xtNfo = _directoryInfo.EnumerateFiles(fileNameWithoutExt + "_xjb.xml").FirstOrDefault();
            if (xtNfo == null) {
                return;
            }

            if (xtNfo.Exists) {
                GetXtreamerNfoInfo(xtNfo.FullName);
            }
            else {
                Debug.WriteLine("File: " + xtNfo.Name + " is not accessible.", "ERROR");
            }
        }

        private bool CheckReleaseYear(long? year) {
            return year != null && year != 0 && year != 1;
        }

        private void AddActors<T>(IEnumerable<T> actors, bool overrideValues = false) where T : IXmlActor {
            foreach (T actor in actors) {
                if (string.IsNullOrEmpty(actor.Name)) {
                    continue;
                }

                ActorInfo movieActor = Movie.Actors.FirstOrDefault(a => a.Name == actor.Name);
                if (movieActor != null) {
                    if (overrideValues) {
                        //exists so just update
                        if (!string.IsNullOrEmpty(actor.Role)) {
                            movieActor.Character = actor.Role;
                        }
                        if (!string.IsNullOrEmpty(actor.Thumb)) {
                            movieActor.Thumb = actor.Thumb;
                        }
                    }
                    else {
                        //exists so just update
                        if (string.IsNullOrEmpty(movieActor.Character) && !string.IsNullOrEmpty(actor.Role)) {
                            movieActor.Character = actor.Role;
                        }

                        if (string.IsNullOrEmpty(movieActor.Thumb) && !string.IsNullOrEmpty(actor.Thumb)) {
                            movieActor.Thumb = actor.Thumb;
                        }
                    }
                }
                else {
                    //create new person
                    Movie.Actors.Add(new ActorInfo(actor.Name, actor.Thumb));
                }
            }
        }

        private void AddSet(string setName) {
            if (!string.IsNullOrEmpty(setName)) {
                Movie.Set = setName;
            }
        }

        private void AddStudio(string studioName) {
            if (!string.IsNullOrEmpty(studioName) && Movie.Studios.All(s => s != studioName)) {
                Movie.Studios.Add(studioName);
            }
        }

        private void AddDirector(string directorName) {
            if (!string.IsNullOrEmpty(directorName)) {
                PersonInfo director = Movie.Directors.FirstOrDefault(p => p.Name == directorName);
                if (director != null) {
                    return;
                }

                director = new PersonInfo(directorName);
                if (!Movie.Directors.Contains(director)) {
                    Movie.Directors.Add(director);
                }
            }
        }
    }

}