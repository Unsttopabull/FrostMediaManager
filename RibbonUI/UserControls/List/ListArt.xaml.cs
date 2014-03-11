using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using GalaSoft.MvvmLight.Ioc;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditArt.xaml</summary>
    public partial class ListArt : UserControl {
        public static readonly DependencyProperty ArtProperty = DependencyProperty.Register("Art", typeof(ObservableCollection<MovieArt>), typeof(ListArt), new PropertyMetadata(default(ObservableCollection<MovieArt>), ArtChanged));

        public ListArt() {
            InitializeComponent();
            DataContext = SimpleIoc.Default.GetInstance<ListArtViewModel>();
        }

        public ObservableCollection<MovieArt> Art {
            get { return (ObservableCollection<MovieArt>) GetValue(ArtProperty); }
            set { SetValue(ArtProperty, value); }
        }

        private static void ArtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListArtViewModel) ((ListArt) d).DataContext).Art = (ObservableCollection<MovieArt>) e.NewValue;
        }

    }
}
