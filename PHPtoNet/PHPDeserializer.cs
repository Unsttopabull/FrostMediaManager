#region

using System;
using System.Collections;
using System.Globalization;

#endregion

namespace Frost.PHPtoNET {
    internal sealed class PHPObject { }

    internal static class PHPDeserializer {
        #region Util

        /// <summary>Determines whether the specified source is valid PHP serialize() string.</summary>
        /// <param name="scanner">Scanner with the ability to reset to the beginning of the stream</param>
        /// <returns><c>true</c> if the specified source contains a valid PHP serialize() string; otherwise, <c>false</c>.</returns>
        public static bool IsValid(IResetableScanner scanner) {
            Parser parser = new Parser(scanner);
            bool isValid = parser.Parse();

            scanner.Reset();

            return isValid;
        }

        /// <summary>Determines the type in the serialized string</summary>
        /// <param name="scanner">Scanner with the ability to reset to the beginning of the stream</param>
        /// <param name="rewind">Rewind the source stream to the beginning if it supports seeking</param>
        /// <param name="className">if set to <c>true</c> and type is an object return the class name instead of "object".</param>
        /// <returns>The string representation of the type serialized, if type is an object and <paramref name="className"/> is <c>true</c> returns the object name</returns>
        public static string GetSerializedType(IResetableScanner scanner, bool rewind = false, bool className = true) {
            if (rewind) {
                scanner.Reset();
            }

            switch (scanner.Peek()) {
                case 's':
                    return "string";
                case 'N':
                    return "null";
                case 'i':
                    return "integer";
                case 'd':
                    return "double";
                case 'b':
                    return "boolean";
                case 'a':
                    return "array";
                case 'O':
                    string type = null;
                    if (className) {
                        scanner.NextToken(); //O
                        //:
                        if (scanner.NextToken().TokenType == Tokens.Colon) {
                            //len of class name
                            if (scanner.NextToken().TokenType == Tokens.Integer) {
                                type = scanner.NextToken().Lexem;
                            }
                        }
                        scanner.Reset();
                    }

                    return type ?? "object";
                default:
                    return "unknown";
            }
        }

        /// <summary>Parses the next serialized type</summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="memberType">The member type to deserialize to</param>
        /// <returns>The next serialized type casted to an object</returns>
        internal static dynamic GetNextType(IScanner scanner, Type memberType = null) {
            switch (scanner.Peek()) {
                case 's':
                    return ParseString(scanner);
                case 'N':
                    return ParseNull(scanner);
                case 'i':
                    return ParseInt(scanner);
                case 'd':
                    return ParseDouble(scanner);
                case 'b':
                    return ParseBool(scanner);
                case 'a':
                    return (memberType == null || memberType == typeof(Hashtable))
                                   ? PHPArrayDeserializer.ParseMixedKeyArray(scanner)
                                   : PHPArrayDeserializer.ParseSingleTypeArrayByType(scanner, memberType);
                case 'O':
                    if (memberType == null) {
                        return new PHPObject(); //!!
                    }

                    PHPObjectParser objectParser = new PHPObjectParser(scanner);
                    return objectParser.ParseObjByType(memberType);
                default:
                    throw new ParsingException("Type declaration", scanner.CurrToken());
            }
        }

        #endregion

        #region Parse methods for String, Bool, Double, Int

