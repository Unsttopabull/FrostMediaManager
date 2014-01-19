using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Frost.PHPtoNET {

    public class PHPSerializedStream : IDisposable {
        private readonly Encoding _enc;
        private readonly MemoryStream _ms;

        public PHPSerializedStream(string searializedData, Encoding enc) {
            _enc = enc;
            _ms = new MemoryStream(enc.GetBytes(searializedData));
        }

        public PHPSerializedStream(byte[] serializedData, Encoding enc) {
            _enc = enc;
            _ms = new MemoryStream(serializedData);
        }

        public char Peek() {
            char peek = ReadChar();
            _ms.Position--;

            return peek;
        }

        public char ReadChar() {
            int b = _ms.ReadByte();
            if (b == -1) {
                return (char)0;
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

            string readString = ReadStringContents(byteLength+2);
            CheckChar(';');

            return readString;
        }

        private string ReadStringContents(int byteLength) {
            byte[] read = new byte[byteLength];

            int readBytes = _ms.Read(read, 0, byteLength);

            string readString = Encoding.UTF8.GetString(read);
            if (readBytes < byteLength || readBytes == 0) {
                throw new ParsingException("a string of byte lenght "+byteLength, readString, 0, _ms.Position);
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
                throw new ParsingException(expected, readString, 0, _ms.Position);
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

            CheckChar(',');

            return b;
        }

        public IEnumerable ReadArray() {
            CheckString("a:");
            int len = ReadIntegerValue();
            CheckString(":{");

            Hashtable ht = null;
            if (Peek() == 's') {
                ht = ReadDictionary(len);
            }

            if (ht != null) {
                CheckChar('}');
                return ht;
            }

            long pos = _ms.Position;
            Array arr;
            if (!ReadArrayElements(len, out arr)) {
                _ms.Seek(pos, SeekOrigin.Begin);

                ht = ReadDictionary(len);
                CheckChar('}');

                throw new ParsingException("An error has occured while parsing an array. The serialized data is probably malformed.");
            }

            CheckChar('}');
            return arr;
        }

        private Hashtable ReadDictionary(int len) {
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
                return ReadString().Trim('"');
            }

            if (peek == 'i') {
                return ReadInteger();
            }
            throw new ParsingException("an integer or string key", peek.ToString(CultureInfo.InvariantCulture), 0, _ms.Position);
        }

        private bool ReadArrayElements(int len, out Array arr) {
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

            string objName = ReadStringContents(nameByteLenght + 2).Trim('"');
            CheckChar(':');

            int numFields = ReadIntegerValue();
            CheckString(":{");

            dynamic dyn = new ExpandoObject();
            dyn.Name = objName;

            for (int i = 0; i < numFields; i++) {
                string key = ReadKey().ToString();
                object value = ReadElement();

                ((IDictionary<string, object>) dyn).Add(key, value);
            }

            return dyn;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (_ms != null) {
                _ms.Dispose();
            }
        }
    }

}