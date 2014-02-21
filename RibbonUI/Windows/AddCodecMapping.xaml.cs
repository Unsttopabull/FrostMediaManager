using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Win32;
using RibbonUI.Util;
using RibbonUI.Windows.ViewModels;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for AddCodecMapping.xaml</summary>
    public partial class AddCodecMapping : Window {
        private ICollectionView _collectionView;

        public AddCodecMapping(bool isVideo) {
            InitializeComponent();

            AddCodecMappingViewModel dc = new AddCodecMappingViewModel(isVideo);
            DataContext = dc;

            Observable.FromEventPattern<TextChangedEventArgs>(SearchBox, "TextChanged")
                      .Throttle(TimeSpan.FromSeconds(0.5))
                      .ObserveOn(SynchronizationContext.Current)
                      .Subscribe(args => _collectionView.Refresh());


            NewCodecName.TextChanged += dc.CheckCodecExists2;
            //Observable.FromEventPattern<TextChangedEventArgs>(NewCodecName, "TextChanged")
            //          .Throttle(TimeSpan.FromSeconds(0.5))
            //          .ObserveOn(SynchronizationContext.Current)
            //          .Subscribe(dc.CheckCodecExists);
        }

        private void AddOnClick(object sender, RoutedEventArgs e) {
            
        }

        private void CancelOnClick(object sender, RoutedEventArgs e) {
            
        }

        private void LogoSearchOnClick(object sender, RoutedEventArgs e) {
            OpenFileDialog ofd = new OpenFileDialog { CheckFileExists = true, Multiselect = false };

            if (ofd.ShowDialog() == true) {
                LogoBox.Text = ofd.FileName;
            }              
        }

        private void CodecsList_OnLoaded(object sender, RoutedEventArgs e) {
            if (CodecsList.ItemsSource == null) {
                return;
            }

            _collectionView = CollectionViewSource.GetDefaultView(CodecsList.ItemsSource);
            _collectionView.Filter = Filter;
        }

        private bool Filter(object obj) {
            return ((KnownCodec) obj).CodecId.IndexOf(SearchBox.Text, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }
    }
}