        /// <summary>Parses the PHP serialize() of a string.</summary>
        /// <param name="scanner">The scanner.</param>
        /// <returns>Deserialized string</returns>
        /// <exception cref="ParsingException">Throws if the source is not serialized string or is corrupted.</exception>
        public static string ParseString(IScanner scanner) {
            Token t;
            if (scanner.NextToken().Lexem == "s") {
                //:
                if (scanner.NextToken().TokenType == Tokens.Colon) {
                    //dolz niza
                    if (scanner.NextToken().TokenType == Tokens.Integer) {
                        //:
                        if (scanner.NextToken().TokenType == Tokens.Colon) {
                            t = scanner.NextToken(); //val
                            if (t.TokenType == Tokens.String) {
                                string str = t.Lexem;
                                //;
                                if (scanner.NextToken().TokenType == Tokens.Semicolon) {
                                    return str;
                                }
                                t = scanner.CurrToken();
                                throw new ParsingException("\";\"", t);
                            }
                            throw new ParsingException("a String", t);
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
            throw new ParsingException("\"s\"", t);
        }

        /// <summary> Parses the PHP serialize() of a boolean.</summary>
        /// <param name="scanner">The scanner.</param>
        /// <returns></returns>
        /// <exception cref="ParsingException">Throws if the source is not serialized boolean or is corrupted.</exception>
        public static bool ParseBool(IScanner scanner) {
            Token t;
            if (scanner.NextToken().Lexem == "b") {
                if (scanner.NextToken().TokenType == Tokens.Colon) {
                    t = scanner.NextToken(); //0 or 1
                    if (t.TokenType == Tokens.Integer) {
                        bool boolean;
                        switch (t.Lexem) {
                            case "1":
                                boolean = true;
                                break;
                            case "0":
                                boolean = false;
                                break;
                            default:
                                throw new ParsingException("boolean value of either 1 or 0", t.Lexem);
                        }

                        //;
                        if (scanner.NextToken().TokenType == Tokens.Semicolon) {
                            return boolean;
                        }
                        t = scanner.CurrToken();
                        throw new ParsingException("\";\"", t);
                    }
                    throw new ParsingException("boolean", Enum.GetName(typeof(Tokens), t.TokenType));
                }
                t = scanner.CurrToken();
                throw new ParsingException("\":\"", t);
            }
            t = scanner.CurrToken();
            throw new ParsingException("\"b\"", t);
        }

        /// <summary>Parses the PHP serialize() of a floating point number.</summary>
        /// <param name="scanner">The scanner.</param>
        /// <returns></returns>
        /// <exception cref="ParsingException">Throws if the source is not serialized floating point number or is corrupted.</exception>
        public static double ParseDouble(IScanner scanner) {
            Token t; //d
            if (scanner.NextToken().Lexem == "d") {
                //:
                if (scanner.NextToken().TokenType == Tokens.Colon) {
                    t = scanner.NextToken(); //val
                    if (t.TokenType == Tokens.Double || t.TokenType == Tokens.Integer) {
                        double dbl = double.Parse(t.Lexem, CultureInfo.InvariantCulture);
                        //;
                        if (scanner.NextToken().TokenType == Tokens.Semicolon) {
                            return dbl;
                        }
                        t = scanner.CurrToken();
                        throw new ParsingException("\";\"", t);
                    }
                    throw new ParsingException("a Double", t);
                }
                t = scanner.CurrToken();
                throw new ParsingException("\":\"", t);
            }
            t = scanner.CurrToken();
            throw new ParsingException("\"d\"", t);
        }

        /// <summary>Parses the PHP serialize() of an integer.</summary>
        /// <param name="scanner">The scanner.</param>
        /// <returns></returns>
        /// <exception cref="ParsingException">Throws if the source is not serialized integer or is corrupted.</exception>
        public static int ParseInt(IScanner scanner) {
            Token t;
            if (scanner.NextToken().Lexem == "i") {
                //:
                if (scanner.NextToken().TokenType == Tokens.Colon) {
                    t = scanner.NextToken();
                    //value
                    if (t.TokenType == Tokens.Integer) {
                        int integer = int.Parse(t.Lexem);
                        //;
                        if (scanner.NextToken().TokenType == Tokens.Semicolon) {
                            return integer;
                        }
                        t = scanner.CurrToken();
                        throw new ParsingException("\";\"", t);
                    }
                    throw new ParsingException("an Integer", t);
                }
                t = scanner.CurrToken();
                throw new ParsingException("\":\"", t);
            }
            t = scanner.CurrToken();
            throw new ParsingException("\"i\"", t);
        }

        /// <summary>Parses the PHP serialize() of a NULL.</summary>
        /// <param name="scanner">The scanner.</param>
        /// <returns></returns>
        public static object ParseNull(IScanner scanner) {
            Token t;
            if (scanner.NextToken().Lexem == "N") {
                //;
                if (scanner.NextToken().TokenType == Tokens.Semicolon) {
                    return null;
                }
                t = scanner.CurrToken();
                throw new ParsingException("\";\"", t);
            }
            t = scanner.CurrToken();
            throw new ParsingException("\"N\"", t);
        }
        #endregion
    }
}
