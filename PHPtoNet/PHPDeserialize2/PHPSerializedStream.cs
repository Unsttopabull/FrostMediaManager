using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Frost.PHPtoNET.PHPDeserialize2;

namespace Frost.PHPtoNET {

    public class PHPSerializedStream : IDisposable {
        private readonly Encoding _enc;
        private readonly MemoryStream _ms;
        private static readonly string[] IntTypeNames = { "Byte", "Int16", "Int32", "Nullable`1", "SByte", "UInt16", "UInt32"};
        private static readonly string[] DoubleTypeNames = { "Double", "Int64", "Nullable`1", "Single"};
        private static readonly Type Type = typeof(PHPSerializedStream);
        private static readonly Type ListType = typeof(IList);
        private static readonly Type DictType = typeof(IDictionary);
        private static readonly Type StringType = typeof(string);
        private static readonly Type NullableType = typeof(Nullable<>);
        private static readonly Type BoolType = typeof(bool);
        private static readonly Type DblType = typeof(double);
        private static readonly Type IntType = typeof(int);
        private static readonly Type HashTableType = typeof(Hashtable);
        private string debug;

        private const BindingFlags SET_FLAGS =
            BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Public |
            BindingFlags.Instance | BindingFlags.FlattenHierarchy |
            BindingFlags.NonPublic | BindingFlags.Static;

        public PHPSerializedStream(string searializedData, Encoding enc) : this(enc.GetBytes(searializedData), enc) {
            debug = searializedData;
        }

        public PHPSerializedStream(byte[] serializedData, Encoding enc) {
            _enc = enc;
            _ms = new MemoryStream(serializedData);
        }

        public long Position { get { return _ms.Position; } }

        public char Peek() {
            char peek = ReadChar();
            _ms.Position--;

            return peek;
        }

        #region Read

        public char ReadChar() {
            int b = _ms.ReadByte();
            if (b == -1) {
                return (char) 0;
            }

            if (b > 32) {
                return (char) b;
            }
            return ReadChar();
        }

        public string ReadString() {
            CheckString("s:");

            int byteLength = ReadIntegerValue();
            CheckChar(':');

            string readString = ReadStringContents(byteLength + 2);
            CheckChar(';');

            return readString;
        }

        private string ReadStringContents(int byteLength) {
            byte[] read = new byte[byteLength];

            int readBytes = _ms.Read(read, 0, byteLength);

            string readString = Encoding.UTF8.GetString(read);
            if (readBytes < byteLength || readBytes == 0) {
                throw new ParsingException("a string of byte lenght " + byteLength, readString, 0, _ms.Position);
            }

            if (!string.IsNullOrEmpty(readString)) {
                readString = readString.Trim('"');
            }

            return readString;
        }

        private int ReadIntegerValue() {
            List<char> chars = new List<char>();

            while (true) {
                char read = ReadChar();

                if (read < '0' || read > '9') {
                    _ms.Position--;
                    break;
                }
                chars.Add(read);
            }

            int value;
            string strValue = new string(chars.ToArray());
            if (!int.TryParse(strValue, out value)) {
                throw new ParsingException("an integer value", strValue, 0, _ms.Position);
            }
            return value;
        }

        private double ReadDoubleValue() {
            int numPeriods = 0;
            List<char> chars = new List<char>();

            while (true) {
                char read = ReadChar();

                if ((read < '0' || read > '9' || numPeriods >= 2) && read != '.') {
                    _ms.Position--;
                    break;
                }

                if (read == '.') {
                    numPeriods++;
                }

                chars.Add(read);
            }

            double value;
            string strValue = new string(chars.ToArray());
            if (!double.TryParse(strValue, out value)) {
                throw new ParsingException("an integer value", strValue, 0, _ms.Position);
            }
            return value;
        }

