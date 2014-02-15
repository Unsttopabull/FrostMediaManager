using System;

namespace GettextTranslationExtension {
    public class TExtension : MarkupExtension {

        /// <summary>Initializes a new instance of a class derived from <see cref="T:System.Windows.Markup.MarkupExtension"/>. </summary>
        public TExtension(string key) {
            Key = key;
        }

        [ConstructorArgument("key")]
        public string Key { get; set; }

        /// <summary>When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.</summary>
        /// <returns>The object value to set on the property where the extension is applied.</returns>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider) {
            Binding binding = new Binding("Value") {
                Source = new TranslationData(Key)
            };

            return binding.ProvideValue(serviceProvider);
        }
    }

}
