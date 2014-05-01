using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Frost.Common.Models.Provider;
using RibbonUI.Annotations;
using RibbonUI.Util.WebUpdate;
using RibbonUI.Windows.WebUpdate;

namespace RibbonUI.Windows {

    /// <summary>Interaction logic for ExportMoviesAsNfo.xaml</summary>
    public partial class ExportMoviesAsNfo : Window, INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility _closeButtonVisibility;
        private CancellationTokenSource _tokenSource;
        private int _progressText;
        private string _labelText;

        public ExportMoviesAsNfo(ICollection<IMovie> movies) {
            Errors = new ObservableCollection<ErrorInfo>();

            InitializeComponent();

            ProgressBar.Maximum = movies.Count;
            SaveAsNfo(movies);
        }

        private void SaveAsNfo(IEnumerable<IMovie> movies) {
            _tokenSource = new CancellationTokenSource();

            Task.Run(() => {
                if (_tokenSource.Token.IsCancellationRequested) {
                    return;
                }

                //Parallel.ForEach(movies, m => {
                foreach (IMovie m in movies) {

                    if (m == null || _tokenSource.Token.IsCancellationRequested) {
                        Dispatcher.Invoke(() => NumberCompleted++);
                        return;
                    }

                    try {
                        m.SaveAsNfo();
                    }
                    catch (Exception e) {
                        IMovie mCopy = m;
                        Dispatcher.Invoke(() => Errors.Add(new ErrorInfo(ErrorType.Warning, mCopy.Title + "\t" + e.Message)));
                    }
                    finally {
                        Dispatcher.Invoke(() => NumberCompleted++);
                    }
                }
            }, _tokenSource.Token).ContinueWith(t => Dispatcher.Invoke(() => {
                if (Errors.Count == 0) {
                    Close();
                }
            }), _tokenSource.Token);
        }

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

        public int NumberCompleted {
            get { return _progressText; }
            set {
                if (value == _progressText) {
                    return;
                }
                _progressText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ErrorInfo> Errors { get; set; }

        public Visibility CloseButtonVisibility {
            get { return _closeButtonVisibility; }
            private set {
                if (value == _closeButtonVisibility) {
                    return;
                }
                _closeButtonVisibility = value;
                OnPropertyChanged();
            }
        }

        private void OnCloseButtonClicked(object sender, RoutedEventArgs e) {
            OnWindowClose(sender, e);
        }

        private void OnWindowClose(object sender, EventArgs e) {
            _tokenSource.Cancel(false);
            Close();            
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