using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace Frost.PHPtoNET {
    public static class PHPArrayDeserializer{
        /// <summary>Parses the single type unsigned integer keyed array.</summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="arrayType">Type of array to parse</param>
        /// <exception cref="ParsingException">Throws if the source is not a serialized single type integer keyed array or is corrupted.</exception>
        /// <returns>Array of the specified type</returns>
        public static dynamic ParseSingleTypeArrayByType(IScanner scanner, Type arrayType) {
            if (arrayType.IsArray) {
                arrayType = arrayType.GetElementType();
            }

            MethodInfo mi = typeof(PHPArrayDeserializer).GetMethod("ParseSingleTypeArray", BindingFlags.Static | BindingFlags.Public);
            mi = mi.MakeGenericMethod(arrayType);
            return mi.Invoke(null, new object[] {scanner});
        }

        /// <summary>Parses the single type unsigned integer keyed array.</summary>
        /// <typeparam name="T">The type of values contained in the array, if the type is <see cref="System.Object"/> values can be mixed</typeparam>
        /// <param name="scanner">The scanner to use when parsing.</param>
        /// <exception cref="ParsingException">Throws if the source is not a serialized single type integer keyed array or is corrupted.</exception>
        /// <returns>The array containing the parsed values of type <typeparamref name="T"/></returns>
        public static T[] ParseSingleTypeArray<T>(IScanner scanner) {
            Token t;
            if (scanner.NextToken().Lexem == "a") {
                //:
                if (scanner.NextToken().TokenType == Tokens.Colon) {

                    t = scanner.NextToken(); //Num of array el.
                    if (t.TokenType == Tokens.Integer) {

                        int numEl = Int32.Parse(t.Lexem);
                        //:
                        if (scanner.NextToken().TokenType == Tokens.Colon) {
                            if (scanner.NextToken().Lexem == "{") {

                                //arr contents
                                T[] ht = Elements<T>(scanner, numEl);
                                if (scanner.NextToken().Lexem == "}") {
                                    return ht;
                                }
                                t = scanner.CurrToken();
                                throw new ParsingException("\"}\"", t);
                            }
                            t = scanner.CurrToken();
                            throw new ParsingException("\"{\"", t);
                        }
                        t = scanner.CurrToken();
                        throw new ParsingException("\":\"", t);
                    }
                    t = scanner.CurrToken();
                    throw new ParsingException("an Integer", t);
                }
                t = scanner.CurrToken();
                throw new ParsingException("\":\"", t);
            }
            t = scanner.CurrToken();
            throw new ParsingException("\"a\"", t);
        }

        /// <summary>Parses integer or string keyed array. Order is not preserved</summary>
        /// <param name="scanner">The scanner.</param>
        /// <exception cref="ParsingException">Throws if the source is not a serialized integer or string keyed array or is corrupted.</exception>
        /// <returns>The array containing the parsed values</returns>
        public static Hashtable ParseMixedKeyArray(IScanner scanner) {
            Token t;
            if (scanner.NextToken().Lexem == "a") {
                //:
                if (scanner.NextToken().TokenType == Tokens.Colon) {

                    t = scanner.NextToken(); //Num of array el.
                    if (t.TokenType == Tokens.Integer) {

                        int numEl = Int32.Parse(t.Lexem);
                        //:
                        if (scanner.NextToken().TokenType == Tokens.Colon) {
                            if (scanner.NextToken().Lexem == "{") {

                                //arr contents
                                Hashtable ht = Elements(scanner, numEl);
                                if (scanner.NextToken().Lexem == "}") {
                                    return ht;
                                }
                                t = scanner.CurrToken();
                                throw new ParsingException("\"}\"", t);
                            }
                            t = scanner.CurrToken();
                            throw new ParsingException("\"{\"", t);
                        }
                        t = scanner.CurrToken();
                        throw new ParsingException("\":\"", t);
                    }
                    t = scanner.CurrToken();
                    throw new ParsingException("an Integer", t);
                }
                t = scanner.CurrToken();
                throw new ParsingException("\":\"", t);
            }
            t = scanner.CurrToken();
            throw new ParsingException("\"a\"", t);
        }

        /// <summary>Parses the elements of single value type unsigned integer keyed array</summary>
        /// <typeparam name="T">The type of values contained in the array, if <see cref="object"/> values can be mixed</typeparam>
        /// <param name="scanner">The scanner.</param>
        /// <param name="numEl">The number of elements to parse</param>
        /// <exception cref="ParsingException">Throws if a key is not a positive integer or if a cast of an array value failed</exception>
        /// <returns>Parsed elements in an array of specified type <typeparamref name="T"/>.</returns>
        private static T[] Elements<T>(IScanner scanner, int numEl) {
            T[] arr = new T[numEl];
            for (int i = 0; i < numEl; i++) {
                int k = Key<int>(scanner);

                if (k < 0){
                    Token t = scanner.CurrToken();
                    throw new ParsingException("pozitive key index", k.ToString(CultureInfo.InvariantCulture), t.Line, t.Column);
                }

                dynamic valObj = PHPDeserializer.GetNextType(scanner, typeof(T));
                try {
                    arr[k] = valObj;
                }
                catch (InvalidCastException) {
                    throw new ParsingException(valObj.GetType().ToString(), valObj.GetType().ToString());
                }
            }
            return arr;
        }

        /// <summary>Parses the elements of a mixed keyed array</summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="numEl">The number of elements to parse</param>
        /// <returns>A <see cref="Hashtable"/> of parsed values</returns>
        private static Hashtable Elements(IScanner scanner, int numEl) {
            Hashtable ht = new Hashtable();
            for (int i = 0; i < numEl; i++){
                var key = Key<object>(scanner);

                var val = PHPDeserializer.GetNextType(scanner);
                ht.Add(key, val);
            }
            return ht;
        }

        /// <summary>Keys the specified scanner.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="scanner">The scanner.</param>
        /// <returns></returns>
        private static T Key<T>(IScanner scanner) {
            char lex = scanner.Peek();

            T key = default(T);
            bool intKey = false;
            try {
                switch (lex) {
                    case 'i':
                        intKey = true;
                        key = PHPDeserializer.ParseInt(scanner).CastAs<T>();
                        return key;
                    case 's':
                        key = PHPDeserializer.ParseString(scanner).CastAs<T>();
                        return key;
                    default:
                        Token t = scanner.CurrToken();
                        throw new ParsingException("Integer or String key", t);
                }
            }
            catch (InvalidCastException) {
                throw new ParsingException(key.GetType().ToString(), (intKey ? typeof(int) : typeof(string)).ToString());
            }
        }
    }
}