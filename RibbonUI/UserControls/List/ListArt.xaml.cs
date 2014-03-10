using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Frost.Common.Models;
using GalaSoft.MvvmLight.Ioc;
using RibbonUI.ViewModels.UserControls.List;

namespace RibbonUI.UserControls.List {

    /// <summary>Interaction logic for EditArt.xaml</summary>
    public partial class ListArt : UserControl {
        public static readonly DependencyProperty ArtProperty = DependencyProperty.Register("Art", typeof(ObservableCollection<IArt>), typeof(ListArt), new PropertyMetadata(default(ObservableCollection<IArt>), ArtChanged));

        public ListArt() {
            InitializeComponent();
            DataContext = SimpleIoc.Default.GetInstance<ListArtViewModel>();
        }

        public ObservableCollection<IArt> Art {
            get { return (ObservableCollection<IArt>) GetValue(ArtProperty); }
            set { SetValue(ArtProperty, value); }
        }

        private static void ArtChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((ListArtViewModel) ((ListArt) d).DataContext).Art = (ObservableCollection<IArt>) e.NewValue;
        }

    }
}