        private void CheckChar(char expected) {
            char read = ReadChar();
            if (read != expected) {
                throw new ParsingException(expected.ToString(CultureInfo.InvariantCulture), read.ToString(CultureInfo.InvariantCulture), 0, _ms.Position);
            }
        }

        private void CheckString(string expected) {
            byte[] str = new byte[expected.Length];
            _ms.Read(str, 0, expected.Length);

            string readString = _enc.GetString(str);
            if (string.Compare(expected, readString, StringComparison.Ordinal) != 0) {
                throw new ParsingException("\""+expected+"\"", readString, 0, _ms.Position);
            }
        }

        public object ReadNull() {
            CheckString("N;");
            return null;
        }

        public int ReadInteger() {
            CheckString("i:");

            int val = ReadIntegerValue();
            CheckChar(';');

            return val;
        }

        public double ReadDouble() {
            CheckString("d:");

            double val = ReadDoubleValue();

            CheckChar(';');

            return val;
        }

        public bool ReadBoolean() {
            CheckString("b:");

            char bVal = ReadChar();
            bool b;
            if (bVal == '1') {
                b = true;
            }
            else if (bVal == '0') {
                b = false;
            }
            else {
                throw new ParsingException("boolean value of '1' or '0'", bVal.ToString(CultureInfo.InvariantCulture), 0, _ms.Position);
            }

            CheckChar(';');

            return b;
        }

        public IEnumerable ReadArray() {
            CheckString("a:");
            int len = ReadIntegerValue();
            CheckString(":{");

            Hashtable ht = null;
            if (Peek() == 's') {
                ht = ReadDictionaryElements(len);
            }

            if (ht != null) {
                CheckChar('}');
                return ht;
            }

            long pos = _ms.Position;
            object[] arr;
            if (!ReadArrayElements(len, out arr)) {
                _ms.Seek(pos, SeekOrigin.Begin);

                ht = ReadDictionaryElements(len);
                CheckChar('}');

                throw new ParsingException("An error has occured while parsing an array. The serialized data is probably malformed.");
            }

            CheckChar('}');
            return arr;
        }

        public object[] ReadSingleTypeArray() {
            CheckString("a:");
            int len = ReadIntegerValue();
            CheckString(":{");

            object[] arr;
            ReadArrayElements(len, out arr);

            CheckChar('}');

            return arr;
        }

        public Hashtable ReadAsociativeArray() {
            CheckString("a:");
            int len = ReadIntegerValue();
            CheckString(":{");

            Hashtable ht = ReadDictionaryElements(len);

            CheckChar('}');

            return ht;
        }

        private Hashtable ReadDictionaryElements(int len) {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < len; i++) {
                object key = ReadKey();
                object value = ReadElement();
                ht.Add(key, value);
            }

            return ht;
        }

        private object ReadKey() {
            char peek = Peek();
            if (peek == 's') {
                return ReadString();
            }

            if (peek == 'i') {
                return ReadInteger();
            }
            throw new ParsingException("an integer or string key", peek.ToString(CultureInfo.InvariantCulture), 0, _ms.Position);
        }

        private bool ReadArrayElements(int len, out object[] arr) {
            object[] elements = new object[len];

            for (int i = 0; i < len; i++) {
                int idx = ReadInteger();

                if (idx < 0 || idx > len) {
                    arr = null;
                    return false;
                }
                elements[idx] = ReadElement();
            }

            arr = elements;
            return true;
        }

        private object ReadElement() {
            switch (Peek()) {
                case 's':
                    return ReadString();
                case 'N':
                    return ReadNull();
                case 'i':
                    return ReadInteger();
                case 'd':
                    return ReadDouble();
                case 'b':
                    return ReadBoolean();
                case 'a':
                    return ReadArray();
                case 'O':
                    return ReadObject();
                default:
                    throw new ParsingException("Uknown type or malformed data detected.");
            }
        }

