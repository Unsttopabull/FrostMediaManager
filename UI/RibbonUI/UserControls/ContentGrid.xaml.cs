using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using Frost.GettextMarkupExtension;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ContentGrid.xaml</summary>
    public partial class ContentGrid : UserControl, INotifyPropertyChanged {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register("MinRequiredWidth", typeof(double), typeof(ContentGrid), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        public event PropertyChangedEventHandler PropertyChanged;

        public ContentGrid() {
            InitializeComponent();

            MinRequiredWidth = MovieList.RenderSize.Width + MovieFlags.RenderSize.Width;
        }

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }
        
        public ObservableMovie SelectedMovie {
            get { return MovieList.SelectedItem as ObservableMovie; }
        }

        private void OnWindowLoaded(object sender, EventArgs args) {
            ((ContentGridViewModel) DataContext).ParentWindow = Window.GetWindow(this);

            SetTaskBarInfo();
        }

        private void SetTaskBarInfo() {
            Window window = Window.GetWindow(this);

            if (window == null) {
                return;
            }

            BitmapImage overlay = new BitmapImage();
            using (Bitmap bm = new Bitmap("Images/overlay.png")) {
                using (Graphics g = Graphics.FromImage(bm)) {
                    g.DrawString(
                        MovieList.Items.Count.ToString(CultureInfo.InvariantCulture),
                        new Font("Arial", 16, System.Drawing.FontStyle.Bold),
                        new SolidBrush(Color.Red),
                        0,
                        7
                        );
                }

                overlay.BeginInit();
                MemoryStream ms = new MemoryStream();
                bm.Save(ms, ImageFormat.Png);

                ms.Seek(0, SeekOrigin.Begin);
                overlay.StreamSource = ms;
                overlay.EndInit();
            }

            TaskbarItemInfo taskbarItemInfo = new TaskbarItemInfo {
                Overlay = overlay,
                ThumbButtonInfos = new ThumbButtonInfoCollection {
                    new ThumbButtonInfo {
                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/RibbonUI;component/Images/go-next.png")),
                        Description = TranslationManager.T("Go to previous movie"),
                        Command = new RelayCommand(() =>  MovieList.SelectedIndex--, o => MovieList.SelectedIndex > 0),
                    },
                    new ThumbButtonInfo {
                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/RibbonUI;component/Images/go-previous.png")),
                        Description = TranslationManager.T("Go to next movie"),
                        Command = new RelayCommand(() => MovieList.SelectedIndex++, o => MovieList.SelectedIndex < MovieList.Items.Count - 1),
                    }
                }
            };

            window.TaskbarItemInfo = taskbarItemInfo;
        }

        private void MovieListOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (EditMovie != null && EditMovie.MoviePlotCombo.HasItems) {
                EditMovie.MoviePlotCombo.SelectedIndex = 0;
            }

            if (MovieList == null || MovieFlags == null) {
                return;
            }

            MinRequiredWidth = MovieList.RenderSize.Width + MovieFlags.MinRequiredWidth;
        }

        private void MovieListSelectedChanged(object sender, SelectedCellsChangedEventArgs e) {
            OnPropertyChanged("SelectedMovie");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}