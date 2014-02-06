using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Frost.Common.Models.DB.MovieVo;

namespace RibbonUI.UserControls {

    /// <summary>Interaction logic for ContentGrid.xaml</summary>
    public partial class ContentGrid : UserControl {
        private string _filter;

        public static readonly DependencyProperty MinRequiredWidthProperty = DependencyProperty.Register(
            "MinRequiredWidth", typeof(double), typeof(ContentGrid), new FrameworkPropertyMetadata(default(double), FrameworkPropertyMetadataOptions.AffectsRender));

        private ICollectionView _collectionView;

        public double MinRequiredWidth {
            get { return (double) GetValue(MinRequiredWidthProperty); }
            set { SetValue(MinRequiredWidthProperty, value); }
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

            MinRequiredWidth = MovieList.RenderSize.Width + MovieFlags.RenderSize.Width;
        }

        private void MovieListOnLoaded(object sender, RoutedEventArgs e) {
            ICollectionView view = MovieList.ItemsSource as ICollectionView;
            if (view != null) {
                _collectionView = CollectionViewSource.GetDefaultView(view);
                _collectionView.Filter = Filter;
            }
        }
    }

}