        public dynamic ReadObject() {
            CheckString("O:");
            int nameByteLenght = ReadIntegerValue();
            CheckChar(':');

            string objName = ReadStringContents(nameByteLenght + 2);
            CheckChar(':');

            int numFields = ReadIntegerValue();
            CheckString(":{");

            dynamic dyn = new ExpandoObject();
            dyn.__ClassName = objName;

            for (int i = 0; i < numFields; i++) {
                string key = ReadString();
                object value = ReadElement();

                ((IDictionary<string, object>) dyn).Add(key, value);
            }

            CheckChar('}');

            return dyn;
        }

        #endregion

        #region Deserialize

        internal T DeserializeElement<T>(Type type = null) {
            Type t = type ?? typeof(T);

            Debug.WriteLine("Deserializing element with expected type: "+t.FullName);
            Debug.Indent();

            object obj;
            switch (Peek()) {
                case 's': {
                    Debug.WriteLine("Found string:");

                    if (t == StringType || t.FullName == "System.Object") {
                        obj = ReadString();

                        Debug.WriteLine("Read string as string.");
                        Debug.Unindent();
                        return (T) obj;
                    }

                    if (t.IsValueType) {
                        T tObj = DeserializeStructFromString<T>(t);

                        Debug.WriteLine("Read string as struct.");
                        Debug.Unindent();
                        return tObj;
                    }

                    throw new ParsingException(t, "a serialized string", _ms.Position);
                }
                case 'N': {
                    Debug.WriteLine("Found null:");

                    if (t.IsClass || t == NullableType) {
                        obj = ReadNull();

                        T tObj = (T) obj;

                        Debug.WriteLine("Read null.");
                        Debug.Unindent();
                        return tObj;
                    }
                    throw new ParsingException(t, "a nullable type", _ms.Position);
                }
                case 'i': {
                    Debug.WriteLine("Found integer:");
                    Debug.Indent();

                    T tObj = DeserializeInteger<T>(t);

                    Debug.Unindent();
                    Debug.WriteLine("Read integer.");
                    Debug.Unindent();
                    return tObj;
                }
                case 'd': {
                    Debug.WriteLine("Found double:");
                    Debug.Indent();

                    T tObj = DeserializeDouble<T>(t);

                    Debug.Unindent();
                    Debug.WriteLine("Read double.");
                    Debug.Unindent();
                    return tObj;
                }
                case 'b': {
                    Debug.WriteLine("Found bool:");

                    if (t == BoolType) {
                        obj = ReadBoolean();
                        T tObj = (T) obj;

                        Debug.WriteLine("Read bool");
                        Debug.Unindent();
                        return tObj;
                    }
                    throw new ParsingException(t, "a serialized boolean", _ms.Position);
                }
                case 'a': {
                    Debug.WriteLine("Found array:");
                    Debug.Indent();

                    T tObj = DeserializePHPArray<T>(t);

                    Debug.Unindent();
                    Debug.WriteLine("Read Array");
                    Debug.Unindent();
                    return tObj;
                }
                case 'O': {
                    Debug.WriteLine("Found object:");
                    Debug.Indent();

                    T tObj = DeserializeObject<T>(t);

                    Debug.Unindent();
                    Debug.WriteLine("Read Object");
                    Debug.Unindent();
                    return tObj;
                }
                default:
                    throw new ParsingException("Uknown type or malformed data detected.");
            }
        }

        private T DeserializeStructFromString<T>(Type t) {
            object obj;
            string str = ReadString();
            if (t.Module.ScopeName == "CommonLanguageRuntimeLibrary") {
                switch (t.Name) {
                    case "DateTime":
                        DateTime dt;
                        if (DateTime.TryParse(str, out dt)) {
                            obj = dt;
                            return (T) obj;
                        }
                        break;
                    case "TimeSpan":
                        TimeSpan ts;
                        if (TimeSpan.TryParse(str, out ts)) {
                            obj = ts;
                            return (T) obj;
                        }
                        break;
                }
            }

            if (t.GetInterface("IPHPSerializable") != null) {
                IPHPSerializable instance = (IPHPSerializable) Activator.CreateInstance(t);
                obj = instance.FromPHPSerializedString(str);
                return (T) obj;
            }

            throw new ParsingException(t, "the type to implement IPHPSerializable", _ms.Position);
        }

