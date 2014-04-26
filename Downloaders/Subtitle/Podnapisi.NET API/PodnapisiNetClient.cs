using System.Security.Authentication;
using CookComputing.XmlRpc;
using Frost.PodnapisiNET.Models;

namespace Frost.PodnapisiNET {

    public class PodnapisiNetClient {
        internal readonly IPodnapisiNet Proxy;
        internal string Nonce;
        public string Token;
        public const string DOWNLOAD_PREFIX = "http://www.podnapisi.net/static/podnapisi/";

        public PodnapisiNetClient() {
            Proxy = XmlRpcProxyGen.Create<IPodnapisiNet>();

            Session = new Session(this);
            Subtitles = new Subtitles(this);
        }

        public Session Session { get; private set; }

        public Subtitles Subtitles { get; private set; }

        /// <summary>Get a list of supported languages.</summary>
        /// <returns>TBD</returns>
        public SupportedLanguage[] SupportedLanguages() {
            CheckToken();

            SupportedLanguagesInfo langs = Proxy.SupportedLanguages(Token);
            return langs.GetSuppotedLanguages();
        }

        public bool CheckToken() {
            if (string.IsNullOrEmpty(Token)) {
                throw new AuthenticationException("Session has not been initiated and/or authenticated yet.");
            }
            return true;
        }
    }

}