using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SharpTraktTvAPI {

    public class URLBuilder {
        private readonly StringBuilder _uri;
        private bool _first = true;

        public URLBuilder(string baseUri) {
            _uri = new StringBuilder(baseUri);
        }

        public void AddParameter<T>(string paramName, T value) {
            if (_first) {
                _uri.Append("?");
                _first = false;
            }
            else {
                _uri.Append("&");
            }

            _uri.Append(paramName);
            _uri.Append("=");
            _uri.Append(value);
        }

        public void AddSegment(string segmentName) {
            _uri.Append(segmentName + "/");
        }

        public void AddSegmentPath(params string[] segments) {
            _uri.Append(string.Join("/", segments) + "/");
        }

        public string ToUri() {
            return _uri.ToString();
        }

        public T GetResposeAs<T>() {
            string response = GetRequest();
            if (string.IsNullOrEmpty(response)) {
                return default(T);
            }

            return ThrowOnFailure(response).ToObject<T>();
        }

        public T GetPostResponseAs<T>(string data) {
            string response = PostRequest(data);
            if (string.IsNullOrEmpty(response)) {
                return default(T);
            }

            return ThrowOnFailure(response).ToObject<T>();
        }

        public TResponse GetPostResponseAs<TResponse, TData>(TData data) {
            string response = PostRequest(data);
            if (string.IsNullOrEmpty(response)) {
                return default(TResponse);
            }

            return ThrowOnFailure(response).ToObject<TResponse>();
        }

        public string PostRequest<T>(T data) {
            return PostRequest(JsonConvert.SerializeObject(data));
        }

        public string PostRequest(string data) {
            using (WebClient wc = new WebClient()) {
                wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");
                try {
                    return wc.UploadString(_uri.ToString(), data);
                }
                catch (WebException e) {
                    return HandleWebException(e);
                }
                catch (Exception e) {
                    return null;
                }
            }
        }

        public string GetRequest() {
            using (WebClient wc = new WebClient()) {
                wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip");

                try {
                    return wc.DownloadString(_uri.ToString());
                }
                catch (WebException e) {
                    return HandleWebException(e);
                }
                catch (Exception e) {
                    return null;
                }
            }
        }

        private string HandleWebException(WebException e) {
            HttpWebResponse response = e.Response as HttpWebResponse;
            if (response != null && response.StatusCode == HttpStatusCode.NotFound && response.ContentLength > 0) {
                using (Stream stream = response.ContentEncoding == "gzip"
                                           ? new GZipStream(response.GetResponseStream(), CompressionMode.Decompress)
                                           : response.GetResponseStream()) {

                    if (stream != null) {
                        string json;
                        using (StreamReader sr = new StreamReader(stream)) {
                            json = sr.ReadToEnd();
                        }
                        ThrowOnFailure(json);
                    }
                }
            }

            return null;
        }

        private JObject ThrowOnFailure(string json) {
            JObject jObject;
            try {
                jObject = JObject.Parse(json);
            }
            catch (Exception e) {
                throw new TraktTvException("Failed to parse the response as JSON.");
            }

            if (jObject.Count == 2) {
                JProperty status = jObject.Property("status");
                if (status != null && status.ToObject<string>() == "failure") {
                    string error = jObject.Property("error").ToObject<string>();
                    throw new TraktTvException(error);
                }
            }
            return jObject;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() {
            return _uri.ToString();
        }
    }

}