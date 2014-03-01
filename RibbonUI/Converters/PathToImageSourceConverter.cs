using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Frost.Models.Frost;
using Frost.Models.Frost.DB.Files;
using File = System.IO.File;

namespace RibbonUI.Converters {

    public enum PathType {
        Studio,
        AudioChannels,
        VideoResolution,
        VideoResolutionV,
        VideoCodec,
        AudioCodec,
        Box,
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
            if (!Enum.TryParse(parameter as string, out type)) {
                type = PathType.Studio;
            }

            if (!string.IsNullOrEmpty(path)) {
                string filePath;
                switch (type) {
                    case PathType.Studio:
                        filePath = "Images/StudiosE/" + value + ".png";
                        break;
                    case PathType.AudioChannels:
                        filePath = "Images/FlagsE/achan_" + value + ".png";
                        break;
                    case PathType.VideoResolution:
                        filePath = "Images/FlagsE/vres_" + value + ".png";
                        break;
                    case PathType.VideoResolutionV:
                        Video v = (Video) value;
                        if (!v.Resolution.HasValue) {
                            return null;
                        }
                        int res = v.Resolution.Value;
                        switch (v.ScanType) {
                            case ScanType.Interlaced:
                                filePath = "Images/FlagsE/vres_" + res + "i.png";
                                break;
                            case ScanType.Progressive:
                                filePath = "Images/FlagsE/vres_" + res + "p.png";
                                break;
                            default:
                                filePath = "Images/FlagsE/vres_" + res + ".png";
                                break;
                        }
                        break;
                    case PathType.VideoCodec:
                        filePath = "Images/FlagsE/vcodec_" + value + ".png";
                        break;
                    case PathType.AudioCodec:
                        filePath = "Images/FlagsE/acodec_" + value + ".png";
                        break;
                    case PathType.Box:
                        filePath = "Images/Boxes/" + value + ".png";
                        break;
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
