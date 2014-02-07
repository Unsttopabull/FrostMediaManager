using System;
using System.Globalization;
using System.Windows.Data;

namespace RibbonUI.Converters {
    public class SubtractConverter : IValueConverter {

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double val = (double) value;
            double param = System.Convert.ToDouble(parameter);

            return val - param;
        }

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            double val = (double) value;
            double param = System.Convert.ToDouble(parameter);

            return val + param;
        }
    }
}