        private T DeserializeObject<T>(Type type) {
            CheckString("O:");
            int nameByteLenght = ReadIntegerValue();
            CheckChar(':');

            string objName = ReadStringContents(nameByteLenght + 2);
            CheckChar(':');

            int numFields = ReadIntegerValue();
            CheckString(":{");

            Debug.WriteLine("Reading object with name: " + objName);
            Debug.WriteLine("Number of fields " + numFields);

            T obj = (T) Activator.CreateInstance(type);

            for (int i = 0; i < numFields; i++) {
                Debug.WriteLine("Starting to deserialize object member: " + i);
                Debug.Indent();

                DeserializeObjectMember(obj, type);

                Debug.Unindent();
            }

            CheckChar('}');

            return obj;
        }

        private void DeserializeObjectMember<T>(T obj, Type type) {
            string fieldName = ReadString();
            MemberInfo member = type.GetMember(fieldName, MemberTypes.Property | MemberTypes.Field, SET_FLAGS).FirstOrDefault();

            Debug.WriteLine("Member name: "+fieldName);

            if (member != null) {
                if (member.MemberType == MemberTypes.Field) {
                    FieldInfo fieldInfo = (FieldInfo) member;
                    Type memberType = fieldInfo.FieldType;

                    Debug.WriteLine("Member type: Field");
                    Debug.Indent();

                    MethodInfo deserializeMember = Type.GetMethod("DeserializeElement", BindingFlags.NonPublic | BindingFlags.Instance);
                    deserializeMember = deserializeMember.MakeGenericMethod(memberType);

                    object value = deserializeMember.Invoke(this, new object[] { memberType });

                    Debug.Unindent();
                    Debug.WriteLine("Succesfully deserialized member");

                    fieldInfo.SetValue(obj, value);
                }
                else if (member.MemberType == MemberTypes.Property) {
                    PropertyInfo propertyInfo = (PropertyInfo) member;
                    Type memberType = propertyInfo.PropertyType;

                    Debug.WriteLine("Member type: Property");
                    Debug.Indent();

                    MethodInfo deserializeMember = Type.GetMethod("DeserializeElement", BindingFlags.NonPublic | BindingFlags.Instance);
                    deserializeMember = deserializeMember.MakeGenericMethod(memberType);

                    object value = deserializeMember.Invoke(this, new object[] { memberType });

                    Debug.Unindent();
                    Debug.WriteLine("Succesfully deserialized member");

                    propertyInfo.SetValue(obj, value, null);
                }
            }
            else {
                //the member with the same name does not exits
                //read but do nothing with the output
                Debug.WriteLine("Member missing on class.");
                ReadElement();
            }
        }

