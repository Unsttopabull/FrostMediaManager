using System;

namespace SharpTrailerAddictAPI {

    internal class TrailerAddictException : Exception {
        public TrailerAddictException(string message, Exception exception) : base(message, exception) {
        }
    }

}