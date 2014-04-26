using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Report.Receive;

namespace Frost.SharpOpenSubtitles {

    public class OpenSubtitlesClient {
        internal readonly IOpenSubtitles Proxy;
        public string Token;

        public OpenSubtitlesClient(bool renewUntilLogOut) {
            Proxy = XmlRpcProxyGen.Create<IOpenSubtitles>();

            Session = new Session(this, renewUntilLogOut);
            Movie = new Movie(this);
            Subtitle = new Subtitle(this);
            Program = new Program(this);
        }

        public Session Session { get; private set; }
        public Movie Movie { get; private set; }
        public Subtitle Subtitle { get; private set; }
        public Program Program { get; private set; }

        public ServerInfo ServerInfo() {
            return Proxy.ServerInfo();
        }
    }

}