        private T DeserializePHPArray<T>(Type type) {
            Debug.WriteLine("Deserializing array with type: "+type.FullName);

            if (type.IsArray) {
                Debug.WriteLine("Array is a plain array");

                Debug.WriteLine("Starting to deserialize array.");
                Debug.Indent();

                MethodInfo methodInfo = Type.GetMethod("DeserializeArray").MakeGenericMethod(type.GetElementType());
                T tObj = (T) methodInfo.Invoke(this, new object[] { });

                Debug.Unindent();
                Debug.WriteLine("Finished Deserializing array.");
                return tObj;
            }

            if (type.IsGenericType && type.Name == "List`1" && type.Namespace == "System.Collections.Generic") {
                Debug.WriteLine("Array is a generic list");

                Debug.WriteLine("Starting to deserialize generic list.");
                Debug.Indent();

                T tObj = DeserializeGenericList<T>(type);

                Debug.Unindent();
                Debug.WriteLine("Finished Deserializing array.");
                return tObj;
            }

            if (type.IsAssignableFrom(ListType)) {
                Debug.WriteLine("Array is an IList");

                Debug.WriteLine("Starting to deserialize an IList.");
                Debug.Indent();

                T tObj = DeserializeList<T>(type);

                Debug.Unindent();
                Debug.WriteLine("Finished Deserializing array.");
                return tObj;
            }

            if (type.IsGenericType && type.Name == "IDictionary`2" && type.Namespace == "System.Collections") {
                Debug.WriteLine("Array is an generic dictionary");

                Debug.WriteLine("Starting to deserialize a generic dictionary.");
                Debug.Indent();

                T tObj = DeserializeGenericDictionary<T>(type);

                Debug.Unindent();
                Debug.WriteLine("Finished desrializing generic dictionary.");
                return tObj;
            }

            if (type.IsAssignableFrom(DictType)) {
                Debug.WriteLine("Array is an IDictionary");

                Debug.WriteLine("Starting to deserialize an IDictionary.");
                Debug.Indent();

                Hashtable ht = ReadAsociativeArray();
                T tObj;
                if (type == HashTableType) {

                    tObj = ht.CastAs<T>();

                    Debug.WriteLine("Array is a hashtable");
                    Debug.Unindent();
                    Debug.WriteLine("Finished desrializing the hashtable.");
                    return tObj;
                }

                Debug.WriteLine("Converting Hashtable to the " + type.FullName);

                IDictionary dict = (IDictionary) Activator.CreateInstance(type);
                foreach (DictionaryEntry entry in ht) {
                    dict.Add(entry.Key, entry.Value);
                }

                tObj = (T) dict;

                Debug.WriteLine("Finished converting.");
                Debug.Unindent();
                Debug.WriteLine("Finished desrializing the hashtable.");
                return tObj;
            }
            return default(T);
        }

        private T DeserializeGenericDictionary<T>(Type type) {
            Type[] genericArguments = type.GetGenericArguments();

            MethodInfo genericDicParse = Type.GetMethod("DeserializeDictionary").MakeGenericMethod(genericArguments);
            IDictionary genericDict = (IDictionary) genericDicParse.Invoke(this, new object[] { });

            object instance = Activator.CreateInstance(type);

            MethodInfo addToDict = type.GetMethod("Add").MakeGenericMethod(genericArguments);
            foreach (DictionaryEntry entry in genericDict) {
                addToDict.Invoke(instance, new[] { entry.Key, entry.Value });
            }

            return (T) instance;
        }

        private T DeserializeList<T>(Type t) {
            object[] arr = DeserializeArray<object>();

            IList list = (IList) Activator.CreateInstance(t);
            foreach (object value in arr) {
                list.Add(value);
            }
            return (T) list;
        }

        private T DeserializeGenericList<T>(Type t) {
            Type[] genericArguments = t.GetGenericArguments();

            MethodInfo genericArrParse = Type.GetMethod("DeserializeArray").MakeGenericMethod(genericArguments[0]);
            object[] arr = (object[]) genericArrParse.Invoke(this, null);

            object genericList = Activator.CreateInstance(t);

            MethodInfo addToList = t.GetMethod("Add");
            foreach (object element in arr) {
                addToList.Invoke(genericList, new[] { element });
            }

            return (T) genericList;
        }

