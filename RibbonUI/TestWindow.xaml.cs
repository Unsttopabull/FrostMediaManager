using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using Frost.DetectFeatures;

namespace RibbonUI {
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public TextBoxDebugListener Listener { get; private set; }

        public TestWindow() {
            InitializeComponent();

            Listener = new TextBoxDebugListener(Debug);
        }

        private void Debug_OnTextChanged(object sender, TextChangedEventArgs e) {
            Scroll.ScrollToEnd();
        }

        private void LogWindow_Loaded(object sender, RoutedEventArgs e) {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);

            Task.Run(() => {
                FeatureDetector ms = new FeatureDetector(@"E:\Torrenti\FILMI", @"F:\Torrenti\FILMI");
                ms.Search();
            }).ContinueWith(tsk => Dispatcher.Invoke(Close));
        }
    }

    public class TextBoxDebugListener : TraceListener {
        private readonly TextBoxBase _tb;

        public TextBoxDebugListener(TextBoxBase tb) {
            _tb = tb;

        }

        /// <summary>
        /// When overridden in a derived class, writes the specified message to the listener you create in the derived class.
        /// </summary>
        /// <param name="message">A message to write. </param>
        public override void Write(string message) {
            _tb.Dispatcher.Invoke(() => _tb.AppendText(message));
        }

        /// <summary>
        /// When overridden in a derived class, writes a message to the listener you create in the derived class, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write. </param>
        public override void WriteLine(string message) {
            string ident = "";
            if (IndentLevel > 0) {
                ident = new string(' ', IndentLevel * IndentSize);
            }

            _tb.Dispatcher.Invoke(() => _tb.AppendText(ident + message + Environment.NewLine));
        }
    }
}
