using System;

namespace Frost.SharpTrailerAddictAPI {

    internal class TrailerAddictException : Exception {
        public TrailerAddictException(string message, Exception exception) : base(message, exception) {
        }
    }

}