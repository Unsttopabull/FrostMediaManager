using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media.Imaging;
using Frost.Common.Models.FeatureDetector;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {
        private const string PACK_URI = "pack://application:,,,/RibbonUI;component/{0}";

        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(Ribbon),
            new PropertyMetadata(default(ObservableMovie), OnMovieChanged));

        public static readonly DependencyProperty SelectedTabProperty = DependencyProperty.Register("SelectedTab", typeof(RibbonTabs), typeof(Ribbon),
            new PropertyMetadata(default(RibbonTabs), SelectedTabChanged));

        public Ribbon() {
            InitializeComponent();

            InitDownloaders();
        }

        public ObservableMovie Movie {
            get { return (ObservableMovie) GetValue(MovieProperty); }
            set { SetValue(MovieProperty, value); }
        }

        public RibbonTabs SelectedTab {
            get { return (RibbonTabs) GetValue(SelectedTabProperty); }
            set { SetValue(SelectedTabProperty, value); }
        }

        private static void SelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((RibbonViewModel) ((Ribbon) d).DataContext).OnRibbonTabSelect(e.NewValue is RibbonTabs ? (RibbonTabs) e.NewValue : RibbonTabs.None);
        }

        private static void OnMovieChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((RibbonViewModel) ((Ribbon) d).DataContext).SelectedMovie = (ObservableMovie) e.NewValue;
        }

        private void InitDownloaders() {
            if (!Directory.Exists("Downloaders")) {
                return;
            }

            string[] plugins;
            try {
                plugins = Directory.GetFiles("Downloaders", "*.dll");
            }
            catch {
                return;
            }

            foreach (string plugin in plugins) {
                Assembly assembly;
                if (AssemblyEx.CheckIsPlugin(plugin, out assembly)) {
                    try {
                        LightInjectContainer.RegisterAssembly(assembly);
                    }
                    catch(Exception e) {
                        continue;
                    }
                }
            }

            foreach (IParsingClient cli in LightInjectContainer.GetAllInstances<IParsingClient>()) {
                RibbonMenuItem item = GetRibbonMenuItem(cli);
                if (item != null) {
                    MovieInfoDownloaders.Items.Add(item);
                }
            }

            foreach (IFanartClient cli in LightInjectContainer.GetAllInstances<IFanartClient>()) {
                RibbonMenuItem item = GetRibbonMenuItem(cli);
                if (item != null) {
                    MovieArtDownloaders.Items.Add(item);
                }
            }

            foreach (IPromotionalVideoClient cli in LightInjectContainer.GetAllInstances<IPromotionalVideoClient>()) {
                RibbonMenuItem item = GetRibbonMenuItem(cli);
                if (item != null) {
                    MovieVideoDownloaders.Items.Add(item);
                }
            }

        }

        private RibbonMenuItem GetRibbonMenuItem(IInfoClient cli) {
            if (cli == null) {
                return null;
            }

            RibbonMenuItem item = new RibbonMenuItem();
            item.BeginInit();

            item.Header = cli.Name;
            item.Command = ((RibbonViewModel) DataContext).UpdateMovieCommand;
            item.CommandParameter = cli.Name;

            if (cli.Icon == null) {
                item.EndInit();
                return item;
            }

            try {
                BitmapImage imageSource = new BitmapImage(cli.Icon);

                item.ImageSource = imageSource;
            }
            catch (Exception e) {
            }

            item.EndInit();
            return item;
        }

        public void AddMovieInfoDownloader(string name, string icon) {
            RibbonMenuItem rmi = new RibbonMenuItem {
                Header = name,
                ImageSource = new BitmapImage(new Uri(icon)),
                Command = ((RibbonViewModel) DataContext).UpdateMovieCommand,
                CommandParameter = name
            };

            MovieInfoDownloaders.Items.Add(rmi);
        }
    }

}