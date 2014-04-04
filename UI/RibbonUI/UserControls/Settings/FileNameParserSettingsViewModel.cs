using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common.Properties;
using Frost.Common.Proxies.ChangeTrackers;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.XamlControls.Commands;

namespace RibbonUI.UserControls.Settings {

    internal class FileNameParserSettingsViewModel : INotifyPropertyChanged, IDataErrorInfo {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableChangeTrackingCollection<LanguageMapping> _languageMappings;
        private ObservableChangeTrackingCollection<SegmentMapping> _knownSegments;
        private ObservableChangeTrackingCollection<string> _excludedSegments;
        private ObservableChangeTrackingCollection<string> _releaseGroups;
        private string _releaseGroupError;

        public FileNameParserSettingsViewModel() {
            if (Directory.Exists("Images/Languages")) {
                Languages = Directory.EnumerateFiles("Images/Languages", "*.png")
                                     .Select(fileName => ISOLanguageCodes.Instance.GetByISOCode(Path.GetFileNameWithoutExtension(fileName)))
                                     .Where(isoCode => isoCode != null)
                                     .ToList();
            }

            RemoveLanguageMappingCommand = new RelayCommand<LanguageMapping>(
                mapping => CustomLanguageMappings.Remove(mapping),
                mapping => mapping != null
                );

            AddNewLanguageMappingCommand = new RelayCommand<object>(
                o => CustomLanguageMappings.Add(new LanguageMapping(Mapping, Language.Alpha3)),
                o => ValidateLanguageMapping()
                );

            AddNewExcludedSegmentCommand = new RelayCommand<object>(
                o => ExcludedSegments.Add(ExcludedSegment),
                o => ValidateExculdedSegment()
                );

            RemoveExcludedSegmentCommand = new RelayCommand<string>(
                segment => ExcludedSegments.Remove(segment),
                segment => !string.IsNullOrEmpty(segment)
                );

            RemoveReleaseGroupCommand = new RelayCommand<string>(
                releaseGrp => FileNameParser.ReleaseGroups.Remove(releaseGrp),
                releaseGrp => !string.IsNullOrEmpty(releaseGrp)
                );

            AddNewReleaseGroupCommand = new RelayCommand<object>(
                o => FileNameParser.ReleaseGroups.Add(ReleaseGroup),
                o => ValidateReleaseGroup()
                );

            AddNewKnownSegmentCommand = new RelayCommand<object>(
                o => KnownSegments.Add(new SegmentMapping(Segment, SegmentType)),
                o => ValidateKnownSegment()
                );

            RemoveKnownSegmentCommand = new RelayCommand<SegmentMapping>(
                mapping => KnownSegments.Remove(mapping),
                mapping => mapping != null
                );
        }

        public List<ISOLanguageCode> Languages { get; set; }

        public ObservableChangeTrackingCollection<LanguageMapping> CustomLanguageMappings {
            get {
                if (_languageMappings == null) {
                    _languageMappings = new ObservableChangeTrackingCollection<LanguageMapping>(FileNameParser.CustomLanguageMappings);
                }
                return _languageMappings;
            }
            set { _languageMappings = value; }
        }

        public ObservableChangeTrackingCollection<SegmentMapping> KnownSegments {
            get {
                if (_knownSegments == null) {
                    _knownSegments = new ObservableChangeTrackingCollection<SegmentMapping>(FileNameParser.KnownSegments);
                }
                return _knownSegments;
            }
            set { _knownSegments = value; }
        }

        public ObservableChangeTrackingCollection<string> ExcludedSegments {
            get {
                if (_excludedSegments == null) {
                    _excludedSegments = new ObservableChangeTrackingCollection<string>(FileNameParser.ExcludedSegments);
                }
                return _excludedSegments;
            }
            set { _excludedSegments = value; }
        }

        public ObservableChangeTrackingCollection<string> ReleaseGroups {
            get {
                if (_releaseGroups == null) {
                    _releaseGroups = new ObservableChangeTrackingCollection<string>(FileNameParser.ReleaseGroups);
                }
                return _releaseGroups;
            }
            set { _releaseGroups = value; }
        }

