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
using Frost.Common.Models;
using Frost.Common.Properties;
using Frost.GettextMarkupExtension;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Expression.Interactivity.Core;
using RibbonUI.ViewModels.UserControls;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ContentGrid.xaml</summary>
    public partial class ContentGrid : UserControl, INotifyPropertyChanged {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register("MinRequiredWidth", typeof(double), typeof(ContentGrid), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));
        public event PropertyChangedEventHandler PropertyChanged;

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }
        
        public IMovie SelectedMovie {
            get { return MovieList.SelectedItem as IMovie; }
        }

        public ContentGrid() {
            InitializeComponent();
            DataContext = SimpleIoc.Default.GetInstance<ContentGridViewModel>();

            MinRequiredWidth = MovieList.RenderSize.Width + MovieFlags.RenderSize.Width;
        }

        private void OnWindowLoaded(object sender, EventArgs args) {
            ((ContentGridViewModel) DataContext).ParentWindow = Window.GetWindow(this);

            SetTaskBarInfo();
        }

        private void SetTaskBarInfo() {
            Window window = Window.GetWindow(this);

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
                        Description = TranslationManager.T("Go to next movie"),
                        Command = new ActionCommand(() => MovieList.SelectedIndex++),
                    },
                    new ThumbButtonInfo {
                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/RibbonUI;component/Images/go-previous.png")),
                        Description = TranslationManager.T("Go to previous movie"),
                        Command = new ActionCommand(() => MovieList.SelectedIndex--),
                    }
                }
            };
            window.TaskbarItemInfo = taskbarItemInfo;
        }

        private void MovieListOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (EditMovie.MoviePlotCombo.HasItems) {
                EditMovie.MoviePlotCombo.SelectedIndex = 0;
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