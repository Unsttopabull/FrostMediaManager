using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common.Models;
using Frost.Common.Models.Provider;
using Frost.XamlControls.Commands;
using RibbonUI.Annotations;
using RibbonUI.Util.ObservableWrappers;

namespace RibbonUI.UserControls {

    public class MovieFlagsAndInfoViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableMovie _selectedMovie;

        public MovieFlagsAndInfoViewModel() {
            OnSizeChangedCommand = new RelayCommand<MovieFlagsAndInfo>(
                flags => { flags.MinRequiredWidth = flags.MovieFlags.RenderSize.Width + flags.MovieInfo.RenderSize.Width; }
            );
        }

        public ICommand OnSizeChangedCommand { get; private set; }

        public ObservableMovie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                OnPropertyChanged("DurationFormatted");
                OnPropertyChanged("FirstStudioName");
                OnPropertyChanged("FirstStudioLogo");
                OnPropertyChanged("OriginalTitle");
                OnPropertyChanged();
            }
        }

        public string OriginalTitle {
            get {
                if (SelectedMovie != null &&
                    SelectedMovie.OriginalTitle != null &&
                    !SelectedMovie.OriginalTitle.Equals(SelectedMovie.Title))
                {
                    return SelectedMovie.OriginalTitle;
                }
                return null;
            }
        }

        public string DurationFormatted {
            get {
                if (SelectedMovie == null) {
                    return null;
                }

                long? sum = SelectedMovie.Runtime ?? GetVideoRuntimeSum();

                return sum.HasValue && sum.Value > 0
                           ? TimeSpan.FromMilliseconds(Convert.ToDouble(sum)).ToString("h'h 'm'm'")
                           : null;
            }
        }

        public string FirstStudioName {
            get {
                if (SelectedMovie == null || SelectedMovie.Studios == null) {
                    return null;
                }

                try {
                    if (SelectedMovie.Studios.Any()) {
                        IStudio studio = SelectedMovie.Studios.FirstOrDefault();
                        if (studio != null) {
                            return studio.Name;
                        }
                    }
                }
                catch {
                    return null;
                }
                return null;
            }
        }

        public string FirstStudioLogo {
            get {
                if (string.IsNullOrEmpty(FirstStudioName)) {
                    return null;
                }

                var studio = Path.Combine(Directory.GetCurrentDirectory(), "Images/StudiosE/" + FirstStudioName + ".png");
                return studio;
            }
        }

        public IPlot FirstPlot {
            get {
                if (SelectedMovie == null || SelectedMovie.Plots == null) {
                    return null;
                }

                return SelectedMovie.Plots.Any()
                           ? SelectedMovie.Plots.FirstOrDefault()
                           : null;
            }
        }

        private long? GetVideoRuntimeSum() {
            if (SelectedMovie.Videos == null) {
                return null;
            }

            long l = SelectedMovie.Videos.Where(v => v.Duration.HasValue).Sum(v => v.Duration.Value);

            if (!SelectedMovie.Runtime.HasValue && l > 0) {
                SelectedMovie.Runtime = l;
            }

            return (l > 0)
                       ? (long?) l
                       : null;
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