        #region Utility

        public string Segment { get; set; }
        public SegmentType SegmentType { get; set; }

        public string ExcludedSegment { get; set; }

        public string ReleaseGroup { get; set; }

        public string ReleaseGroupError {
            get { return _releaseGroupError; }
            set {
                if (value == _releaseGroupError) {
                    return;
                }
                _releaseGroupError = value;
                OnPropertyChanged();
            }
        }

        public string Mapping { get; set; }
        public ISOLanguageCode Language { get; set; }

        #endregion

        #region Commands

        public ICommand RemoveLanguageMappingCommand { get; private set; }

        public ICommand RemoveExcludedSegmentCommand { get; private set; }

        public ICommand RemoveReleaseGroupCommand { get; private set; }

        public ICommand RemoveKnownSegmentCommand { get; private set; }

        public ICommand AddNewLanguageMappingCommand { get; private set; }

        public ICommand AddNewReleaseGroupCommand { get; private set; }

        public ICommand AddNewExcludedSegmentCommand { get; private set; }

        public ICommand AddNewKnownSegmentCommand { get; private set; }

        #endregion

        #region IDataErrorInfo

        /// <summary>Gets the error message for the property with the given name.</summary>
        /// <returns>The error message for the property. The default is an empty string ("").</returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public string this[string columnName] {
            get {
                string str = null;
                switch (columnName) {
                    case "Segment":
                        ValidateKnownSegment(out str);
                        break;
                    case "ExcludedSegment":
                        ValidateExculdedSegment(out str);
                        break;
                    case "ReleaseGroup":
                        ValidateReleaseGroup(out str);
                        break;
                    case "Mapping":
                        ValidateLanguageMapping(out str);
                        break;
                }
                return str;
            }
        }

        /// <summary>Gets an error message indicating what is wrong with this object.</summary>
        /// <returns>An error message indicating what is wrong with this object. The default is an empty string ("").</returns>
        public string Error { get; private set; }

        #endregion

        #region Validation Methods
        private bool ValidateLanguageMapping() {
            string str;
            return ValidateLanguageMapping(out str);
        }

        private bool ValidateLanguageMapping(out string error) {
            if (string.IsNullOrEmpty(Mapping)) {
                error = "Language mapping must not be empty";
                return false;
            }

            if (Language == null) {
                error = "Language must be selected";
                return false;
            }

            if (FileNameParser.CustomLanguageMappings.ContainsKey(Mapping)) {
                error = "Mapping already exists.";
                return false;
            }
            error = null;
            return true;
        }

        private bool ValidateKnownSegment() {
            string str;
            return ValidateKnownSegment(out str);
        }

        private bool ValidateKnownSegment(out string error) {
            if (string.IsNullOrEmpty(Segment)) {
                error = "Known segment must not be empty";
                return false;
            }

            if (FileNameParser.KnownSegments.Contains(Segment)) {
                error = "Segment already exists";
                return false;
            }
            error = null;
            return true;
        }

        private bool ValidateExculdedSegment() {
            string str;
            return ValidateExculdedSegment(out str);
        }

        private bool ValidateExculdedSegment(out string error) {
            if (string.IsNullOrEmpty(ExcludedSegment)) {
                error = "Excluded segment must not be empty";
                return false;
            }
            if (FileNameParser.ExcludedSegments.Contains(ExcludedSegment)) {
                error = "Excluded segment already exists";
                return false;
            }
            error = null;
            return true;
        }

        private bool ValidateReleaseGroup() {
            string str;
            return ValidateReleaseGroup(out str);
        }

        private bool ValidateReleaseGroup(out string error) {
            if (string.IsNullOrEmpty(ReleaseGroup)) {
                error = "Release group must not be empty.";
                return false;
            }

            if (ReleaseGroup.Length < 3) {
                error = "Release group must be more than 2 characters";
                return false;
            }

            if (FileNameParser.ReleaseGroups.Contains(ReleaseGroup)) {
                error = "Release group already exists";
                return false;
            }
            error = null;
            return true;
        }

        #endregion

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}