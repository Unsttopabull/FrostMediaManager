using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.DetectFeatures.Util;

namespace RibbonUI.Validations {

    public class EmptyOrNullValidationRule : ValidationRule {

        /// <summary>When overridden in a derived class, performs validation checks on a value.</summary>
        /// <returns>A <see cref="T:System.Windows.Controls.ValidationResult"/> object.</returns>
        /// <param name="value">The value from the binding target to check.</param><param name="cultureInfo">The culture to use in this rule.</param>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
            BindingGroup bindingGroup = (BindingGroup)value;
            if (bindingGroup != null) {
                CodecIdBinding binding = bindingGroup.Items[0] as CodecIdBinding;

                if (binding == null) {
                    return new ValidationResult(false, "Binding is null. Entry will not be saved");
                }

                if (string.IsNullOrEmpty(binding.CodecId)) {
                    return new ValidationResult(false, "Codec ID is null or empty. Entry will not be saved");
                }

                if (string.IsNullOrEmpty(binding.Mapping)) {
                    return new ValidationResult(false, "Mapping is null or empty. Entry will not be saved");
                }
            }

            return ValidationResult.ValidResult;
        }
    }

}