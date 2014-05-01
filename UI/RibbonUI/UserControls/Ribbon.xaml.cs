using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media.Imaging;
using Frost.Common.Models.FeatureDetector;
using Frost.InfoParsers;
using Frost.InfoParsers.Models;
using LightInject;
using RibbonUI.Util;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    internal enum DownloaderType {
        MovieInfo,
        Art,
        PromotionalVideo
    }

    /// <summary>Interaction logic for Ribbon.xaml</summary>
    public partial class Ribbon : UserControl {
        private const string PACK_URI = "pack://application:,,,/RibbonUI;component/{0}";

        public static readonly DependencyProperty MovieProperty = DependencyProperty.Register("Movie", typeof(ObservableMovie), typeof(Ribbon),
            new PropertyMetadata(default(ObservableMovie), OnMovieChanged));

        public static readonly DependencyProperty SelectedTabProperty = DependencyProperty.Register("SelectedTab", typeof(RibbonTabs), typeof(Ribbon),
            new PropertyMetadata(default(RibbonTabs), SelectedTabChanged));

        static Ribbon() {
            MovieInfoPlugins = new List<Plugin>();
            MovieArtPlugins = new List<Plugin>();
            MoviePromoVideoPlugins = new List<Plugin>();
        }

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

        public static List<Plugin> MovieInfoPlugins { get; private set; }

        public static List<Plugin> MovieArtPlugins { get; private set; }

        public static List<Plugin> MoviePromoVideoPlugins { get; private set; }

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
                    catch{
                    }
                }
            }

            LoadMovieInfoDownloaders();
            LoadMovieArtDownloaders();
            LoadPromotionalVideoDownloaders();
        }

        private void LoadMovieArtDownloaders() {
            Type fanartDownloaderClients = typeof(IFanartClient);
            IEnumerable<string> parsingClients = LightInjectContainer.AvailableServices
                                                                     .Where(s => s.ServiceType == fanartDownloaderClients)
                                                                     .Select(sr => sr.ServiceName);
            foreach (string clientName in parsingClients) {
                IFanartClient cli;
                try {
                    cli = LightInjectContainer.GetInstance<IFanartClient>(clientName);
                }
                catch (Exception e) {
                    MessageBox.Show(string.Format("Failed to load plugin {0}.\n\n{1}", clientName, e.Message));
                    continue;
                }

                RibbonMenuItem item = GetRibbonMenuItem(cli, DownloaderType.Art);
                if (item != null) {
                    MovieArtDownloaders.Items.Add(item);
                }
            }
        }

        private void LoadPromotionalVideoDownloaders() {
            Type promotionalVideoClients = typeof(IPromotionalVideoClient);
            IEnumerable<string> parsingClients = LightInjectContainer.AvailableServices
                                                                     .Where(s => s.ServiceType == promotionalVideoClients)
                                                                     .Select(sr => sr.ServiceName);
            foreach (string clientName in parsingClients) {
                IPromotionalVideoClient cli;
                try {
                    cli = LightInjectContainer.GetInstance<IPromotionalVideoClient>(clientName);
                }
                catch (Exception e) {
                    MessageBox.Show(string.Format("Failed to load plugin {0}.\n\n{1}", clientName, e.Message));
                    continue;
                }

                RibbonMenuItem item = GetRibbonMenuItem(cli, DownloaderType.PromotionalVideo);
                if (item != null) {
                    MovieVideoDownloaders.Items.Add(item);
                }
            }
        }

        private void LoadMovieInfoDownloaders() {
            Type parsingClientType = typeof(IParsingClient);
            IEnumerable<string> parsingClients = LightInjectContainer.AvailableServices
                                                                     .Where(s => s.ServiceType == parsingClientType)
                                                                     .Select(sr => sr.ServiceName);
            foreach (string clientName in parsingClients) {
                IParsingClient cli;
                try {
                    cli = LightInjectContainer.GetInstance<IParsingClient>(clientName);
                }
                catch (Exception e) {
                    MessageBox.Show(string.Format("Failed to load plugin {0}.\n\n{1}", clientName, e.Message));
                    continue;
                }

                RibbonMenuItem item = GetRibbonMenuItem(cli, DownloaderType.MovieInfo);
                if (item != null) {
                    MovieInfoDownloaders.Items.Add(item);
                }
            }
        }

        private RibbonMenuItem GetRibbonMenuItem(IInfoClient cli, DownloaderType type) {
            if (cli == null) {
                return null;
            }

            Plugin p = new Plugin(cli.Name);
            RibbonMenuItem item = new RibbonMenuItem();
            item.BeginInit();

            item.Header = cli.Name;

            if (cli.Icon != null) {
                try {
                    BitmapImage imageSource = new BitmapImage(cli.Icon);

                    item.ImageSource = imageSource;
                }
                catch (Exception e) {
                }
            }
            
            p.IconPath = cli.Icon;

            switch (type) {
                case DownloaderType.MovieInfo:
                    item.Command = ((RibbonViewModel) DataContext).UpdateMovieCommand;
                    MovieInfoPlugins.Add(p);
                    break;
                case DownloaderType.Art:
                    item.Command = ((RibbonViewModel) DataContext).UpdateMovieArtCommand;
                    MovieArtPlugins.Add(p);
                    break;
                case DownloaderType.PromotionalVideo:
                    item.Command = ((RibbonViewModel) DataContext).UpdatePromotionalVideosCommand;
                    MoviePromoVideoPlugins.Add(p);
                    break;
            }

            item.CommandParameter = cli.Name;
            item.EndInit();

            return item;
        }
    }

}