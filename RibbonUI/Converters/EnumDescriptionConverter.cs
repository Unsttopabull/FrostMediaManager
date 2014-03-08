using System;
using System.Globalization;
using System.Windows.Data;
using Frost.DetectFeatures.FileName;
using RibbonUI.Util;

namespace RibbonUI.Converters {

    public class EnumDescriptionConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Array) {
                SegmentType[] arr = (SegmentType[]) value;

                string[] strArr = new string[arr.Length];
                for (int i = 0; i < arr.Length; i++) {
                    strArr[i] = arr[i].GetDescription();
                }
                return strArr;
            }

            return ((Enum) value).GetDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            string description = value as string;

            return description != null
                ? description.GetEnumValueFromDescription(targetType)
                : null;
        }
    }

}