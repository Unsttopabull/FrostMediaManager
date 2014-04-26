using System;

namespace SharpTraktTvAPI {
    public class TraktTvException : Exception {

        public TraktTvException(string error) : base(error){
        }
    }
}
