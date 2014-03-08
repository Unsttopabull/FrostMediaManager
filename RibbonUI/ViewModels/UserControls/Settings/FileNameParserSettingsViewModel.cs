using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frost.Common.Annotations;
using Frost.Common.Util.ISO;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;
using Frost.XamlControls.Commands;

namespace RibbonUI.ViewModels.UserControls.Settings {

    internal class FileNameParserSettingsViewModel : INotifyPropertyChanged, IDataErrorInfo {
        private string _releaseGroupError;
        public event PropertyChangedEventHandler PropertyChanged;

        public FileNameParserSettingsViewModel() {
            if (Directory.Exists("Images/Languages")) {
                Languages = Directory.EnumerateFiles("Images/Languages", "*.png")
                                     .Select(fileName => ISOLanguageCodes.Instance.GetByISOCode(Path.GetFileNameWithoutExtension(fileName)))
                                     .Where(isoCode => isoCode != null)
                                     .ToList();
            }

            RemoveLanguageMappingCommand = new RelayCommand<LanguageMapping>(
                mapping => FileNameParser.CustomLanguageMappings.Remove(mapping),
                mapping => mapping != null
                );

            RemoveExcludedSegmentCommand = new RelayCommand<string>(
                segment => FileNameParser.ExcludedSegments.Remove(segment),
                segment => !string.IsNullOrEmpty(segment)
                );

            RemoveReleaseGroupCommand = new RelayCommand<string>(
                releaseGrp => FileNameParser.ReleaseGroups.Remove(releaseGrp),
                releaseGrp => !string.IsNullOrEmpty(releaseGrp)
                );

            RemoveKnownSegmentCommand = new RelayCommand<SegmentMapping>(
                mapping => FileNameParser.KnownSegments.Remove(mapping),
                mapping => mapping != null
                );

            AddNewLanguageMappingCommand = new RelayCommand<object>(
                o => FileNameParser.CustomLanguageMappings.Add(Mapping, Language.Alpha3),
                o => ValidateLanguageMapping()
                );

            AddNewReleaseGroupCommand = new RelayCommand<object>(
                o => FileNameParser.ReleaseGroups.Add(ReleaseGroup),
                o => ValidateReleaseGroup()
                );

            AddNewExcludedSegmentCommand = new RelayCommand<object>(
                o => FileNameParser.ExcludedSegments.Add(ExcludedSegment),
                o => ValidateExculdedSegment()
                );

            AddNewKnownSegmentCommand = new RelayCommand<object>(
                o => FileNameParser.KnownSegments.Add(Segment, SegmentType),
                o => ValidateKnownSegment()
                );
        }

        public List<ISOLanguageCode> Languages { get; set; }

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


        private bool ValidateLanguageMapping() {
            string str;
            return ValidateLanguageMapping(out str);
        }

        private bool ValidateLanguageMapping(out string error){
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}