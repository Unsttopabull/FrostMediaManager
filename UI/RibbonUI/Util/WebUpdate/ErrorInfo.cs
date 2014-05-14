using Frost.RibbonUI.Windows.WebUpdate;

namespace Frost.RibbonUI.Util.WebUpdate {

    public class ErrorInfo {
        public ErrorInfo(ErrorType type, string message) {
            Type = type;
            Message = message;
        }

        public ErrorType Type { get; private set; }
        public string Message { get; private set; }
    }

}