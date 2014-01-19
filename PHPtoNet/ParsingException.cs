#region

using System;

#endregion

namespace Frost.PHPtoNET {
    internal class ParsingException : Exception {
        private const string MSG = "Error occured while parsing, expected {0}, found \"{1}\" on line {2} column {3}.";
        private const string MSG2 = "Expcted value of type {0} got {1}";
        private readonly long _column;
        private readonly bool _custom;
        private readonly string _customMessage = "";
        private readonly string _expected;
        private readonly int _line;
        private readonly bool _msg;
        private readonly string _read;
        private readonly string[] _typeNames;


        public ParsingException(string expected, string read, int line, long column) {
            _read = read;
            _line = line;
            _column = column;
            _expected = expected;
            _msg = true;
            _typeNames = null;
        }

        public ParsingException(string expected, Token token) {
            _read = token.Lexem;
            _line = token.Line;
            _column = token.Column;
            _expected = expected;
            _msg = true;
            _typeNames = null;
        }

        public ParsingException(string expected, string found) {
            _typeNames = new[] {expected, found};
            _msg = false;
        }

        public ParsingException(string customMessage) {
            _custom = true;
            _customMessage = customMessage;
        }

        public override string Message {
            get {
                if (!_custom) {
                    return _msg
                        ? string.Format(MSG, _expected, _read, _line, _column)
                        : string.Format(MSG2, _typeNames[0], _typeNames[1]);
                }
                return _customMessage;
            }
        }
    }
}
