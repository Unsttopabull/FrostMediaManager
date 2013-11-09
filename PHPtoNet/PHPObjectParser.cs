#region

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

namespace Frost.PHPtoNET {
    internal enum PHPType {
        Unknown,
        Integer,
        Double,
        Boolean,
        Null,
        Array,
        Object,
        String
    }

    public class PHPObjectParser {
        private readonly IScanner _scanner;

        public PHPObjectParser(IScanner scanner) {
            _scanner = scanner;
        }

        /// <param name="serializedObj">String to be parsed</param>
        /// <param name="encoding">Encoding in which the source string is saved, if <b>null</b> or omitted defaults to <see cref="Encoding.UTF8"/></param>
        public PHPObjectParser(string serializedObj, Encoding encoding = null) {
            _scanner = (encoding == null)
                ? new Scanner(serializedObj)
                : new Scanner(serializedObj, encoding);
        }

        #region Object

        public bool Obj<T>(ref T obj) {
            Token t = _scanner.NextToken();

            if (t.Lexem == "O") {
                t = _scanner.NextToken(); //:

                if (t.TokenType == Tokens.Colon) {
                    _scanner.NextToken(); //name len
                    t = _scanner.NextToken(); //:

                    if (t.TokenType == Tokens.Colon) {
                        _scanner.NextToken(); //ClassName
                        t = _scanner.NextToken(); //:

                        if (t.TokenType == Tokens.Colon) {
                            t = _scanner.NextToken(); //num prop
                            if (t.TokenType == Tokens.Integer) {
                                int numProp = int.Parse(t.Lexem);
                                t = _scanner.NextToken(); //:

                                if (t.TokenType == Tokens.Colon) {
                                    t = _scanner.NextToken(); //{

                                    if (t.Lexem == "{") {
                                        //obj members
                                        Fields(numProp, ref obj);

                                        //t = _scanner.CurrToken(); //}
                                        return _scanner.NextToken().Lexem == "}"; //}
                                    }
                                }
                            }
                        }
                    }
                }
            }
            obj = default(T);
            return false;
        }

        private void Fields<T>(int numProp, ref T objToModify) {
            for (int i = 0; i < numProp; i++) {
                string memberName = PHPDeserializer.ParseString(_scanner);

                Type memberType;
                var member = GetMember(memberName, typeof(T), out memberType);

                dynamic val = PHPDeserializer.GetNextType(_scanner, memberType);
                if (member != null) {
                    SetMemberValue(val, objToModify, member);
                }
            }
        }

        private static dynamic GetMember(string memberName, Type t, out Type memberType) {
            FieldInfo field = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic |
                                          BindingFlags.FlattenHierarchy | BindingFlags.Instance |
                                          BindingFlags.Static)
                               .FirstOrDefault(fi => fi.Name == memberName);

            //if found set value and return
            if (field != null) {
                memberType = field.FieldType;
                return field;
            }

            PropertyInfo property = t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                    BindingFlags.FlattenHierarchy | BindingFlags.Instance |
                                                    BindingFlags.Static)
                                     .FirstOrDefault(fi => fi.Name == memberName);

            //if found set value, else do nothing
            memberType = (property != null)
                                 ? property.PropertyType
                                 : null;

            return property;
        }

        private void SetMemberValue<T, T2>(dynamic value, T objToModify, T2 member) {
            if (value != null) {
                Type valueType = value.GetType();

                Type memberType = typeof(T2) == typeof(PropertyInfo)
                                     ? member.CastAs<PropertyInfo>().PropertyType
                                     : member.CastAs<FieldInfo>().FieldType;

                if (valueType != memberType) {
                    if (valueType == typeof(Hashtable)) {
                        Array arr = ConvertHashtableToArray((Hashtable)value, memberType.ToString());
                        SetValue(objToModify, arr, member);
                    }
                    else if (valueType == typeof(PHPObject)) {
                        dynamic obj = ParseObjByType(memberType);

                        SetValue(objToModify, obj, member);
                    }
                    else {
                        try {
                            SetValue(objToModify, Convert.ChangeType(value, memberType), member);
                        }
                        catch (Exception) {
                            throw new ParsingException(valueType.ToString(), memberType.ToString());
                        }
                    }
                    return;
                }
                SetValue(objToModify, value, member);
            }
            else {
                SetValue(objToModify, value, member);
            }
        }

        private static void SetValue<T>(T objToModify, object value, dynamic fieldOrProperty) {
            if (fieldOrProperty.GetType() == typeof(PropertyInfo)) {
                fieldOrProperty.SetValue(objToModify, value, null);
            }
            else {
                fieldOrProperty.SetValue(objToModify, value);
            }
        }

        internal dynamic ParseObjByType(Type objectType) {
            //needs empty default constructor
            dynamic obj = Activator.CreateInstance(objectType);//Extend.New(objectType);

            MethodInfo methodInfo = typeof(PHPObjectParser).GetMethod("Obj");
            methodInfo = methodInfo.MakeGenericMethod(objectType);
            methodInfo.Invoke(this, new object[] { obj });
            return obj;
        }

        private static Array ConvertHashtableToArray(Hashtable val, string type) {
            switch (type) {
                case "System.Int32[]":
                    return HashtableToArrayPreservingOrder<int>(val);
                case "System.Double[]":
                    return HashtableToArrayPreservingOrder<double>(val);
                case "System.Byte[]":
                    return HashtableToArrayPreservingOrder<byte>(val);
                case "System.Int64[]":
                    return HashtableToArrayPreservingOrder<long>(val);
                case "System.Int16[]":
                    return HashtableToArrayPreservingOrder<short>(val);
                case "System.String[]":
                    return HashtableToArrayPreservingOrder<string>(val);
                default:
                    return default(Array);
            }
        }

        private static T[] HashtableToArrayPreservingOrder<T>(Hashtable val) {
            T[] arr = new T[val.Count];

            IDictionaryEnumerator enumerator = val.GetEnumerator();
            while (enumerator.MoveNext()) {
                arr[(int)enumerator.Key] = (enumerator.Value is T)
                                                    ? (T)enumerator.Value
                                                    : (T)Convert.ChangeType(enumerator.Value, typeof(T));
            }
            return arr;
        }

        #endregion
    }
}
