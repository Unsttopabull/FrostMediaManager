using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Frost.Common.Models.DB.MovieVo;
using Frost.Common.Models.DB.MovieVo.ISO;
using Frost.Common.Models.DB.MovieVo.People;
using Frost.Common.Models.XML.XBMC;
using Frost.Common.Util.ISO;

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

        private void AddActors<T>(IEnumerable<T> actors, bool overrideValues = false) where T : XbmcXmlActor {
            foreach (T actor in actors) {
                if (string.IsNullOrEmpty(actor.Name)) {
                    continue;
                }

                MovieActor movieActor = Movie.ActorsLink.FirstOrDefault(a => a.Person.Name == actor.Name);
                if (movieActor != null) {
                    if (overrideValues) {
                        //exists so just update
                        if (!string.IsNullOrEmpty(actor.Role)) {
                            movieActor.Character = actor.Role;
                        }
                        if (!string.IsNullOrEmpty(actor.Thumb)) {
                            movieActor.Person.Thumb = actor.Thumb;
                        }
                    }
                    else {
                        //exists so just update
                        if (string.IsNullOrEmpty(movieActor.Character) && !string.IsNullOrEmpty(actor.Role)) {
                            movieActor.Character = actor.Role;
                        }

                        if (string.IsNullOrEmpty(movieActor.Person.Thumb) && !string.IsNullOrEmpty(actor.Thumb)) {
                            movieActor.Person.Thumb = actor.Thumb;
                        }
                    }
                }
                else {
                    string character = null;

                    //is not on the movie list yet so get the actor from DB or create new one
                    Person person = _mvc.People.FirstOrDefault(p => p.Name == actor.Name);

                    if (person != null) {
                        if (overrideValues && !string.IsNullOrEmpty(actor.Thumb)) {
                            person.Thumb = actor.Thumb;
                        }
                        else if(!overrideValues) {
                            //if the DB person is missing a thumb but it exists in NFO add it
                            if (string.IsNullOrEmpty(person.Thumb) && !string.IsNullOrEmpty(actor.Thumb)) {
                                person.Thumb = actor.Thumb;
                            }
                        }

                        if (!string.IsNullOrEmpty(actor.Role)) {
                            character = actor.Role;
                        }
                    }
                    else {
                        //create new person
                        person = new Person(actor.Name, actor.Thumb);
                    }
                    Movie.ActorsLink.Add(new MovieActor(Movie, person, character));
                }
            }
        }

        private void AddSet(string setName) {
            if (!string.IsNullOrEmpty(setName)) {
                Set set = _mvc.Sets.FirstOrDefault(s => s.Name == setName);
                if (set == null) {
                    Movie.Set = new Set(setName);
                }
            }
        }

        private void AddGenre(Genre genre) {
            if (!string.IsNullOrEmpty(genre.Name) && !Movie.Genres.Contains(genre)) {
                Genre gnr = _mvc.Genres.FirstOrDefault(g => g.Name == genre.Name) ?? genre;
                Movie.Genres.Add(gnr);
            }
        }

        private void AddStudio(string studioName) {
            if (!string.IsNullOrEmpty(studioName) && Movie.Studios.All(s => s.Name != studioName)) {
                Studio studio = _mvc.Studios.FirstOrDefault(s => s.Name == studioName) ?? new Studio(studioName);
                Movie.Studios.Add(studio);
            }
        }

        private void AddDirector(string directorName) {
            if (!string.IsNullOrEmpty(directorName)) {
                Person director = Movie.Directors.FirstOrDefault(p => p.Name == directorName);
                if (director != null) {
                    return;
                }

                director = _mvc.People.FirstOrDefault(p => p.Name == directorName) ?? new Person(directorName);
                if (!Movie.Directors.Contains(director)) {
                    Movie.Directors.Add(director);
                }
            }
        }

        private void AddCertification(Certification certification) {
            certification.Country = CheckCountry(certification.Country);

            Movie.Certifications.Add(certification);
        }

        private Country CheckCountry(Country countryToCheck) {
            if (countryToCheck == null) {
                return null;
            }

            string name = countryToCheck.Name;

            //possible ISO 3166 code
            if (name.Length == 2 || name.Length == 3) {
                Country country = Country.FromISO3166(name);
                if (country != null) {
                    return _mvc.Countries.FirstOrDefault(c => c.Name == country.Name) ?? country;
                }

                country = _mvc.Countries.FirstOrDefault(c => c.Name == name);
                if (country != null) {
                    return country;
                }
            }
            else {
                Country dbCountry = _mvc.Countries.FirstOrDefault(c => c.Name == name);
                if (dbCountry != null) {
                    return dbCountry;
                }

                //the country is not in the DB yet
                //check if ISO codes are already present
                ISO3166 iso3166 = countryToCheck.ISO3166;
                if (string.IsNullOrEmpty(iso3166.Alpha3) && string.IsNullOrEmpty(iso3166.Alpha2)) {
                    //if not try to obtain them
                    ISOCountryCode code = ISOCountryCodes.Instance.GetByEnglishName(name);
                    if (code != null) {
                        return new Country(code);
                    }
                }
            }
            return countryToCheck;
        }

        private Language CheckLanguage(Language languageToCheck) {
            if (languageToCheck == null) {
                return null;
            }

            string name = languageToCheck.Name;

            //possible ISO 3166 code
            if (name.Length == 2 || name.Length == 3) {
                Language language = CheckIfNameIsIsoCode(name);
                if (language != null) {
                    return language;
                }
            }
            else {
                Language dbCountry = _mvc.Languages.FirstOrDefault(language => language.Name == name);
                if (dbCountry != null) {
                    return dbCountry;
                }

                //the country is not in the DB yet
                //check if ISO codes are already present
                ISO639 iso639 = languageToCheck.ISO639;
                if (string.IsNullOrEmpty(iso639.Alpha3) && string.IsNullOrEmpty(iso639.Alpha2)) {
                    //if not try to obtain them
                    ISOLanguageCode code = ISOLanguageCodes.Instance.GetByEnglishName(name);
                    if (code != null) {
                        if (_mvc.Languages.FirstOrDefault(language => language.Name == code.EnglishName) != null) {
                            dbCountry = _mvc.Languages.FirstOrDefault(language => language.Name == code.EnglishName);
                        }
                        else {
                            dbCountry = new Language(code);
                        }
                        return dbCountry;
                    }
                }
            }
            return languageToCheck;
        }

        private Language CheckIfNameIsIsoCode(string name) {
            Language language = Language.FromISO639(name);
            if (language != null) {
                return _mvc.Languages.FirstOrDefault(l => l.Name == language.Name) ?? language;
            }

            return _mvc.Languages.FirstOrDefault(l => l.Name == name);
        }
    }

}