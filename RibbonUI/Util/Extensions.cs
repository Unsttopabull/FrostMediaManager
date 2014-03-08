using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace RibbonUI.Util {

    public static class Extensions {
        public static string GetDescription(this Enum enumObj) {
            DescriptionAttribute description = enumObj.GetType()
                                                      .GetField(enumObj.ToString())
                                                      .GetCustomAttribute<DescriptionAttribute>(false);
            return description != null
                       ? description.Description
                       : enumObj.ToString();
        }

        public static T GetEnumValueFromDescription<T>(this string description) where T : struct {
            Type t = typeof(T);
            object enumValueFromDescription = GetEnumValueFromDescription(description, t);
            if (enumValueFromDescription != null) {
                return (T) enumValueFromDescription;
            }
            return default(T);
        }

        public static object GetEnumValueFromDescription(this string description, Type enumType) {
            if (!enumType.IsEnum) {
                return null;
            }

            string enumName = enumType.GetFields().Where(fi => {
                DescriptionAttribute descriptionAttribute = fi.GetCustomAttribute<DescriptionAttribute>(false);
                return descriptionAttribute != null && descriptionAttribute.Description == description;
            })
            .Select(fi => fi.Name)
            .FirstOrDefault();

            return enumName == null 
                ? null
                : Enum.Parse(enumType, enumName);
        }
    }

}