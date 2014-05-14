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
using Frost.Common.Models;
using Frost.Common.Models.FeatureDetector;
using Frost.DetectFeatures;
using Frost.GettextMarkupExtension;
using Frost.InfoParsers.Models;
using Frost.InfoParsers.Models.Art;
using Frost.InfoParsers.Models.Info;
using Frost.RibbonUI.Properties;
using Frost.RibbonUI.Util;
using Frost.RibbonUI.Util.WebUpdate;
using Frost.XamlControls.Commands;
using log4net;

namespace Frost.RibbonUI.Windows.Search {

    public class SearchMoviesViewModel : INotifyPropertyChanged {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SearchMoviesViewModel));
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
                    UIHelper.HandleProviderException(Log, e);
                }

                ProgressText = "Finished!";

                CloseParentWindow();
            }
            else if (task.IsFaulted) {
                if (Log.IsErrorEnabled) {
                    if (task.Exception != null && task.Exception.InnerExceptions != null) {
                        int count = task.Exception.InnerExceptions.Count;
                        for (int i = 0; i < count; i++) {
                            Exception exception = task.Exception.InnerExceptions[i];
                            Log.Error(string.Format("Error while detecting movies ({0}/{1}).", i, count), exception);
                        }
                    }
                }

                string errorMessage = Gettext.T("Errors occured during detection phase. Search & Save can not continue.");
                if (ParentWindow != null) {
                    MessageBox.Show(ParentWindow, errorMessage);
                }
                else {
                    MessageBox.Show(errorMessage);
                }
                CloseParentWindow();
            }
            else if (task.IsCanceled) {
                CloseParentWindow();
            }
        }

        private Task Save(IReadOnlyList<MovieInfo> movieInfos, IMoviesDataService service) {
            ProgressMax = movieInfos.Count;
            double percent = 1.0 / ProgressMax;

            LogText = "Searching for movie info, art and videos online ...";

            //for (int i = 0; i < movieInfos.Count; i++) {
            Parallel.For(0, movieInfos.Count, new ParallelOptions { MaxDegreeOfParallelism = 5 }, i => {

                if (_tokenSource.Token.IsCancellationRequested) {
                    return;
                }

                MovieInfo movieInfo = movieInfos[i];
                ProgressText = movieInfo.Title ?? "Movie " + i;

                Task[] downloaders = new Task[3];
                if (SearchInfo) {
                    downloaders[0] = SearchMovieInfo(movieInfo, InfoPlugin);
                }

                if (SearchArt) {
                    downloaders[1] = SearchMovieArt(movieInfo, ArtPlugin);
                }

                if (SearchVideos) {
                    downloaders[2] = SearchMovieVideos(movieInfo, VideoPlugin);
                }

                try {
                    Task.WaitAll(downloaders.Where(t => t != null).ToArray());
                }
                catch (Exception e) {
                    if (Log.IsErrorEnabled) {
                        if (e is AggregateException ) {
                            AggregateException ae = e as AggregateException;
                            if (ae.InnerExceptions != null) {
                                int count = ae.InnerExceptions.Count;
                                for (int j = 0; j < count; j++) {
                                    Exception ex = ae.InnerExceptions[i];
                                    Log.Error(string.Format("Error occured wile downloading art/info/videos ({0}/{1}).", i, count), ex);
                                }
                            }
                        }
                        else {
                            Log.Error("Error occured wile downloading art/info/videos.", e);
                        }
                    }
                }

                ProgressValue++;

                if (ParentWindow != null) {
                    ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressValue += percent);
                }
            });

            if (_tokenSource.Token.IsCancellationRequested) {
                return Task.FromResult<object>(null);
            }

            ProgressValue = 0;
            if (ParentWindow != null) {
                ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressValue = 0);
            }

            LogText = "Provider is saving movies ...";
            for (int i = 0; i < movieInfos.Count; i++) {
                if (_tokenSource.Token.IsCancellationRequested) {
                    return Task.FromResult<object>(null);
                }

                MovieInfo movieInfo = movieInfos[i];
                ProgressText = movieInfo.Title ?? "Movie " + i;

                try {
                    service.SaveDetected(movieInfo);
                }
                catch (Exception e) {
                    if (Log.IsErrorEnabled) {
                        Log.Error(string.Format("Provider \"{0}\" thew an exception while saving detected movie \"{1}\"", service, movieInfo.Title), e);
                    }
                }
                finally {
                    ProgressValue++;

                    if (ParentWindow != null) {
                        ParentWindow.Dispatcher.Invoke(() => ParentWindow.TaskbarItemInfo.ProgressValue += percent);
                    }
                }
            }

            return Task.FromResult<object>(null);
        }

        private async Task SearchMovieInfo(IMovieInfo movieInfo, Plugin infoPlugin) {
            if (infoPlugin != null) {
                IParsingClient client = LightInjectContainer.TryGetInstance<IParsingClient>(infoPlugin.Name);
                if (client == null) {
                    if (Log.IsWarnEnabled) {
                        Log.Warn(string.Format("Failed to instantie an IParsingClient using service name \"{0}\".", infoPlugin.Name));
                    }
                    return;
                }

                MovieInfoUpdater mi = new MovieInfoUpdater(client, movieInfo);
                await mi.Update(true);
                await mi.UpdateMovieInfo();
            }
        }

        private async Task SearchMovieVideos(IMovieInfo movieInfo, Plugin videoPlugin) {
            if (videoPlugin != null) {
                IPromotionalVideoClient client = LightInjectContainer.TryGetInstance<IPromotionalVideoClient>(videoPlugin.Name);
                if (client == null) {
                    if (Log.IsWarnEnabled) {
                        Log.Warn(string.Format("Failed to instantie an IPromotionalVideoClient using service name \"{0}\".", videoPlugin.Name));
                    }
                    return;
                }

                PromoVideoUpdater mi = new PromoVideoUpdater(client, movieInfo);
                await mi.Update(true);
            }
        }

        private async Task SearchMovieArt(IMovieInfo movieInfo, Plugin artPlugin) {
            if (artPlugin != null) {
                IFanartClient client = LightInjectContainer.TryGetInstance<IFanartClient>(artPlugin.Name);
                if (client == null) {
                    if (Log.IsWarnEnabled) {
                        Log.Warn(string.Format("Failed to instantie an IFanartClient using service name \"{0}\".", artPlugin.Name));
                    }
                    return;
                }

                ArtUpdater mi = new ArtUpdater(client, movieInfo);
                await mi.Update(true);
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