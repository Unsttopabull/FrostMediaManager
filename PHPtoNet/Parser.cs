using System;
using System.Diagnostics;

namespace Frost.PHPtoNET {
    internal class Parser {
        private readonly IScanner _scanner;

        public Parser(IScanner scanner) {
            _scanner = scanner;
        }

        public bool Parse() {
            bool pravilen = Tip();

            bool eof = _scanner.CurrToken().EOF;
            if (!eof) {
                Console.WriteLine();
                Console.WriteLine("Ni EOF!");
                Debug.WriteLine("Ni EOF!");
            }

            return pravilen && eof;
        }

        private bool Tip() {
            Token t = _scanner.NextToken();
            switch (t.Lexem) {
                case "s":
                    return Str();
                case "N":
                    return Null();
                case "i":
                    return Int();
                case "d":
                    return Dbl();
                case "b":
                    return Bool();
                case "a":
                    return Arr();
                case "O":
                    return Obj();
            }
            return false;
        }

        private bool Obj() {
            Token t = _scanner.CurrToken();

            if (t.Lexem == "O") {
                t = _scanner.NextToken();  //:

                if (t.Lexem == ":") {
                    _scanner.NextToken(); //len imena
                    t = _scanner.NextToken(); //:

                    if (t.Lexem == ":") {
                        _scanner.NextToken(); //ClassName
                        t = _scanner.NextToken(); //:

                        if (t.Lexem == ":") {
                            _scanner.NextToken(); //num prop
                            _scanner.NextToken(); //:
                            t = _scanner.NextToken(); //{

                            if (t.Lexem == "{") {
                                //obj vsebina
                                Lastnosti();

                                t = _scanner.CurrToken(); //}
                                return t.Lexem == "}";
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool Arr() {
            if (_scanner.CurrToken().Lexem == "a") {
                if (_scanner.NextToken().Lexem == ":") {
                    _scanner.NextToken(); //Num of array el. 

                    if (_scanner.NextToken().Lexem == ":") {
                        if (_scanner.NextToken().Lexem == "{") {
                            //arr contents
                            if (!Elementi()) {
                                return false;
                            }

                            if (_scanner.NextToken().Lexem == "}") {
                                return _scanner.NextToken().Lexem == ";";
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool Bool() {
            if (_scanner.CurrToken().Lexem == "b") {
                if (_scanner.NextToken().Lexem == ":") {
                    return _scanner.NextToken().Lexem == ";";
                }
            }
            return false;
        }

        private bool Dbl(){
            Token t = _scanner.CurrToken(); //d
            if (t.Lexem == "d"){
                t = _scanner.NextToken(); //:
                if (t.Lexem == ":"){
                    _scanner.NextToken(); //val
                    t = _scanner.NextToken(); //;
                    return t.Lexem == ";";
                }
            }
            return false;
        }

        private bool Int() {
            if (_scanner.CurrToken().Lexem == "i") {
                if (_scanner.NextToken().Lexem == ":") {
                    return _scanner.NextToken().Lexem == ";";
                }
            }
            return false;
        }

        private bool Null() {
            if (_scanner.CurrToken().Lexem == "N") {
                return _scanner.NextToken().Lexem == ";";
            }
            return false;
        }

        private bool Str() {
            Token t = _scanner.CurrToken();

            if (t.Lexem == "s") {
                t = _scanner.NextToken(); //:

                if (t.Lexem == ":") {
                    _scanner.NextToken(); //dolz niza
                    _scanner.NextToken(); //:
                    _scanner.NextToken(); //niz;
                    t = _scanner.NextToken(); //;

                    return t.Lexem == ";";
                }
            }
            return false;
        }

        private bool Kljuc() {
            string lex = _scanner.CurrToken().Lexem;

            if (lex == "s") {
                return Str();
            }
            return lex == "i" && Int();
        }

        private bool Elementi() {
            string lex = _scanner.NextToken().Lexem;
            if (lex == "s" || lex == "i") {
                return Element() && Elementi();
            }
            return true;
        }

        private bool Element() {
            return Kljuc() && Tip();
        }

        private bool Lastnosti() {
            return Lastnost() && Lastnosti();
        }

        private bool Lastnost() {
            _scanner.NextToken();
            return Str() && Tip();
        }
    }
}