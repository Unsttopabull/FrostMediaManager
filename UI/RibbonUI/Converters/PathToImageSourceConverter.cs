﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Frost.RibbonUI.Converters {

    public enum PathType {
        Unknown,
        Language,
        Country
    }

    public class PathToImageSourceConverter : IValueConverter {

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) {
                return null;
            }

            string path = value.ToString();

            PathType type;
            Enum.TryParse(parameter as string, out type);

            if (!string.IsNullOrEmpty(path)) {
                string filePath;
                switch (type) {
                    case PathType.Language:
                        filePath = "Images/Languages/" + value + ".png";
                        break;
                    case PathType.Country:
                        filePath = "Images/Countries/" + value + ".png";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (File.Exists(filePath)) {
                    path = string.Format("file://{0}/{1}", Directory.GetCurrentDirectory(), filePath);
                    BitmapImage bitmapImage = new BitmapImage(new Uri(path, UriKind.Absolute));
                    return bitmapImage;
                }
            }
            return null;
        }

        /// <summary>Converts a value.</summary>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
