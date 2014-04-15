using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Frost.GettextMarkupExtension;
using log4net.Config;
using RibbonUI.Util;

namespace RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        internal static List<Provider> Systems { get; private set; }

        static App() {
            Systems = new List<Provider>();
        }

        public App() {
            BasicConfigurator.Configure();

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoadPlugins();

            TranslationManager.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");
            DispatcherUnhandledException += UnhandledExeption;
        }


        public static bool IsShutdown { get; private set; }

        private void LoadPlugins() {
            if (Directory.Exists("providers")) {
                string[] plugins;
                try {
                    plugins = Directory.GetFiles("providers", "*.dll");
                }
                catch (Exception) {
                    MessageBox.Show("Could not access providers folder. Program will now exit");
                    IsShutdown = true;
                    Shutdown();
                    return;
                }

                int numFailed = 0;
                foreach (string plugin in plugins) {
                    Assembly assembly;
                    Provider provider;
                    if (!AssemblyEx.CheckIsPlugin(plugin, out assembly, out provider)) {
                        numFailed++;
                        continue;
                    }

                    provider.AssemblyPath = plugin;
                    Systems.Add(provider);
                }

                if (numFailed == plugins.Length) {
                    MessageBox.Show("Couldn't load any providers in the plugin folder. Program will now exit.", "Error loading providers", MessageBoxButton.OK);
                    IsShutdown = true;
                    Shutdown();
                }
                return;
            }

            MessageBox.Show("No providers for movie library manipulation found. Program will now exit.", "No providers found.");
            IsShutdown = true;
            Shutdown();
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }

}