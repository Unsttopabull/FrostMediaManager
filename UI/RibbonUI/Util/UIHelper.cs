using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Frost.Common.Models.Provider;
using Frost.Common.Models.Provider.ISO;
using Frost.Common.Util.ISO;
using Frost.GettextMarkupExtension;
using log4net;
using RibbonUI.Design.Models;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Util {

    public static class UIHelper {
        private static List<ILanguage> _languages;
        private static List<ICountry> _countries;
        private static List<IStudio> _studios;

        private static readonly string[] ExculsionList = {
            "Ancient",
            "Ancient",
            "Old",
            "Middle",
            "languages",
            "Languages",
            "Ottoman",
            "pidgin",
            "BCE",
            "post"
        };

        public static IEnumerable<MovieLanguage> LanguagesWithImages {
            get { return GetLanguagesWithImages().Select(l => new MovieLanguage(new DesignLanguage(l))); }
        }

        public static IEnumerable<MovieLanguage> Languages {
            get { return GetLanguages().Select(l => new MovieLanguage(l)); }
        }

        public static IEnumerable<ISOLanguageCode> GetLanguagesWithImages() {
            if (!Directory.Exists("Images/Languages")) {
                return null;
            }

            return Directory.EnumerateFiles("Images/Languages", "*.png")
                            .Select(fileName => ISOLanguageCodes.Instance.GetByISOCode(Path.GetFileNameWithoutExtension(fileName)))
                            .Where(isoCode => isoCode != null)
                            .OrderBy(isoCode => isoCode.EnglishName)
                            .ToList();
        }

        public static IEnumerable<ILanguage> GetLanguages() {
            if (_languages != null) {
                return _languages;
            }

            _languages = new List<ILanguage>();

            IEnumerable<ISOLanguageCode> codes = ISOLanguageCodes.GetAllKnownCodes();

            bool canCreateLang = LightInjectContainer.CanGetInstance<ILanguage>();
            foreach (ISOLanguageCode lang in codes) {
                if (ExculsionList.Any(s => lang.EnglishName.Contains(s))) {
                    continue;
                }

                ILanguage l = canCreateLang
                                  ? LightInjectContainer.GetInstance<ILanguage>()
                                  : new DesignLanguage();

                l.Name = lang.EnglishName;
                l.ISO639 = new ISO639(lang.Alpha2, lang.Alpha3);

                _languages.Add(l);
            }
            return _languages;
        }

        public static IEnumerable<ISOLanguageCode> GetISOLanguages() {
            IEnumerable<ISOLanguageCode> codes = ISOLanguageCodes.GetAllKnownCodes();
            return codes.Where(lang => !ExculsionList.Any(s => lang.EnglishName.Contains(s))).ToList();
        }

        public static IEnumerable<ICountry> GetCountries() {
            if (_countries != null) {
                return _countries;
            }

            _countries = new List<ICountry>();

            IEnumerable<ISOCountryCode> codes = ISOCountryCodes.GetAllKnownCodes();

            bool canCreateLang = LightInjectContainer.CanGetInstance<ICountry>();
            foreach (ISOCountryCode lang in codes) {
                if (ExculsionList.Any(s => lang.EnglishName.Contains(s))) {
                    continue;
                }

                ICountry c = canCreateLang
                                 ? LightInjectContainer.GetInstance<ICountry>()
                                 : new DesignCountry();

                c.Name = lang.EnglishName;
                c.ISO3166 = new ISO3166(lang.Alpha2, lang.Alpha3);

                _countries.Add(c);
            }
            return _countries;
        }

        public static void ProviderCouldNotRemove() {
            MessageBox.Show(Gettext.T("Provider could not remove the item.\nProbable causes:\n\t* Item does not exists in the store\n\t* An error has occured."));
        }

        public static void ProviderCouldNotAdd() {
            MessageBox.Show(Gettext.T("Error: Provider could not add the item.\nPlease contact provider creator."));
        }

        public static IEnumerable<IStudio> GetStudios() {
            if (_studios != null) {
                return _studios;
            }

            _studios = new List<IStudio>();

            if (Directory.Exists("Images/StudiosE")) {
                foreach (string file in Directory.EnumerateFiles("Images/StudiosE")) {
                    _studios.Add(new DesignStudio(file));
                }
            }
            return _studios;
        }

        public static void HandleProviderException(ILog log, Exception exception) {
            string exceptionText;
            if (exception is NotSupportedException) {
                exceptionText = string.Format("Provider does not support the requested operation.{0}", !string.IsNullOrEmpty(exception.Message) ? "\nProvider message: " + exception.Message : null);
                if (log.IsErrorEnabled) {
                    log.Error(exceptionText, exception);
                }

                MessageBox.Show(exceptionText);
                return;
            }

            if (exception is NotImplementedException) {
                exceptionText = string.Format("Provider has not implemented the requested operation.{0}", !string.IsNullOrEmpty(exception.Message) ? "\nProvider message: " + exception.Message : null);

                if (log.IsErrorEnabled) {
                    log.Error(exceptionText, exception);
                }

                MessageBox.Show(exceptionText);
                return;
            }

            exceptionText = string.Format("An error has occured int the provider{0}", !string.IsNullOrEmpty(exception.Message) ? "\nProvider message: " + exception.Message : null);
            if (log.IsErrorEnabled) {
                log.Error(exceptionText, exception);
            }

            MessageBox.Show(exceptionText);
        }
    }

    public enum PluginType {
        Art,
        MovieInfo,
        Subtitles,
        Videos
    }

}