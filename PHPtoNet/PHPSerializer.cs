using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Frost.PHPtoNET {

    /// <summary></summary>
    /// <typeparam name="TObj">The type of the object to serialize.</typeparam>
    public class PHPSerializer<TObj> {
        private readonly TObj _object;
        private static readonly Type _stringType = typeof(string);
        private const BindingFlags PUBLIC_FLAGS = BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private readonly BindingFlags _usedFlags;

        public PHPSerializer(TObj obj, bool serializePrivate = false, bool serializeStatic = false) {
            _object = obj;

            _usedFlags = PUBLIC_FLAGS;
            if (serializePrivate) {
                _usedFlags |= BindingFlags.NonPublic;
            }

            if (serializeStatic) {
                _usedFlags |= BindingFlags.Static;
            }
        }

        public string Serialize() {
            return SerializeClass(_object);
        }

        private string SerializeClass<T>(T obj) {
            Type type = obj.GetType();

            MemberInfo[] memberInfos = type.GetMembers(_usedFlags)
                                           .Where(mi => mi.MemberType != MemberTypes.Method &&
                                                        mi.MemberType != MemberTypes.Constructor &&
                                                        mi.MemberType != MemberTypes.NestedType)
                                           .ToArray();

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("O:{0}:\"{1}\":{2}:{{", Encoding.UTF8.GetByteCount(type.Name), type.Name, memberInfos.Length));

            foreach (MemberInfo member in memberInfos) {
                if (member.IsDefined(typeof(NonSerializedAttribute), false) || member.Name.EndsWith("__BackingField")) {
                    continue;
                }
                sb.Append(SerializeMember(obj, member));
            }
            sb.Append("}");
            return sb.ToString();
        }

        private string SerializeMember<T>(T obj, MemberInfo memberInfo) {
            Type memberType = memberInfo.MemberType == MemberTypes.Property ? ((PropertyInfo) memberInfo).PropertyType : ((FieldInfo) memberInfo).FieldType;
            object value = memberInfo.MemberType == MemberTypes.Property ? ((PropertyInfo) memberInfo).GetValue(obj, new object[] { }) : ((FieldInfo) memberInfo).GetValue(obj);

            string prefix = string.Format("s:{0}:\"{1}\";", Encoding.UTF8.GetByteCount(memberInfo.Name), memberInfo.Name);

            string memberInfoSer = SerailizeMemberInfo(memberType, value);
            if (!string.IsNullOrEmpty(memberInfoSer)) {
                return prefix + memberInfoSer;
            }
            return "";
        }

        private string SerailizeMemberInfo(Type memberType, object value) {
            if (memberType.GetInterface("ICollection") != null) {
                return value != null
                    ? SerializeIEnumerable((IEnumerable) value, memberType.GetElementType())
                    : "N;";
            }

            if (memberType.GetInterface("IDictionary") != null) {
                return value != null
                    ? SerializeIDictionary((IDictionary) value, memberType.GetElementType())
                    : "N;";
            }

            if (memberType == _stringType) {
                return SerializeString(value as string);
            }

            if (memberType.IsEnum) {
                return SerializeString(value.ToString());
            }

            if (memberType.IsClass) {
                if (memberType.Module.ScopeName == "CommonLanguageRuntimeLibrary") {
                    return null;
                }

                return (value == null ? "N;" : SerializeClass(value));
            }

            if (memberType.IsPrimitive) {
                return SerializePrimitive(memberType, value);
            }

            //Structs
            if (memberType.IsValueType) {
                if (memberType.Module.ScopeName == "CommonLanguageRuntimeLibrary") {
                    return null;
                }

                return SerializeClass(memberType);
            }

            return SerializeString(value.ToString());
        }

        private string SerializePrimitive(Type memberType, object value) {
            string postfix = null;
            switch (memberType.Name) {
                case "Boolean":
                    postfix = SerializeBoolean((bool) value);
                    break;
                case "Byte":
                case "SByte":
                case "Int16":
                case "UInt16":
                case "Int32":
                    postfix = SerializeInteger(Convert.ToInt32(value));
                    break;
                case "UInt32":
                    postfix = SerializeUInt((uint) value);
                    break;
                case "Char":
                    postfix = SerializeString(new string(new[] { (char) value }));
                    break;
                case "Int64":
                case "UInt64":
                case "Single":
                case "Double":
                    postfix = SerializeDouble(Convert.ToDouble(value));
                    break;
            }
            return postfix;
        }

        #region Serialize Primitives

        private string SerializeUInt(uint value) {
            return string.Format((value > int.MaxValue) ? "d:{0};" : "i:{0};", value.ToString(CultureInfo.InvariantCulture));
        }

        private string SerializeDouble(double value) {
            return string.Format("d:{0};", value.ToString(CultureInfo.InvariantCulture));
        }

        private string SerializeInteger(int value) {
            return string.Format("i:{0};", value.ToString(CultureInfo.InvariantCulture));
        }

        private string SerializeBoolean(bool value) {
            return value ? "b:1;" : "b:0;";
        }

        #endregion

        private string SerializeString(string value) {
            return string.IsNullOrEmpty(value)
                       ? "N;"
                       : string.Format("s:{0}:\"{1}\";", Encoding.UTF8.GetByteCount(value), value);
        }

        private string SerializeIEnumerable(IEnumerable enumerable, Type elementType) {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (object val in enumerable) {
                sb.AppendFormat("i:{0};", i++);
                sb.Append(SerailizeMemberInfo(elementType, val) ?? "N;");
            }

            sb.Append("}");
            return string.Format("a:{0}:{{", i) + sb;
        }


        private string SerializeIDictionary(IDictionary value, Type elementType) {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            foreach (DictionaryEntry val in value) {
                object key = val.Key;
                if (key is int) {
                    sb.AppendFormat("i:{0};", (int) key);
                }
                else {
                    sb.AppendFormat("s:{0};", key);
                }

                sb.Append(SerailizeMemberInfo(elementType, val) ?? "N;");
                i++;
            }

            sb.Append("}");
            return string.Format("a:{0}:{{", i) + sb;
        }
    }

}