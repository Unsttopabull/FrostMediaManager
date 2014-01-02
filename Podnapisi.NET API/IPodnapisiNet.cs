using CookComputing.XmlRpc;
using Frost.PodnapisiNET.Models;

namespace Frost.PodnapisiNET {

    [XmlRpcUrl("http://ssp.podnapisi.net:8000/RPC2/")]
    public interface IPodnapisiNet : IXmlRpcProxy {

        [XmlRpcMethod("initiate")]
        LogInInfo Initiate(string useragent);

        [XmlRpcMethod("authenticate")]
        AuthenticationInfo Authenticate(string session, string username, string password);

        [XmlRpcMethod("search")]
        SearchResult Search(string session, string[] hashes);
    }
}
