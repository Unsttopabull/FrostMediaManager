using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Frost.Common.Models;
using Frost.GettextMarkupExtension;
using Frost.Models.Frost.DB.Files;
using RibbonUI.ViewModels.Windows;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for EditAudio.xaml</summary>
    public partial class EditVideo : Window {
        public static readonly DependencyProperty VideoProperty = DependencyProperty.Register("Video", typeof(IVideo), typeof(EditVideo), new PropertyMetadata(default(IVideo), SelectedVideoChanged));

        public EditVideo() {
            InitializeComponent();
        }

        public IVideo Video {
            get { return (IVideo) GetValue(VideoProperty); }
            set { SetValue(VideoProperty, value); }
        }

        private static void SelectedVideoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((EditVideoViewModel) ((EditVideo) d).DataContext).SelectedVideo = (IVideo) e.NewValue;
        }
    }
}