        private T DeserializeInteger<T>(Type type) {
            if (type.Module.ScopeName != "CommonLanguageRuntimeLibrary" || Array.BinarySearch(IntTypeNames, type.Name) < 0) {
                throw new ParsingException(type, "a serialized integer", _ms.Position);
            }

            object obj;
            if (type.Name == "Nullable`1") {
                Type nullableType = type.GetGenericArguments()[0];
                if (Array.BinarySearch(IntTypeNames, nullableType.Name) >= 0) {
                    obj = ReadInteger();
                    if (nullableType != IntType) {
                        obj = Convert.ChangeType(obj, nullableType);
                    }

                    return (T) Activator.CreateInstance(type, obj);
                }
            }
            else {
                obj = ReadInteger();

                if (type != IntType) {
                    obj = Convert.ChangeType(obj, type);
                }
                return (T) obj;
            }
            throw new ParsingException(type, "a serialized integer", _ms.Position);
        }

        private T DeserializeDouble<T>(Type type) {
            if (type.Module.ScopeName != "CommonLanguageRuntimeLibrary" || Array.BinarySearch(DoubleTypeNames, type.Name) < 0) {
                throw new ParsingException(type, "a serialized double", _ms.Position);
            }

            object obj;
            if (type.Name == "Nullable`1") {
                if (Array.BinarySearch(DoubleTypeNames, type.GetGenericArguments()[0].Name) >= 0) {
                    Type nullableType = type.GetGenericArguments()[0];
                    obj = ReadDouble();
                    if (nullableType != DblType) {
                        obj = Convert.ChangeType(obj, nullableType);
                    }

                    return (T) Activator.CreateInstance(type, obj);
                }
            }
            else {
                obj = ReadDouble();

                if (type != DblType) {
                    obj = Convert.ChangeType(obj, type);
                }
                return (T) obj;
            }
            throw new ParsingException(type, "a serialized double", _ms.Position);
        }

        public TElement[] DeserializeArray<TElement>() {
            CheckString("a:");
            int len = ReadIntegerValue();
            CheckString(":{");
            
            Debug.WriteLine("Deserializing array with expected elements of type " + typeof(TElement).FullName);
            Debug.Indent();

            TElement[] arr = DeserializeArrayElements<TElement>(len);

            Debug.Unindent();
            Debug.WriteLine("Finished deserializing array");

            CheckChar('}');

            return arr;
        }

        public IDictionary<TKey, TValue> DeserializeDictionary<TKey, TValue>() {
            CheckString("a:");
            int len = ReadIntegerValue();
            CheckString(":{");

            Debug.WriteLine("Deserializing dictionary with expected elements of type {0} and keys of type {1}", typeof(TValue).FullName, typeof(TKey));
            Debug.Indent();

            IDictionary<TKey,TValue> dict = DeserializeDictionaryElements<TKey, TValue>(len);

            Debug.Unindent();
            Debug.WriteLine("Finished deserializing dictionary");

            CheckChar(';');

            return dict;
        }

        private IDictionary<TKey, TValue> DeserializeDictionaryElements<TKey, TValue>(int len) {
            IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < len; i++) {
                TKey key = DeserializeKey<TKey>();
                TValue value = DeserializeElement<TValue>();
                dict.Add(key, value);
            }

            return dict;
        }

        private TElement[] DeserializeArrayElements<TElement>(int len) {
            TElement[] elements = new TElement[len];

            for (int i = 0; i < len; i++) {
                int idx = ReadInteger();

                if (idx < 0 || idx > len) {
                    return null;
                }

                Debug.WriteLine("Deserializing element with index: "+idx);
                Debug.Indent();

                elements[idx] = DeserializeElement<TElement>();

                Debug.Unindent();
                Debug.WriteLine("Finished deserializing element with index: "+idx);
            }
            return elements;
        }

        private T DeserializeKey<T>() {
            char peek = Peek();

            if (peek == 'i') {
                return ReadInteger().CastAs<T>();
            }

            if (peek == 's' && typeof(T) == StringType) {
                return ReadString().CastAs<T>();
            }
            throw new ParsingException("an integer or string key ('i' or 's')", peek.ToString(CultureInfo.InvariantCulture), 0, _ms.Position);
        }
        #endregion

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (_ms != null) {
                _ms.Dispose();
            }
        }
    }

}