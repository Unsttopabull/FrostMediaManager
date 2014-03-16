using System;

namespace Frost.SharpLanguageDetect {

    /**
    * @author Nakatani Shuyo
    */
    public enum ErrorCode {

        NoTextError,
        FormatError,
        FileLoadError,
        DuplicateLangError,
        NeedLoadProfileError,
        CantDetectError,
        CantOpenTrainData,
        TrainDataFormatError,
        InitParamError

    }

    /**
    * @author Nakatani Shuyo
    *
    */
    public class LangDetectException : Exception {

        private readonly ErrorCode _code;

        /**
        * @param code
        * @param message
        */
        public LangDetectException(ErrorCode code, string message) : base(message) {
            _code = code;
        }

        /**
        * @return the error code
        */
        public ErrorCode Code {
            get { return _code; }
        }

    }

}