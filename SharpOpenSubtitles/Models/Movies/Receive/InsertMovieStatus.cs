﻿using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.Session.Receive;

namespace Frost.SharpOpenSubtitles.Models.Movies.Receive {
    public class InsertMovieStatus : SessionInfo {

        /// <summary>ID of the newly inserted movie in the database, you can use it later for uploading subtitles.</summary>
        [XmlRpcMember("id")]
        public string ID;
    }
}