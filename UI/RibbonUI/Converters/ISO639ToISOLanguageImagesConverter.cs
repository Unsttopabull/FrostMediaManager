﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using Frost.Common.Util.ISO;

namespace Frost.RibbonUI.Converters {
    public class ISO639ToISOLanguageImagesConverter : IValueConverter {

        /// <summary>Converts a value. </summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var isoLanguageCode = ISOLanguageCodes.Instance.GetByISOCode(value as string);
            if (isoLanguageCode != null) {
                return string.Format("file://{0}/{1}/{2}.png", Directory.GetCurrentDirectory(), "Images/Languages", isoLanguageCode.Alpha3);
            }
            return null;
        }

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}
