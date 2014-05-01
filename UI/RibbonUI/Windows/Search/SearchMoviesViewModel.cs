using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.DetectFeatures;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Properties;
using RibbonUI.Util;

namespace RibbonUI.Windows.Search {

    public class SearchMoviesViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly string[] _filePaths;
        private FeatureDetector _detector;
        private string _logText;
        private string _progressText;
        private double _progressValue;
        private double _progressMax;
        private bool _isIndeterminate;
        private bool _allowClose;
        private ICommand _closeWindowCommand;
        private CancellationTokenSource _tokenSource;

        public SearchMoviesViewModel() {
            _progressMax = 100;
            _isIndeterminate = true;
            LogText = "Detecting movies...";

            _filePaths = Settings.Default.SearchFolders.Cast<string>().ToArray();
            if (_filePaths.Length == 0) {
                throw new ArgumentException("No search folders defined.");
            }
        }

        public void Search() {
            _detector = new FeatureDetector(_filePaths);
            _detector.PropertyChanged += DetectorPropertyChanged;

            _tokenSource = new CancellationTokenSource();
            Task.Run(() => _detector.Search(_tokenSource.Token))
                .ContinueWith(OnDetectFinished);
        }

        #region UI

        public Window ParentWindow { get; set; }

        public string LogText {
            get { return _logText; }
            private set {
                if (value == _logText) {
                    return;
                }
                _logText = value;
                OnPropertyChanged();
            }
        }

        public string ProgressText {
            get { return _progressText; }
            set {
                if (value == _progressText) {
                    return;
                }
                _progressText = value;
                OnPropertyChanged();
            }
        }

        public double ProgressValue {
            get { return _progressValue; }
            private set {
                if (value.Equals(_progressValue)) {
                    return;
                }
                _progressValue = value;
                OnPropertyChanged();
            }
        }

        public double ProgressMax {
            get { return _progressMax; }
            set {
                if (value.Equals(_progressMax)) {
                    return;
                }
                _progressMax = value;
                OnPropertyChanged();
            }
        }

        public bool IsIndeterminate {
            get { return _isIndeterminate; }
            private set {
                if (value.Equals(_isIndeterminate)) {
                    return;
                }
                _isIndeterminate = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region SeachSettings

        public bool SearchInfo { get; set; }
        public bool SearchArt { get; set; }
        public bool SearchVideos { get; set; }
        public Plugin InfoPlugin { get; set; }
        public Plugin ArtPlugin { get; set; }
        public Plugin VideoPlugin { get; set; }

        public ICommand CloseWindowCommand {
            get { return _closeWindowCommand ?? (_closeWindowCommand = new RelayCommand<CancelEventArgs>(OnWindowClose)); }
            set { _closeWindowCommand = value; }
        }

        #endregion

        private void OnWindowClose(CancelEventArgs arg) {
            _tokenSource.Cancel(false);
            arg.Cancel = !_allowClose;

            LogText = "Canceling seach & detect ...";
            ProgressText = "Canceling ...";
        }

        private void CloseParentWindow() {
            _allowClose = true;
            if (ParentWindow != null) {
                ParentWindow.Dispatcher.Invoke(() => ParentWindow.Close());
            }
        }

        private void DetectorPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName != "Count") {
                return;
            }

            FeatureDetector featureDetector = sender as FeatureDetector;
            if (featureDetector != null) {
                ProgressText = string.Format("Detected {0} movies.", featureDetector.Count);
            }
        }

        private async void OnDetectFinished(Task<IEnumerable<MovieInfo>> task) {
            if (task.Exception != null && task.Exception.InnerExceptions != null && task.Exception.InnerExceptions.Count > 0) {
                CloseParentWindow();
                return;
            }

            IsIndeterminate = false;
            _detector.PropertyChanged -= DetectorPropertyChanged;

            if (ParentWindow != null) {
                ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal);
            }

            if (task.IsCompleted) {
                LogText = "Saving movies...";
                IMoviesDataService service = LightInjectContainer.GetInstance<IMoviesDataService>();

                if (task.Result == null) {
                    CloseParentWindow();
                    return;
                }

                List<MovieInfo> movieInfos = task.Result.ToList();

                if (_tokenSource.Token.IsCancellationRequested) {
                    CloseParentWindow();
                    return;
                }
                
                await Task.Run(() => Save(movieInfos, service));

                if (_tokenSource.Token.IsCancellationRequested) {
                    CloseParentWindow();
                    return;
                }

                try {
                    service.SaveChanges();
                }
                catch (Exception e) {
                    UIHelper.HandleProviderException(e);
                }

                ProgressText = "Finished!";
            }
            else if (task.IsFaulted) {
                const string ERROR_MESSAGE = "Errors occured during detection phase. Search & Save can not continue.";
                if (ParentWindow != null) {
                    MessageBox.Show(ParentWindow, ERROR_MESSAGE);
                }
                else {
                    MessageBox.Show(ERROR_MESSAGE);
                }
                CloseParentWindow();
            }
            else if (task.IsCanceled) {
                CloseParentWindow();
            }
        }

        private void Save(IReadOnlyList<MovieInfo> movieInfos, IMoviesDataService service) {
            ProgressMax = movieInfos.Count;
            double percent = 1.0 / ProgressMax;

            for (int i = 0; i < movieInfos.Count; i++) {
                if (_tokenSource.Token.IsCancellationRequested) {
                    return;
                }

                MovieInfo movieInfo = movieInfos[i];
                ProgressText = movieInfo.Title ?? "Movie " + i;

                try {
                    service.SaveDetected(movieInfo);
                }
                catch (TaskCanceledException e) {
                }
                catch (Exception e) {
                }
                finally {
                    ProgressValue++;

                    if (ParentWindow != null) {
                        ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressValue += percent);
                    }                    
                }
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