using System;
using System.Windows;
using Frost.RibbonUI.Util;

namespace Frost.RibbonUI.Windows.Add {

    /// <summary>Interaction logic for AddCodecMapping.xaml</summary>
    public partial class AddCodecMapping : Window {

        public AddCodecMapping(bool isVideo) {
            InitializeComponent();

            DataContext = new AddCodecMappingViewModel(isVideo);
        }

        public KnownCodec AddedCodec {
            get {
                AddCodecMappingViewModel vm = DataContext as AddCodecMappingViewModel;
                return vm != null && vm.SelectedCodec != null
                    ? vm.SelectedCodec
                    : null;
            }
        }

        public bool IsNew {
            get {
                AddCodecMappingViewModel vm = DataContext as AddCodecMappingViewModel;
                return vm != null && vm.IsNew;
            }
        }

        private void AddCodecMappingOnClosed(object sender, EventArgs e) {
            AddCodecMappingViewModel dc = DataContext as AddCodecMappingViewModel;
            if (dc != null) {
                dc.Dispose();
            }
        }
    }
}
