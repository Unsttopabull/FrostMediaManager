using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using Frost.Common;
using Frost.Common.Models.FeatureDetector;
using Frost.DetectFeatures;
using RibbonUI.Annotations;
using RibbonUI.Properties;
using RibbonUI.Util;

namespace RibbonUI.Windows {

    public class SearchMoviesViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _logText;
        private readonly FeatureDetector _detector;
        private string _progressText;
        private double _progressValue;
        private double _progressMax;
        private bool _isIndeterminate;

        public SearchMoviesViewModel() {
            _progressMax = 100;
            _isIndeterminate = true;
            LogText = "Detecting movies...";

            string[] filePaths = Settings.Default.SearchFolders.Cast<string>().ToArray();
            if (filePaths.Length == 0) {
                throw new ArgumentException("No search folders defined.");
            }

            _detector = new FeatureDetector(filePaths);
            _detector.PropertyChanged += DetectorPropertyChanged;

            Task.Run(() => _detector.Search())
                .ContinueWith(OnDetectFinished);
        }

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

        private void DetectorPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName != "Count") {
                return;
            }

            FeatureDetector featureDetector = sender as FeatureDetector;
            if (featureDetector != null) {
                ProgressText = string.Format("Detected {0} movies.", featureDetector.Count);
            }
        }

        private async void OnDetectFinished(Task<IEnumerable<MovieInfo>> obj) {
            if (obj.Exception != null && obj.Exception.InnerExceptions != null && obj.Exception.InnerExceptions.Count > 0) {
                
            }

            IsIndeterminate = false;
            _detector.PropertyChanged -= DetectorPropertyChanged;

            if (ParentWindow != null) {
                ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal);
            }

            if (obj.IsCompleted) {
                LogText = "Saving movies...";
                IMoviesDataService service = LightInjectContainer.GetInstance<IMoviesDataService>();
                List<MovieInfo> movieInfos = obj.Result.ToList();
                
                await Task.Run(() => Save(movieInfos, service));

                service.SaveChanges();

                ProgressText = "Finished!";
            }
            else if (obj.IsFaulted) {
                const string ERROR_MESSAGE = "Errors occured during detection phase. Search & save can not continue.";
                if (ParentWindow != null) {
                    MessageBox.Show(ParentWindow, ERROR_MESSAGE);
                }
                else {
                    MessageBox.Show(ERROR_MESSAGE);
                }
            }
            else if (obj.IsCanceled) {
                
            }

            if (ParentWindow != null) {
                ParentWindow.Dispatcher.Invoke(() => ParentWindow.Close());
            }
        }

        private void Save(IReadOnlyList<MovieInfo> movieInfos, IMoviesDataService service) {
            ProgressMax = movieInfos.Count;
            double percent = 1.0 / ProgressMax;

            for (int i = 0; i < movieInfos.Count; i++) {
                MovieInfo movieInfo = movieInfos[i];
                ProgressText = movieInfo.Title ?? "Movie " + i;

                try {
                    service.SaveDetected(movieInfo);
                    ProgressValue++;

                    if (ParentWindow != null) {
                        ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressValue += percent);
                    }
                }
                catch (Exception e) {
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