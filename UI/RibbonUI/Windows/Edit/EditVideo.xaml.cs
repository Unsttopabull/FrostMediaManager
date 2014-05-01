using System.Windows;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.Windows.Edit {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditVideo : Window {
        public static readonly DependencyProperty VideoProperty = DependencyProperty.Register("Video", typeof(MovieVideo), typeof(EditVideo), new PropertyMetadata(default(MovieVideo), SelectedVideoChanged));

        public EditVideo() {
            InitializeComponent();
        }

        public MovieVideo Video {
            get { return (MovieVideo) GetValue(VideoProperty); }
            set { SetValue(VideoProperty, value); }
        }

        private static void SelectedVideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditVideoViewModel) ((EditVideo) d).DataContext).SelectedVideo = (MovieVideo) e.NewValue;
        }
    }
}
