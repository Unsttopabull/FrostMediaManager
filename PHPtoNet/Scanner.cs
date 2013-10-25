using System;
using System.IO;
using System.Text;

namespace PHPSerialize {
    public interface IScanner {
        Token CurrToken();
        Token NextToken();
        char Peek();
    }

    public interface IResetableScanner: IScanner {
        void Reset();
    }

    public class Scanner : IResetableScanner, IDisposable {
        /// <summary>Predstavlja število stanj avtomata</summary>
        private const int MAX_STATE = 12;

        /// <summary>Predstavlja začetno stanje avtomata</summary>
        private const int START_STATE = 1;

        private const int STRING_MID = 5;
        private const int STRING_END = 6;

        /// <summary>Število, ki v tabeli stanj. Pove da ni prehoda</summary>
        private const int NO_TRANSITION = 0;

        /// <summary>Tabela stanj</summary>
        private readonly int[,] _avtomat = new int[MAX_STATE,0xffff]; //UTF

        /// <summary>Tabela končnih stanj</summary>
        private readonly int[] _finalStates = new int[MAX_STATE];

        /// <summary>Current number of read bytes from the stream</summary>
        private long _bytesRead;

        /// <summary>Predstavlja zaporedno številko trenutno branega stolpca v datoteki</summary>
        private int _column;

        /// <summary>Predstavlja trenutni osnovni leksikalni simbol</summary>
        private Token _lastToken;

        /// <summary>Predstavlja zaporedno številko trnutno brane vrstice v datoteki</summary>
        private int _line;

        /// <summary>Predstavlja podatkovi tok vhodne datoteke</summary>
        private StreamReader _source;


        /// <summary>Scanner constructor, requires string source as a stream</summary>
        /// <param name="source">String source stream</param>
        /// <param name="rewind">Rewind the source stream to the beginning if it supports seeking</param>
        public Scanner(StreamReader source, bool rewind = false) {
            _line = 1;
            _column = 1;
            InitAvtomat();
            _source = source;

            if (!rewind || !_source.BaseStream.CanSeek) {
                return;
            }

            ResetStream();
        }

        /// <summary>Scanner constructor</summary>
        /// <param name="source">String to be parsed</param>
        /// <param name="enc">Encoding in which the source string is saved, if <b>null</b> defaults to <see cref="Encoding.UTF8"/></param>
        public Scanner(string source, Encoding enc = null) {
            _line = 1;
            _column = 1;
            InitAvtomat();

            if (enc == null) {
                enc = Encoding.UTF8;
            }
            _source = new StreamReader(new MemoryStream(enc.GetBytes(source)));
        }

        public Token NextToken() {
            return _lastToken = NextTokenImp();
        }

        /// <summary>
        ///     Return the current Token
        /// </summary>
        /// <returns>Current token</returns>
        public Token CurrToken() {
            return _lastToken;
        }

        #region Stream operations

        public char Peek() {
            return (char) _source.Peek();
        }

        public void Reset() {
            _column = 0;
            _line = 0;
            ResetStream();
        }

        /// <summary>
        ///     Reads one character in the stream
        /// </summary>
        /// <returns>Integral representation of the read character</returns>
        internal int Read() {
            int tmp = '\0';

            try {
                tmp = _source.Read();
                _column++;
                _bytesRead++;
            }
            catch (IOException e) {
                Console.Error.WriteLine(e.StackTrace);
            }

            if (tmp == '\n') {
                _line++;
                _column = 1;
            }
            return tmp;
        }

        private void ResetStream() {
            _source.Read(); //ensure that BOM is detected if configured to do so
            _source.BaseStream.Position = 0;
            _source = new StreamReader(_source.BaseStream, _source.CurrentEncoding, false);
        }

        #endregion

