using GalaSoft.MvvmLight.Ioc;

namespace RibbonUI {

    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow {
        

        public MainWindow() {
            InitializeComponent();

            DataContext = SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        }
    }
}