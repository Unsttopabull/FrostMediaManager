using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for InputBox.xaml</summary>
    public partial class InputBox : Window {
        public static readonly DependencyProperty TextBoxTextProperty = DependencyProperty.Register("TextBoxText", typeof(string), typeof(InputBox), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register("LabelText", typeof(string), typeof(InputBox), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register("ButtonText", typeof(string), typeof(InputBox), new PropertyMetadata(default(string)));

        public InputBox() {
            InitializeComponent();
        }

        public string TextBoxText {
            get { return (string) GetValue(TextBoxTextProperty); }
            set { SetValue(TextBoxTextProperty, value); }
        }

        public string LabelText {
            get { return (string) GetValue(LabelTextProperty); }
            set { SetValue(LabelTextProperty, value); }
        }

        public string ButtonText {
            get { return (string) GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        private void ButtonClick(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void InputTextBoxOnKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                DialogResult = true;
                Close();
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e) {
            InputTextBox.Focus();
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Escape) {
                Close();
            }
        }

        private static string Show(InputBox ib) {
            if (ib.ShowDialog() == true) {
                return ib.TextBoxText;
            }
            return null;
        }

        public static string Show(Window parent, string labelText) {
            return Show(new InputBox { Owner = parent, LabelText = labelText });
        }

        public static string Show(Window parent, string labelText, string caption) {
            return Show(new InputBox { Owner = parent, LabelText = labelText, Title = caption });
        }

        public static string Show(Window parent, string labelText, string caption, string buttonText) {
            return Show(new InputBox { Owner = parent, Title = caption, LabelText = labelText, ButtonText = buttonText });
        }

        public static string Show(Window parent, string labelText, string caption, string buttonText, string inputBoxText) {
            return Show(new InputBox { Owner = parent, Title = caption, LabelText = labelText, ButtonText = buttonText, TextBoxText = inputBoxText });
        }
    }

}