        /// <summary>
        ///     Initializes DFA
        /// </summary>
        private void InitAvtomat() {
            const int INT_FINAL = 2;
            const int DBL_FINAL = 4;
            const int IGNORE = 7;
            const int TYPE_FINAL = 8;
            const int COLON = 9;
            const int PARENTHESES = 10;
            const int SEMICOLON = 11;


            //#števila
            _avtomat[START_STATE, '-'] = INT_FINAL; //negative nums
            for (int i = '0'; i <= '9'; i++) {
                //#int
                // če smo v stanju START_STATE ali 2 ob cifri, gremo v stanje 2
                _avtomat[START_STATE, i] = _avtomat[INT_FINAL, i] = INT_FINAL;

                //#double, decimalni del 
                // če smo v stanju 3 ali 4 ob cifri, gremo v stanje 4
                _avtomat[3, i] = _avtomat[DBL_FINAL, i] = DBL_FINAL;
            }

            //#TypeDecl
            _avtomat[START_STATE, 'i'] = TYPE_FINAL;
            _avtomat[START_STATE, 'd'] = TYPE_FINAL;
            _avtomat[START_STATE, 'b'] = TYPE_FINAL;
            _avtomat[START_STATE, 'a'] = TYPE_FINAL;
            _avtomat[START_STATE, 'O'] = TYPE_FINAL;
            _avtomat[START_STATE, 'N'] = TYPE_FINAL;
            _avtomat[START_STATE, 's'] = TYPE_FINAL;

            //type end 
            _avtomat[START_STATE, ';'] = SEMICOLON;

            //#colon
            _avtomat[START_STATE, ':'] = COLON;

            //#parentheses
            _avtomat[START_STATE, '{'] = PARENTHESES;
            _avtomat[START_STATE, '}'] = PARENTHESES;

            //#double
            // če smo v stanju 2 ob piki ali vejici, gremo v stanje 3;
            _avtomat[INT_FINAL, '.'] = 3;
            _avtomat[INT_FINAL, ','] = 3;

            //#string start
            _avtomat[START_STATE, '"'] = STRING_MID;

            //#string unicode content
            for (int j = 0; j < 0xffff; j++) {
                _avtomat[STRING_MID, j] = STRING_MID;
            }

            //#string end
            _avtomat[STRING_MID, '"'] = STRING_END;

            #region WS

            // če smo v stanju START_STATE ob praznem mestu, gremo v stanje IGNORE
            _avtomat[START_STATE, '\n'] =
                    _avtomat[START_STATE, ' '] = _avtomat[START_STATE, '\t'] = _avtomat[START_STATE, '\r'] = IGNORE;
            // če smo v stanju IGNORE ob praznem mestu, gremo v stanje IGNORE 
            _avtomat[IGNORE, '\n'] = _avtomat[IGNORE, ' '] = _avtomat[IGNORE, '\t'] = _avtomat[IGNORE, '\r'] = IGNORE;

            #endregion

            #region End states

            // stanji START_STATE, 3 nista končni, vrnemo sintaktično napako napako
            _finalStates[START_STATE] = _finalStates[3] = (int) Tokens.Error;

            _finalStates[INT_FINAL] = (int) Tokens.Integer;
            _finalStates[DBL_FINAL] = (int) Tokens.Double;
            _finalStates[STRING_END] = (int) Tokens.String;
            _finalStates[IGNORE] = (int) Tokens.Ignore;
            _finalStates[TYPE_FINAL] = (int) Tokens.TypeDecl;
            _finalStates[COLON] = (int) Tokens.Colon;
            _finalStates[PARENTHESES] = (int) Tokens.Parentheses;
            _finalStates[SEMICOLON] = (int) Tokens.Semicolon;

            #endregion
        }

        /// <summary>
        ///     Reads the next token in the stream
        /// </summary>
        /// <returns>The next token in the stream, if none exists returns EOF otherwise ERROR</returns>
        private Token NextTokenImp() {
            int currState = START_STATE;
            var lexem = new StringBuilder();
            int startCol = _column;
            int startLn = _line;
            do {
                int peek = _source.Peek();
                int newState = GetNextState(currState, peek);
                if (newState != NO_TRANSITION) {
                    //Don't capture " at the start and end of a string in a lexem
                    if (!IsStringStartOrEnd(currState, peek)) {
                        lexem.Append((char) Read());
                    }
                    else {
                        Read(); // ignore "
                        peek = _source.Peek(); //refresh the next symbol

                        //Insure proper handling of quotes inside the string
                        if (EscapedQuote(newState, peek) && currState != START_STATE) {
                            lexem.Append('"');
                            newState = STRING_MID;
                        }
                    }

                    currState = newState; //advance the state
                }
                else {
                    //If state is final and Token shoud not be ignored return the current token
                    //otherwise return the next one
                    if (IsFinalState(currState)) {
                        var token = new Token(lexem.ToString(), startCol, startLn, GetFinalState(currState),
                                              _source.EndOfStream);
                        return (token.TokenType == Tokens.Ignore)
                                       ? NextToken()
                                       : token;
                    }

                    //If we're not on a final state we check for EOF. If we find it we return it and end scanning,
                    //otherwise we return an error
                    return _source.EndOfStream
                                   ? new Token("EOF", startCol, startLn, Tokens.Ignore, true)
                                   : new Token("ERROR", startCol, startLn, Tokens.Error, false);
                }
            } while (true);
        }

        #region State Functions
        /// <summary>
        ///     Checks if a transition to a new state exists
        /// </summary>
        /// <param name="currState">Current state of DFA</param>
        /// <param name="nextSymbol">Next symbol in the stream</param>
        /// <returns>NO_TRANSTION or integer representation of the next state</returns>
        protected int GetNextState(int currState, int nextSymbol) {
            return nextSymbol == -1
                           ? NO_TRANSITION
                           : _avtomat[currState, nextSymbol];
        }

        /// <summary>
        ///     Determines whether the current state is a final one.
        /// </summary>
        /// <param name="currState">Current state</param>
        /// <returns>
        ///     <c>true</c> if current state is final; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsFinalState(int currState) {
            return _finalStates[currState] != 0;
        }

        /// <summary>
        ///     Returns the token type of the final state
        /// </summary>
        /// <param name="aState">Current state</param>
        /// <returns>Token type of the final state</returns>
        protected Tokens GetFinalState(int aState) {
            return (Tokens) _finalStates[aState];
        }
        #endregion

        #region String checks

        private static bool EscapedQuote(int trenStanje, int peek) {
            return !(trenStanje == STRING_END && (peek == ';' || peek == ':'));
        }

        /// <summary>
        ///     Determines whether the current symbol starts or ends a string
        /// </summary>
        /// <param name="currState">Current DFA state</param>
        /// <param name="nextSymbol">The next symbol in the stream</param>
        /// <returns>
        ///     <c>true</c> if the current symbol represents either start or end of a string, <c>false</c> otherwise.
        /// </returns>
        private static bool IsStringStartOrEnd(int currState, int nextSymbol) {
            return ((currState == START_STATE || currState == STRING_MID) && nextSymbol == '"');
        }

        #endregion

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() {
            if (_source != null) {
                _source.Dispose();
            }
        }
    }
}
