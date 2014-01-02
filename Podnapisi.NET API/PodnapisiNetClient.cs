using CookComputing.XmlRpc;

namespace Frost.PodnapisiNET {

    public class PodnapisiNetClient {
        internal readonly IPodnapisiNet Proxy;
        internal string Nonce;
        public string Token;

        public PodnapisiNetClient() {
            Proxy = XmlRpcProxyGen.Create<IPodnapisiNet>();

            Session = new Session(this);
        }

        public Session Session { get; private set; }


    }

}