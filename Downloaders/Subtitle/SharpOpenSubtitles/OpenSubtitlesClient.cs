using System;
using System.Security.Authentication;
using System.Threading;
using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Report;
using Frost.SharpOpenSubtitles.Models.Session;

namespace Frost.SharpOpenSubtitles {

    public class OpenSubtitlesClient {
        private const int SESSION_LENGTH = 900000; //15 min
        public const string DEBUG_UA = "OS Test User Agent";
        private readonly bool _renewUntilLogOut;
        internal readonly IOpenSubtitles Proxy;
        public string Token;
        private Timer _timer;

        public OpenSubtitlesClient(bool renewUntilLogOut) {
            _renewUntilLogOut = renewUntilLogOut;
            Proxy = XmlRpcProxyGen.Create<IOpenSubtitles>();

            Movie = new Movie(this);
            Subtitle = new Subtitle(this);
            Program = new Program(this);
        }

        public Movie Movie { get; private set; }
        public Subtitle Subtitle { get; private set; }
        public Program Program { get; private set; }

        #region Session Methods

        /// <summary>
        /// Login user <paramref name="userName"/> identified by password <paramref name="password"/> communicating in language <paramref name="language"/> and working in client application useragent <paramref name="userAgent"/>.<br />
        /// This function should be always called when starting communication with OSDb server to identify user, specify application and start a new session (either registered user or anonymous).<br />
        /// If user has no account, blank username and password should be used.
        /// </summary>
        /// <param name="userName">Nickname of user that's trying to login, can be left blank if logging in anonymously.</param>
        /// <param name="password">User's password to verify identity, can be left blank if logging in anonymously.</param>
        /// <param name="language">​ISO639 2-letter language code to specify the language all subsequent communication should use (mainly for error messages).</param>
        /// <param name="userAgent">Identifier of application/useragent that is trying to execute this operation, must be specified, empty parameter is not allowed.</param>
        /// <returns>A status of the request and a session token if successfull.</returns>
        public LogInInfo LogIn(string userName, string password, string language, string userAgent) {
            LogInInfo logIn = Proxy.LogIn(userName, password, language, userAgent);

            switch (logIn.Status.Substring(0, 3)) {
                case "200":
                    Token = logIn.Token;
                    if (_renewUntilLogOut) {
                        _timer = new Timer(Renew, new[] { userName, password, language, userAgent }, SESSION_LENGTH, SESSION_LENGTH);
                    }
                    break;
                case "206":
                    //PARTIAL CONTENT + MESSAGE
                    Console.Error.WriteLine(logIn.Status);
                    break;
                case "414":
                    try {
                        _timer.Dispose();
                        _timer = null;
                    }
                    catch (Exception) {
                    }
                    throw new InvalidCredentialException(string.Format("User agent {0} not registered.", userAgent));
            }

            return logIn;
        }

        /// <summary>
        /// Login anonymously communicating in language <paramref name="language"/> and working in client application useragent <paramref name="userAgent"/>.<br />
        /// This function should be always called when starting communication with OSDb server to identify user, specify application and start a new session (either registered user or anonymous).<br />
        /// If user has no account, blank username and password should be used.
        /// </summary>
        /// <param name="language">​ISO639 2-letter language code to specify the language all subsequent communication should use (mainly for error messages).</param>
        /// <param name="userAgent">Identifier of application/useragent that is trying to execute this operation, must be specified, empty parameter is not allowed.</param>
        /// <returns>A status of the request and a session token if successfull.</returns>
        public LogInInfo LogInAnonymous(string language, string userAgent) {
            return LogIn("", "", language, userAgent);
        }


        /// <summary>This will logout user identified by token token. This function should be called just before exiting/closing client application.</summary>
        /// <returns>TBD</returns>
        public SessionInfo LogOut() {
            if (_timer != null) {
                _timer.Dispose();
                _timer = null;
            }
            return Proxy.LogOut(Token);
        }

        /// <summary>
        /// This function is used to keep the session token alive while client application is idling.<br />
        /// Should be called every 15 minutes between XML-RPC requests (in case user is idle or client application is not currently communicating with OSDb server) to keep the connection alive while client application is still running.<br />
        /// It can be also used to check if given token is still active.</summary>
        /// <returns>TBD</returns>
        public SessionInfo NoOperation() {
            return Proxy.NoOperation(Token);
        }

        /// <summary>Renews the validity of the session token. If the session has expired it re-logins the user.</summary>
        /// <param name="state">The user credentials to be used in an event of a re-login.</param>
        private void Renew(object state) {
            string[] info = (string[]) state;

            SessionInfo session = NoOperation();
            if (session.Status != "200 OK") {
                LogInInfo logIn = LogIn(info[0], info[1], info[2], info[3]);
                Token = logIn.Token;
            }
        }

        #endregion

        public ServerInfo ServerInfo() {
            return Proxy.ServerInfo();
        }
    }

}