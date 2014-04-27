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

        public void AddParameter<T>(string paramName, T value, bool urlEncode = false) {
            if (_first) {
                _uri.Append("?");
                _first = false;
            }
            else {
                _uri.Append("&");
            }

            _uri.Append(paramName);
            _uri.Append("=");

            if (urlEncode) {
                _uri.Append(WebUtility.UrlEncode(value.ToString()));
            }
            else {
                _uri.Append(value);
            }
        }

        public void AddSegment(string segmentName, bool trailingSlash = true) {
            _uri.Append(segmentName);
            if (trailingSlash) {
                _uri.Append("/");
            }
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
                    string response = wc.UploadString(_uri.ToString(), data);

                    string  contentEncoding = wc.ResponseHeaders["Content-Encoding"];
                    if (contentEncoding != null && contentEncoding == "gzip") {
                        using (StreamReader sr = new StreamReader(new GZipStream(new MemoryStream(Encoding.UTF8.GetBytes(response)), CompressionMode.Decompress))) {
                            response = sr.ReadToEnd();
                        }
                    }
                    return response;
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
                    byte[] response = wc.DownloadData(_uri.ToString());
                    string json;

                    string  contentEncoding = wc.ResponseHeaders["Content-Encoding"];
                    if (contentEncoding != null && contentEncoding == "gzip") {
                        using (StreamReader sr = new StreamReader(new GZipStream(new MemoryStream(response), CompressionMode.Decompress))) {
                            json = sr.ReadToEnd();
                        }
                    }
                    else {
                        json = Encoding.UTF8.GetString(response);
                    }
                    return json;
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

        private JContainer ThrowOnFailure(string json) {

            if (json.StartsWith("[")) {
                try {
                    return JArray.Parse(json);
                }
                catch (Exception e) {
                    throw new TraktTvException("Failed to parse the response as JSON.");
                }
            }

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