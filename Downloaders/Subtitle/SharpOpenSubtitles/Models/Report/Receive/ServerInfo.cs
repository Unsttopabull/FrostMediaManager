using CookComputing.XmlRpc;

namespace Frost.SharpOpenSubtitles.Models.Report {

    public class ServerInfo {

        /// <summary>Version of server's XML-RPC API implementation.</summary>
        [XmlRpcMember("xmlrpc_version")]
        public string XmlrpcVersion;

        /// <summary>XML-RPC interface URL.</summary>
        [XmlRpcMember("xmlrpc_url")]
        public string XmlrpcUrl;

        /// <summary>Server's application name and version.</summary>
        [XmlRpcMember("application")]
        public string Application;

        /// <summary>Contact e-mail address for server related quuestions and problems.</summary>
        [XmlRpcMember("contact")]
        public string Contact;

        /// <summary>Main server URL.</summary>
        [XmlRpcMember("website_url")]
        public string WebsiteUrl;

        /// <summary>Number of users currently online.</summary>
        [XmlRpcMember("users_online_total")]
        public int UsersOnlineTotal;

        /// <summary>Number of users currently online using a client application (XML-RPC API).</summary>
        [XmlRpcMember("users_online_program")]
        public int UsersOnlineProgram;

        /// <summary>Number of currently logged-in users.</summary>
        [XmlRpcMember("users_loggedin")]
        public int UsersLoggedIn;

        /// <summary>Maximum number of users throughout the history.</summary>
        [XmlRpcMember("users_max_alltime")]
        public string UsersMaxAlltime;

        /// <summary>Number of registered users.</summary>
        [XmlRpcMember("users_registered")]
        public string UsersRegistered;

        /// <summary>Total number of subtitle downloads.</summary>
        [XmlRpcMember("subs_downloads")]
        public string SubsDownloads;

        /// <summary>Total number of subtitle files stored on the server.</summary>
        [XmlRpcMember("subs_subtitle_files")]
        public string SubsSubtitleFiles;

        /// <summary>Total number of movies in the database.</summary>
        [XmlRpcMember("movies_total")]
        public string MoviesTotal;

        /// <summary>Total number of movie A.K.A. titles in the database.</summary>
        [XmlRpcMember("movies_aka")]
        public string MoviesAka;

        /// <summary>Total number of subtitle languages supported.</summary>
        [XmlRpcMember("total_subtitles_languages")]
        public string TotalSubtitlesLanguages;

        /// <summary>Structure containing information about last updates of translations.</summary>
        [XmlRpcMember("last_update_strings")]
        public LastUpdates LastUpdateStrings;

        /// <summary></summary>
        [XmlRpcMember("download_limits")]
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public DownloadLimits DownloadLimits;

        /// <summary>Time taken to execute this command on server.</summary>
        [XmlRpcMember("seconds")]
        public double Seconds;
    }

}