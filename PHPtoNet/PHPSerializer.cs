using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Frost.PHPtoNET {

    /// <summary></summary>
    public class PHPSerializer {
        private bool _private;
        private bool _static;
        private static readonly Type _stringType = typeof(string);
        private const BindingFlags PUBLIC_FLAGS = BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        private BindingFlags _usedFlags;

        /// <summary>Initializes a new instance of the <see cref="PHPSerializer"/> class.</summary>
        public PHPSerializer() {
            _usedFlags = PUBLIC_FLAGS;
        }

        /// <summary>Gets or sets a value indicating whether private (private/internal/protected) members are to be serialized aswell.</summary>
        /// <value>Is <c>true</c> if private (private/internal/protected) members are to be serialized; otherwise, <c>false</c></value>
        public bool SerializePrivateMembers {
            get { return _private; }
            set {
                if (value) {
                    _private = true;
                    _usedFlags |= BindingFlags.NonPublic;
                }
                else {
                    _private = false;
                    _usedFlags &= ~BindingFlags.NonPublic;
                }
            }
        }

        /// <summary>Gets or sets a value indicating whether static members are to be serialized aswell.</summary>
        /// <value>Is <c>true</c> if static members are to be serialized; otherwise, <c>false</c></value>
        public bool SerializeStaticMembers {
            get { return _static; }
            set {
                if (value) {
                    _static = true;
                    _usedFlags |= BindingFlags.Static;
                }
                else {
                    _static = false;
                    _usedFlags &= ~BindingFlags.Static;
                }
            }
        }

        /// <summary>Serializes the specified object, struct or primitive.</summary>
        /// <param name="obj">The object, struct or primitive to serialize.</param>
        /// <returns></returns>
        public string Serialize(object obj) {
            if (obj == null) {
                return "N;";
            }

            return SerailizeMemberInfo(obj.GetType(), obj);
        }

        private string SerializeClass<T>(T obj, Type type) {
            MemberInfo[] memberInfos = type.GetMembers(_usedFlags)
                                           .Where(mi => mi.MemberType != MemberTypes.Method &&
                                                        mi.MemberType != MemberTypes.Constructor &&
                                                        mi.MemberType != MemberTypes.NestedType &&
                                                        mi.MemberType != MemberTypes.TypeInfo)
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

            //bool debug = memberType.Name != "String" && !memberType.IsPrimitive && !memberType.IsValueType && !memberType.IsArray;

            string prefix = string.Format("s:{0}:\"{1}\";", Encoding.UTF8.GetByteCount(memberInfo.Name), memberInfo.Name);

            string memberInfoSer = SerailizeMemberInfo(memberType, value);
            if (!string.IsNullOrEmpty(memberInfoSer)) {
                string serializedMember = prefix + memberInfoSer;
                return serializedMember;
            }
            return "";
        }

        private string SerailizeMemberInfo(Type memberType, object value) {
            if (memberType == _stringType) {
                return SerializeString(value as string);
            }

            if (memberType.IsEnum) {
                return SerializeString(value.ToString());
            }

            if (memberType.IsPrimitive) {
                return SerializePrimitive(memberType, value);
            }

            if (memberType.GetInterface("IDictionary") != null) {
                if (memberType.IsGenericType) {
                    Type[] arguments = memberType.GetGenericArguments();

                    return value != null && arguments.Length == 2
                               ? SerializeIDictionary((IDictionary) value, arguments[1])
                               : "N;";                    
                }

                return value != null
                    ? SerializeIDictionary((IDictionary) value)
                    : "N;";
            }

            if (memberType.GetInterface("ICollection") != null) {
                if (memberType.IsGenericType) {
                    Type[] arguments = memberType.GetGenericArguments();

                    return value != null
                               ? SerializeIEnumerable((IEnumerable) value, arguments[0])
                               : "N;";
                }

                return value != null
                           ? SerializeIEnumerable((IEnumerable) value, memberType.GetElementType())
                           : "N;";
            }

            if (memberType.IsClass) {
                if (memberType.Module.ScopeName == "CommonLanguageRuntimeLibrary") {
                    return SerializeString(value.ToString());
                }

                return (value == null ? "N;" : SerializeClass(value, memberType));
            }

            //Structs
            if (memberType.IsValueType) {
                if (memberType.Module.ScopeName == "CommonLanguageRuntimeLibrary") {
                    if (memberType.Name == "Nullable`1") {
                        return value != null
                            ? SerailizeMemberInfo(value.GetType(), value)
                            : "N;";
                    }

                    return SerializeString(value.ToString());
                }

                return SerializeClass(value, memberType);
            }

            throw new ParsingException(string.Format("Could not serialize member of type \"{0}\"", memberType.FullName));
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


        private string SerializeIDictionary(IDictionary value, Type valType = null) {
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

                if (valType == null) {
                    sb.Append(SerailizeMemberInfo(val.Value.GetType(), val.Value) ?? "N;");
                }
                else {
                    sb.Append(SerailizeMemberInfo(valType, val.Value) ?? "N;");
                }
                i++;
            }

            sb.Append("}");
            return string.Format("a:{0}:{{", i) + sb;
        }
    }

}