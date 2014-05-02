using System;
using System.Linq;
using Newtonsoft.Json;

namespace SharpTraktTvAPI.Models.Genreal {

    public class Images {

        [JsonProperty("poster")]
        public string Poster { get; set; }

        [JsonIgnore]
        public string PosterSmall {
            get { return GetSpecificSize(Poster, 138); }
        }

        [JsonIgnore]
        public string PosterMedium {
            get { return GetSpecificSize(Poster, 300); }
        }

        [JsonProperty("fanart")]
        public string Fanart { get; set; }

        [JsonIgnore]
        public string FanartSmall {
            get { return GetSpecificSize(Fanart, 218); }
        }

        [JsonIgnore]
        public string FanartMedium {
            get { return GetSpecificSize(Fanart, 940); }
        }

        private string GetSpecificSize(string path, int size) {
            if (string.IsNullOrEmpty(path)) {
                return null;
            }

            int idxForward = path.LastIndexOf('/');
            string path2;
            if (idxForward > 0) {
                path2 = path.Substring(0, idxForward + 1);
            }
            else {
                return null;
            }


            string withoutExt = System.IO.Path.GetFileNameWithoutExtension(path);
            string ext = System.IO.Path.GetExtension(path);

            return string.Format("{0}{1}-{2}{3}", path2, withoutExt, size, ext);
        }
    }

}