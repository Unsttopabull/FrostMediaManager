using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using RibbonUI.Annotations;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for Loading.xaml</summary>
    public partial class Loading : Window, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _labelText;
        private double _progressValue;
        private DispatcherTimer _timer;

        public Loading(int maxProgress) {
            InitializeComponent();
            ProgressMax = maxProgress;

            _timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        public double ProgressMax { get; set; }

        public string LabelText {
            get { return _labelText; }
            set {
                if (value == _labelText) {
                    return;
                }

                _labelText = value;
                OnPropertyChanged();
            }
        }

        public double ProgressValue {
            get { return _progressValue; }
            set {
                if (value.Equals(_progressValue)) {
                    return;
                }
                _progressValue = value;
                OnPropertyChanged();
            }
        }

        private void TimerTick(object sender, EventArgs e) {
            if (ProgressBar.Value < ProgressMax) {
                ProgressValue += 5;
            }
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