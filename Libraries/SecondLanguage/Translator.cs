#region License

/*
SecondLanguage Gettext Library for .NET
Copyright 2013 James F. Bellinger <http://www.zer7.com>

This software is provided 'as-is', without any express or implied
warranty. In no event will the authors be held liable for any damages
arising from the use of this software.

Permission is granted to anyone to use this software for any purpose,
including commercial applications, and to alter it and redistribute it
freely, subject to the following restrictions:

1. The origin of this software must not be misrepresented; you must
not claim that you wrote the original software. If you use this software
in a product, an acknowledgement in the product documentation would be
appreciated but is not required.

2. Altered source versions must be plainly marked as such, and must
not be misrepresented as being the original software.

3. This notice may not be removed or altered from any source
distribution.
*/

/*
   Modified by Martin Kraner <martinkraner@outlook.com>, added priority culture support.
   If priority culture translation exists it uses it othrewise falls back to the previous system)
*/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace SecondLanguage {

    /// <summary>
    /// Called by <see cref="Translator"/> to format translated strings.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <param name="args">Arguments to replace the format string's placeholders with.</param>
    /// <returns>The formatted translated string.</returns>
    public delegate string TranslatorFormatCallback(string format, params object[] args);

    /// <summary>
    /// Translates strings.
    /// This is the primary class to interact with in this library, unless you are making an editor.
    /// Create yourself a global instance to refer to from all parts of your code.
    /// </summary>
    public class Translator {
        private TranslatorFormatCallback _formatCallback;
        private Translator _parent;
        private static readonly Translator Singleton = new Translator();
        private bool _throwOnFormatExceptions;
        private List<Translation> _translations;
        private static Translation _selectedTranslation;

        static Translator() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translator"/> class.
        /// No translations are added automatically.
        /// </summary>
        private Translator() {
            FormatCallback = string.Format;
            ClearTranslationList();
        }

        /// <summary>
        /// Clears the list of translation files.
        /// Without any translations, <see cref="Translate"/> will return the strings it is passed.
        /// This is a good place to start if you are using your native language's strings as
        /// translation keys.
        /// </summary>
        public void ClearTranslationList() {
            AboutToChange();
            _translations = new List<Translation>(); // Thread-safe.
        }

        /// <summary>
        /// Registers a translation file.
        /// Because translations are evaluated in the order they are added,
        /// be sure to add the main language first and fallbacks later.
        /// </summary>
        /// <param name="file">The file to register.</param>
        public void RegisterTranslation(Translation file) {
            AboutToChange();
            Throw.If.Null(file, "file");

            _translations = new List<Translation>(_translations.Concat(new[] { file })); // Thread-safe.
        }

        /// <summary>
        /// Registers a translation file, loaded from disk.
        /// Because translations are evaluated in the order they are added,
        /// be sure to add the main language first and fallbacks later.
        /// </summary>
        /// <param name="filename">The name of the file to load and register.</param>
        /// <param name="rootDirectory">
        ///     The root directory. If you specify a full path in <paramref name="filename"/>,
        ///     this has no effect.
        ///     
        ///     If this is <c>null</c>, the assembly resolver base directory (which is
        ///     normally your program directory) is used.
        /// </param>
        /// <param name="culture"></param>
        /// <returns>The newly-registered translation.</returns>
        public Translation RegisterTranslation(string filename, string rootDirectory = null, CultureInfo culture = null) {
            Throw.If.Null(filename, "filename");

            if (rootDirectory == null) {
                rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            Translation translation;
            if (filename.EndsWith(".MO", true, CultureInfo.InvariantCulture)) {
                translation = new GettextMOTranslation(culture);
            }
            else {
                translation = new GettextPOTranslation(culture);
            }

            translation.Load(Path.Combine(rootDirectory, filename));
            RegisterTranslation(translation);
            return translation;
        }

        /// <summary>
        /// Tries to load and register a translation.
        /// Because translations are evaluated in the order they are added,
        /// be sure to add the main language first and fallbacks later.
        /// Use this variant if you don't want or need the new <see cref="Translation"/> object.
        /// </summary>
        /// <param name="filename">The name of the file to load and register.</param>
        /// <param name="rootDirectory">
        ///     The root directory. If you specify a full path in <paramref name="filename"/>,
        ///     this has no effect.
        ///     
        ///     If this is <c>null</c>, the assembly resolver base directory (which is
        ///     normally your program directory) is used.
        /// </param>
        /// <returns><c>true</c> if the file loaded successfully.</returns>
        public bool TryRegisterTranslation(string filename,
                                           string rootDirectory = null) {
            Translation translation;
            return TryRegisterTranslation(filename, out translation, rootDirectory);
        }

        /// <summary>
        /// Tries to load and register a translation.
        /// Because translations are evaluated in the order they are added,
        /// be sure to add the main language first and fallbacks later.
        /// Use this variant if you want to get back the new <see cref="Translation"/> object.
        /// </summary>
        /// <param name="filename">The name of the file to load and register.</param>
        /// <param name="translation">The newly-registered translation.</param>
        /// <param name="rootDirectory">
        ///     The root directory. If you specify a full path in <paramref name="filename"/>,
        ///     this has no effect.
        ///     
        ///     If this is <c>null</c>, the assembly resolver base directory (which is
        ///     normally your program directory) is used.
        /// </param>
        /// <param name="culture"></param>
        /// <returns><c>true</c> if the file loaded successfully.</returns>
        public bool TryRegisterTranslation(string filename, out Translation translation, string rootDirectory = null, CultureInfo culture = null) {
            try {
                translation = RegisterTranslation(filename, rootDirectory, culture);
                return true;
            }
            catch {
                translation = null;
                return false;
            }
        }

        private List<string> CultureNamesFromCultureInfo(CultureInfo culture) {
            var cultureNames = new List<string>();

            do {
                if (!cultureNames.Contains(culture.Name)) {
                    cultureNames.Add(culture.Name);
                }
                culture = culture.Parent;
            }
            while (culture != CultureInfo.InvariantCulture); // Check at the end so that, if someone *wants to*, they can add the InvariantCulture.

            return cultureNames;
        }

        /// <summary>
        /// Adds translations found using a search pattern.
        /// </summary>
        /// <param name="searchPattern">
        ///     The search pattern.
        ///     
        ///     For example, language\*.po will look for all .po files in the language subdirectory
        ///     of the root directory.
        /// </param>
        /// <param name="rootDirectory">
        ///     The root directory for the search.
        ///     
        ///     If this is <c>null</c>, the search will begin from the assembly resolver base directory,
        ///     which normally is your program directory. This is typically what you want.
        /// </param>
        /// <param name="culture"></param>
        /// <returns>Newly-registered translations.</returns>
        public Translation[] RegisterTranslationsBySearchPattern(string searchPattern, string rootDirectory = null, CultureInfo culture = null) {
            Throw.If.Null(searchPattern, "searchPattern");

            if (rootDirectory == null) {
                rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }

            string[] files;
            try {
                files = Directory.GetFiles(rootDirectory, searchPattern);
            }
            catch (IOException) {
                return new Translation[0];
            }

            var translations = new List<Translation>();
            foreach (var filename in files) {
                Translation translation;
                if (TryRegisterTranslation(filename, out translation, null, culture)) {
                    translations.Add(translation);
                }
            }
            return translations.ToArray();
        }

        /// <summary>
        /// Adds translations found using a culture-specific search pattern.
        /// </summary>
        /// <param name="searchPattern">
        ///     The search pattern.
        ///     
        ///     <see cref="string.Format(string, object)"/> is called on this pattern with a culture name.
        ///     For example, for the en-US culture, a pattern of locale\{0}\LC_MESSAGES\*.mo will search for all .mo files
        ///     in locale\en-US\LC_MESSAGES as well as locale\en\LC_MESSAGES.
        /// </param>
        /// <param name="culture">
        ///     The culture to find translations for.
        ///     If this is <c>null</c>, the user's current UI culture will be used.
        /// </param>
        /// <param name="rootDirectory">
        ///     The root directory for the search.
        ///     
        ///     If this is <c>null</c>, the search will begin from the assembly resolver base directory,
        ///     which normally is your program directory. This is typically what you want.
        /// </param>
        /// <returns>Newly-registered translations.</returns>
        public Translation[] RegisterTranslationsByCulture(string searchPattern,
                                                           CultureInfo culture = null,
                                                           string rootDirectory = null) {
            Throw.If.Null(searchPattern, "searchPattern");
            if (culture == null) {
                culture = CultureInfo.CurrentUICulture;
            }

            var translations = new List<Translation>();
            foreach (var cultureName in CultureNamesFromCultureInfo(culture)) {
                var formattedSearchPattern = string.Format(searchPattern, cultureName);
                translations.AddRange(RegisterTranslationsBySearchPattern(formattedSearchPattern, rootDirectory, culture));
            }
            return translations.ToArray();
        }

        /// <summary>
        /// Adds translations found using culture-specific search patterns.
        /// </summary>
        /// <param name="searchPatterns">
        ///     Search patterns.
        ///     
        ///     <see cref="string.Format(string, object)"/> is called on each pattern with a culture name.
        ///     For example, for the en-US culture, a pattern of locale/{0}/LC_MESSAGES/*.mo will search for all .mo files
        ///     in locale/en-US/LC_MESSAGES as well as locale/en/LC_MESSAGES.
        /// </param>
        /// <param name="cultures">
        ///     The cultures to find translations for.
        ///     If this is <c>null</c>, the user's current UI culture will be used.
        /// </param>
        /// <param name="rootDirectory">
        ///     The root directory for the search.
        ///     
        ///     If this is <c>null</c>, the search will begin from the assembly resolver base directory,
        ///     which normally is your program directory. This is typically what you want.
        /// </param>
        /// <returns>Newly-registered translations.</returns>
        public Translation[] RegisterTranslationsByCulture(string[] searchPatterns,
                                                           CultureInfo[] cultures = null,
                                                           string rootDirectory = null) {
            Throw.If.NullElements(searchPatterns, "searchPatterns");
            if (cultures == null) {
                cultures = new[] { CultureInfo.CurrentUICulture };
            }
            Throw.If.NullElements(cultures, "cultures");

            var translations = new List<Translation>();
            foreach (var cultureName in cultures.SelectMany(CultureNamesFromCultureInfo).Distinct()) {
                foreach (var searchPattern in searchPatterns) {
                    var formattedSearchPattern = string.Format(searchPattern, cultureName);
                    translations.AddRange(RegisterTranslationsBySearchPattern(formattedSearchPattern, rootDirectory));
                }
            }
            return translations.ToArray();
        }

        private bool TryFormat(string format, object[] args, out string result) {
            try {
                result = FormatCallback(format, args);
                return true;
            }
            catch (FormatException) {
                if (ThrowOnFormatExceptions) {
                    throw;
                }
                result = null;
                return false;
            }
        }

        /// <summary>
        /// Translates a string.
        /// </summary>
        /// <param name="id">The translation key. For Gettext projects, this is typically an untranslated string.</param>
        /// <param name="args">Arguments to replace the format string's placeholders with.</param>
        /// <returns>The translated string, or the formatted translation key if none is set.</returns>
        public string Translate(string id, params object[] args) {
            return TranslateContextual(null, id, args);
        }

        /// <summary>
        /// Translates a string in a given context.
        /// </summary>
        /// <param name="context">The context, if any, or <c>null</c>.</param>
        /// <param name="id">The translation key. For Gettext projects, this is typically an untranslated string.</param>
        /// <param name="args">Arguments to replace the format string's placeholders with.</param>
        /// <returns>The translated string, or the formatted translation key if none is set.</returns>
        public string TranslateContextual(string context, string id, params object[] args) {
            Throw.If.Null(id, "id").Null(args, "args");

            string result;
            if (_selectedTranslation != null) {
                if (FormatTranslation(context, id, args, _selectedTranslation, out result)) {
                    return result;
                }
            }

            foreach (Translation translation in TranslationList) {
                if (FormatTranslation(context, id, args, translation, out result)) {
                    return result;
                }
            }

            var parent = Parent; // Thread-safe.
            if (parent != null) {
                return parent.TranslateContextual(context, id, args);
            }

            return TryFormat(id, args, out result) ? result : id;
        }

        private bool FormatTranslation(string context, string id, object[] args, Translation uiCultureTranslation, out string result) {
            var format = uiCultureTranslation.GetString(id, context);
            if (format != null) {
                if (TryFormat(format, args, out result)) {
                        return true;
                }
            }

            result = null;
            return false;
        }

        private bool FormatPluralTranslation(string context, string id, string idPlural, long value, object[] args, Translation uiCultureTranslation, out string result) {
            var format = uiCultureTranslation.GetPluralString(id, idPlural, value, context);
            if (format != null) {
                if (TryFormat(format, args, out result)) {
                        return true;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        /// Translates a string with distinct singular and plural forms.
        /// </summary>
        /// <param name="id">The singular translation key. For Gettext projects, this is typically a singular untranslated string.</param>
        /// <param name="idPlural">The plural translation key. For Gettext projects, this is typically a plural untranslated string.</param>
        /// <param name="value">The value to look up the plural string for.</param>
        /// <param name="args">Arguments to replace the format string's placeholders with.</param>
        /// <returns>The translated string, or the formatted translation key if none is set.</returns>
        public string TranslatePlural(string id, string idPlural, long value, params object[] args) {
            return TranslateContextualPlural(null, id, idPlural, value, args);
        }

        /// <summary>
        /// Translates a string with distinct singular and plural forms, in a given context.
        /// </summary>
        /// <param name="context">The context, if any, or <c>null</c>.</param>
        /// <param name="id">The singular translation key. For Gettext projects, this is typically a singular untranslated string.</param>
        /// <param name="idPlural">The plural translation key. For Gettext projects, this is typically a plural untranslated string.</param>
        /// <param name="value">The value to look up the plural string for.</param>
        /// <param name="args">Arguments to replace the format string's placeholders with.</param>
        /// <returns>The translated string, or the formatted translation key if none is set.</returns>
        public string TranslateContextualPlural(string context, string id, string idPlural, long value, params object[] args) {
            Throw.If.Null(id, "id").Null(idPlural, "idPlural").Null(args, "args");

            string result;

            if (_selectedTranslation != null) {
                if (FormatPluralTranslation(context, id, idPlural, value, args, _selectedTranslation, out result)) {
                    return result;
                }
            }

            foreach (var translation in TranslationList) {
                var format = translation.GetPluralString(id, idPlural, value, context);
                if (format != null) {
                    if (TryFormat(format, args, out result)) {
                        return result;
                    }
                }
            }

            var parent = Parent; // Thread-safe.
            if (parent != null) {
                return parent.TranslateContextualPlural(context, id, idPlural, value, args);
            }

            var key = value == -1 || value == 1 ? id : idPlural;
            return TryFormat(key, args, out result) ? result : id;
        }

        /// <summary>
        /// Makes the <see cref="Translator"/> read-only.
        /// </summary>
        public void MakeReadOnly() {
            IsReadOnly = true;
        }

        protected void AboutToChange() {
            if (IsReadOnly) {
                throw new InvalidOperationException("Translator is read-only.");
            }
        }

        /// <summary>
        /// The callback used for formatting strings.
        /// You can use this to replace the default <see cref="string.Format(string, object)"/>,
        /// if for instance your translation keys are C-style.
        /// </summary>
        public TranslatorFormatCallback FormatCallback {
            get { return _formatCallback; }
            set {
                AboutToChange();
                Throw.If.Null(value, "value");
                _formatCallback = value;
            }
        }

        /// <summary>
        /// <c>true</c> if the file cannot be modified.
        /// The translations may still be modifiable. Call <see cref="Translation.MakeReadOnly"/>
        /// on the translations if you do not want them to be editable either.
        /// </summary>
        public bool IsReadOnly { get; private set; }

        /// <summary>
        /// The parent translator.
        /// If a string is not set in the current translation list, the parent's is checked.
        /// </summary>
        public Translator Parent {
            get { return _parent; }
            set {
                AboutToChange();
                _parent = value;
            }
        }

        /// <summary>
        /// Normally, if a <see cref="FormatException"/> is thrown for a translated string,
        /// it is ignored and the fallback is used. If all fallbacks fail, the translation key
        /// is used. If this fails to format as well, the unformatted translation key is returned.
        /// This is so translation calls can never crash your program, even if the translation
        /// is formatted incorrectly in a particular language.
        /// 
        /// If you dislike this behavior, you can re-enable the exception.
        /// </summary>
        public bool ThrowOnFormatExceptions {
            get { return _throwOnFormatExceptions; }
            set {
                AboutToChange();
                _throwOnFormatExceptions = value;
            }
        }

        /// <summary>
        /// The list of translation files currently in use.
        /// </summary>
        public IEnumerable<Translation> TranslationList {
            get {
                foreach (var translation in _translations) {
                    yield return translation;
                }
            }
        }

        /// <summary>
        /// Translates a string.
        /// </summary>
        /// <param name="id">The translation key. For Gettext projects, this is typically an untranslated string.</param>
        /// <param name="args">Arguments to replace the format string's placeholders with.</param>
        /// <returns>The translated string, or the formatted translation key if none is set.</returns>
        public string this[string id, params object[] args] {
            get { return Translate(id, args); }
        }

        /// <summary>A default <see cref="Translator"/>.</summary>
        public static Translator Default { get { return Singleton; } }

        public static CultureInfo SelectedCulture {
            get { return _selectedTranslation.TranslationCulture; }
            set {
                if (value == null) {
                    _selectedTranslation = null;
                }

                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                Translation specificUiTranslation = Default.TranslationList.FirstOrDefault(t => t.TranslationCulture.Equals(currentUiCulture));
                if (specificUiTranslation != null) {
                    _selectedTranslation = specificUiTranslation;
                }
                else if (!currentUiCulture.IsNeutralCulture) {
                    Translation neutralUiTranslation = Default.TranslationList.FirstOrDefault(t => t.TranslationCulture.Equals(currentUiCulture.Parent));
                    if (neutralUiTranslation != null) {
                        _selectedTranslation = neutralUiTranslation;
                    }
                }
            }
        }
    }

}