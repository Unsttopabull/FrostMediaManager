using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace GettextTranslationExtension {
    public class TranslationManager {
        private static readonly TranslationManager Singleton = new TranslationManager();
        public event EventHandler LanguageChanged;

        static TranslationManager() {
            
        }

        public TranslationManager() {
            
        }

        public ITranslationProvider TranslationProvider { get; set; }

        public static TranslationManager Instance {get { return Singleton; }}

        public CultureInfo CurrentLangauge {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set {
                if (value != CurrentLangauge) {
                    Thread.CurrentThread.CurrentUICulture = value;
                    OnLanguageChanged();
                }
            }
        }

        public IEnumerable<CultureInfo> Languages {
            get {
                if (TranslationProvider != null) {
                    return TranslationProvider.Languages;
                }
                return Enumerable.Empty<CultureInfo>();
            }
        }

        public string Translate(string key) {
            if (TranslationProvider == null) {
                return key;
            }

            return TranslationProvider.Translate(key) ?? key;
        }

        protected virtual void OnLanguageChanged() {
            if (LanguageChanged != null) {
                LanguageChanged(this, EventArgs.Empty);
            }
        }
    }
}
