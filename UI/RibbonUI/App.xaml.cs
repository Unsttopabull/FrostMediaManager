﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Frost.GettextMarkupExtension;
using Frost.RibbonUI.Util;
using log4net;
using log4net.Config;

namespace Frost.RibbonUI {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {
        private static readonly ILog Log = LogManager.GetLogger(typeof(App));
        internal static List<Provider> Systems { get; private set; }

        static App() {
            Systems = new List<Provider>();
        }

        public App() {
            if (File.Exists("log4Net.config")) {
                XmlConfigurator.Configure(new FileInfo("log4Net.config"));
            }
            else {
                BasicConfigurator.Configure();
            }

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            LoadPlugins();

            Gettext.CurrentTranslationProvider = new SecondLanguageTranslationProvider("Languages");
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
                    MessageBox.Show(Gettext.T("Could not access providers folder. Program will now exit"));
                    IsShutdown = true;
                    Shutdown();
                    return;
                }

                int numFailed = 0;
                foreach (string plugin in plugins) {
                    Assembly assembly;
                    Provider provider;
                    if (!AssemblyEx.CheckIsProvider(plugin, out assembly, out provider)) {
                        numFailed++;
                        continue;
                    }

                    provider.AssemblyPath = plugin;
                    Systems.Add(provider);
                }

                if (numFailed == plugins.Length) {
                    MessageBox.Show(Gettext.T("Couldn't load any providers in the plugin folder. Program will now exit."), Gettext.T("Error loading providers"), MessageBoxButton.OK);
                    IsShutdown = true;
                    Shutdown();
                }
                return;
            }

            MessageBox.Show(Gettext.T("No providers for movie library manipulation found. Program will now exit."), Gettext.T("No providers found."));
            IsShutdown = true;
            Shutdown();
        }

        private void UnhandledExeption(object sender, DispatcherUnhandledExceptionEventArgs e) {
            if (e.Exception is DbException) {
                return;
            }

            if (Log.IsErrorEnabled && e.Exception != null) {
                Log.Error("There was an error working with the database.", e.Exception);
            }

            if (e.Exception != null) {
                MessageBox.Show(e.Exception.Message);
            }
            e.Handled = true;
        }
    }

}