using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using Frost.DetectFeatures;
using Frost.GettextMarkupExtension;
using Microsoft.WindowsAPICodePack.Dialogs;
using RibbonUI.Properties;
using ComboBox = System.Windows.Controls.ComboBox;
using MessageBox = System.Windows.MessageBox;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for Settings.xaml</summary>
    public partial class SettingsWindow : Window {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public SettingsWindow() {
            InitializeComponent();

            try {
                DialogResult = null;
                IsDialog = true;
            }
            catch (InvalidOperationException) {
                IsDialog = false;
            }
        }

        private bool IsDialog { get; set; }

        private void WindowLoaded(object sender, RoutedEventArgs e) {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }



        private void CloseClick(object sender, RoutedEventArgs e) {
            Settings.Default.Reload();
            App.LoadSettings();

            if (IsDialog) {
                DialogResult = false;
            }
            Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e) {
            Settings.Default.Save();

            if (IsDialog) {
                DialogResult = true;
            }
            Close();
        }

    }

}