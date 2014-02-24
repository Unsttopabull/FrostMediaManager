using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using Frost.Common.Annotations;
using Frost.Common.Models.DB.MovieVo;
using Frost.GettextMarkupExtension;
using Microsoft.Expression.Interactivity.Core;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ContentGrid.xaml</summary>
    public partial class ContentGrid : UserControl, INotifyPropertyChanged {
        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register(
            "MinRequiredWidth", typeof(double), typeof(ContentGrid), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        private ICollectionView _collectionView;
        public event PropertyChangedEventHandler PropertyChanged;

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
        }

        public Movie SelectedMovie {
            get { return MovieList.SelectedItem as Movie; }
        }

        public ContentGrid() {
            InitializeComponent();
            MinRequiredWidth = MovieList.RenderSize.Width + MovieFlags.RenderSize.Width;

            Observable.FromEventPattern<TextChangedEventArgs>(ListFilter, "TextChanged")
                      .Throttle(TimeSpan.FromSeconds(0.5))
                      .ObserveOn(SynchronizationContext.Current)
                      .Subscribe(args => _collectionView.Refresh());
        }

        private bool Filter(object o) {
            return ((Movie) o).Title.IndexOf(ListFilter.Text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }

        private void MovieSubtitlesGotFocus(object sender, RoutedEventArgs e) {
            Ribbon rb = ((MainWindow) ((Grid) Parent).Parent).Ribbon;
            rb.ContextSubtitle.Visibility = Visibility.Visible;
            rb.SubtitlesTab.IsSelected = true;
        }

        private void MovieSubtitlesOnLostFocus(object sender, RoutedEventArgs e) {
            Ribbon rb = ((MainWindow) ((Grid) Parent).Parent).Ribbon;
            rb.ContextSubtitle.Visibility = Visibility.Collapsed;
            rb.Search.IsSelected = true;
        }

        private void MovieListOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (EditMovie.MoviePlotCombo.HasItems) {
                EditMovie.MoviePlotCombo.SelectedIndex = 0;
            }

            MinRequiredWidth = MovieList.RenderSize.Width + MovieFlags.MinRequiredWidth;
        }

        private void MovieListOnLoaded(object sender, RoutedEventArgs e) {
            ICollectionView view = MovieList.ItemsSource as ICollectionView;
            if (view != null) {
                _collectionView = CollectionViewSource.GetDefaultView(view);
                _collectionView.SortDescriptions.Add(new SortDescription("SortTitle", ListSortDirection.Ascending));
                _collectionView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                _collectionView.Filter = Filter;
            }
            ChangeTaskBarItems();
        }

        private void MovieListSelectedChanged(object sender, SelectedCellsChangedEventArgs e) {
            OnPropertyChanged("SelectedMovie");
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ChangeTaskBarItems() {
            Window window = Window.GetWindow(this);

            BitmapImage overlay = new BitmapImage();
            using (Bitmap bm = new Bitmap("Images/overlay.png")) {
                using (Graphics g = Graphics.FromImage(bm)) {
                    g.DrawString(MovieList.Items.Count.ToString(CultureInfo.InvariantCulture), new Font("Arial", 16, System.Drawing.FontStyle.Bold),
                        new SolidBrush(Color.Red), 0, 7);
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
    }

}