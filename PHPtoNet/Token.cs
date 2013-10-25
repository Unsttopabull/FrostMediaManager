using System;

namespace PHPSerialize {
    public enum Tokens {
        Error = 0,
        Integer = 1,
        Double = 2,
        TypeDecl = 3,
        String = 4,
        Ignore = 5,
        Parentheses = 6,
        Colon = 7,
        Semicolon = 8
    }

    public class Token {
        public Token(string lexem, int column, int row, int token, bool eof) {
            Lexem = lexem;
            Column = column;
            Line = row;
            TokenType = (Tokens)token;
            EOF = eof;
        }

        public Token(string lexem, int column, int row, Tokens token, bool eof) {
            Lexem = lexem;
            Column = column;
            Line = row;
            TokenType = token;
            EOF = eof;
        }

        /// <summary>Vrne ali nastavi Leksem</summary>
        /// <example>"20,2" | "*"</example>
        /// <value>Leksem</value>
        public string Lexem { get; set; }

        /// <summary>Vrne ali nastavi stolpec, kjer se token pojavi</summary>
        /// <example>1</example>
        /// <value>Stolpec</value>
        public int Column { get; set; }

        /// <summary>Gets or sets the row.</summary>
        /// <example>1</example>
        /// <value>vrstica</value>
        public int Line { get; set; }   // 1

        /// <summary>Vrednost, ki jo token predstavlja</summary>
        /// <example>float, separator, operator, ...</example>
        public Tokens TokenType { get; set; }

        /// <summary>
        /// Vrne ali nastavi ali trenutni token predstavlja konec datoteke.
        /// </summary>
        /// <value>
        ///   <c>true</c> če konec datoteke; drugače, <c>false</c>.
        /// </value>
        public bool EOF { get; set; }

        public override String ToString() {
            return string.Format("\"{0}\" {1} ({2}, {3}) {4}", Lexem, TokenType, Line, Column, EOF ? Environment.NewLine+"EOF" : "");
        }
    }
}