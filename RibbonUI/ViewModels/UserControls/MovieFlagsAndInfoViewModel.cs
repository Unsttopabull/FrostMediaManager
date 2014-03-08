using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common.Annotations;
using Frost.Models.Frost.DB;
using Frost.XamlControls.Commands;
using RibbonUI.UserControls;

namespace RibbonUI.ViewModels.UserControls {

    public class MovieFlagsAndInfoViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private Movie _selectedMovie;

        public MovieFlagsAndInfoViewModel() {
            OnSizeChangedCommand =
                new RelayCommand<MovieFlagsAndInfo>(flags => { flags.MinRequiredWidth = flags.MovieFlags.RenderSize.Width + flags.MovieInfo.RenderSize.Width; });
        }

        public ICommand OnSizeChangedCommand { get; private set; }

        public Movie SelectedMovie {
            get { return _selectedMovie; }
            set {
                if (Equals(value, _selectedMovie)) {
                    return;
                }
                _selectedMovie = value;

                OnPropertyChanged("DurationFormatted");
                OnPropertyChanged("FirstStudioName");
                OnPropertyChanged();
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
                if (SelectedMovie == null) {
                    return null;
                }

                if (SelectedMovie.Studios.Count > 0) {
                    Studio studio = SelectedMovie.Studios.FirstOrDefault();
                    if (studio != null) {
                        return studio.Name;
                    }
                }
                return null;
            }
        }

        private long? GetVideoRuntimeSum() {
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