using System;
using System.IO;
using System.Reflection;
using Frost.InfoParsers.Models;

namespace SharpTrailerAddictAPI {
    public class TrailerAddictClient : IPromotionalVideoClient {
        public const string CLIENT_NAME = "TrailerAddict";

        public TrailerAddictClient() {
            Name = CLIENT_NAME;

            string directoryName;
            try {
                 directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
            catch {
                return;
            }

            if (directoryName != null) {
                Icon = new Uri(directoryName+"/traileraddict.png");
            }
        }

        public bool IsImdbSupported { get; private set; }
        public bool IsTmdbSupported { get; private set; }
        public bool IsTitleSupported { get; private set; }
        public string Name { get; private set; }
        public Uri Icon { get; private set; }
    }
}
