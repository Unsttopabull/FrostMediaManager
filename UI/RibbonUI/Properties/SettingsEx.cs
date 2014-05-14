using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Frost.Common.Util.Collections;
using Frost.DetectFeatures;
using Frost.DetectFeatures.FileName;
using Frost.DetectFeatures.Util;

namespace Frost.RibbonUI.Properties {

    internal partial class Settings {
        #region Reset

        internal static void ResetToFactory() {
            Default.SearchFolders = new StringCollection();

            ResetFeatureDetector();
            ResetFileFeatures();
            ResetFileNameParser();

            Default.Save();
        }

        private static void ResetFeatureDetector() {
            foreach (string extension in Default.KnownVideoExtensionsAdded) {
                FeatureDetector.VideoExtensions.Remove(extension);
            }
            Default.KnownVideoExtensionsAdded = new StringCollection();

            foreach (string extension in Default.KnownVideoExtensionsRemoved) {
                FeatureDetector.VideoExtensions.Add(extension);
            }
            Default.KnownVideoExtensionsRemoved = new StringCollection();
        }

        private static void ResetFileFeatures() {
            //Subtitle extensions
            foreach (string extension in Default.KnownSubtitleExtensionsAdded) {
                FileFeatures.KnownSubtitleExtensions.Remove(extension);
            }
            Default.KnownSubtitleExtensionsAdded = new StringCollection();

            foreach (string extension in Default.KnownSubtitleExtensionsRemoved) {
                FileFeatures.KnownSubtitleExtensions.Add(extension);
            }
            Default.KnownSubtitleExtensionsRemoved = new StringCollection();

            //Audio codec bindings
            foreach (KeyValuePair<string, string> pair in Default.AudioCodecIdBindingsAdded) {
                FileFeatures.AudioCodecIdMappings.Remove(pair.Key);
            }
            Default.AudioCodecIdBindingsAdded = new SerializableStringDictionary();

            foreach (KeyValuePair<string, string> pair in Default.AudioCodecIdBindingsRemoved) {
                FileFeatures.AudioCodecIdMappings.Add(pair.Key, pair.Value);
            }
            Default.AudioCodecIdBindingsRemoved = new SerializableStringDictionary();

            //Video codec bindings
            foreach (KeyValuePair<string, string> pair in Default.VideoCodecIdBindingsAdded) {
                FileFeatures.VideoCodecIdMappings.Remove(pair.Key);
            }
            Default.VideoCodecIdBindingsAdded = new SerializableStringDictionary();

            foreach (KeyValuePair<string, string> pair in Default.VideoCodecIdBindingsRemoved) {
                FileFeatures.VideoCodecIdMappings.Add(pair.Key, pair.Value);
            }
            Default.VideoCodecIdBindingsRemoved = new SerializableStringDictionary();
        }

        #region FileNameParser

        private static void ResetFileNameParser() {
            ResetSubtitleFormats();
            ResetExcludedSegments();
            ResetRleaseGroups();
            ResetCustomLanguageMappings();
        }

        private static void ResetCustomLanguageMappings() {
            foreach (KeyValuePair<string, string> pair in Default.CustomLanguageMappingsRemoved) {
                FileNameParser.CustomLanguageMappings.Add(pair.Key, pair.Value);
            }
            Default.CustomLanguageMappingsRemoved = new SerializableStringDictionary();

            foreach (KeyValuePair<string, string> mapping in Default.CustomLanguageMappingsAdded) {
                FileNameParser.CustomLanguageMappings.Remove(mapping.Key);
            }
            Default.CustomLanguageMappingsAdded = new SerializableStringDictionary();
        }

        private static void ResetSubtitleFormats() {
            foreach (KeyValuePair<string, string> pair in Default.KnownSegmentsAdded) {
                FileNameParser.KnownSegments.Remove(pair.Key);
            }
            Default.KnownSubtitleFormatsAdded = new StringCollection();

            foreach (KeyValuePair<string, string> pair in Default.KnownSegmentsRemoved) {
                SegmentType type;
                if (!Enum.TryParse(pair.Value, out type)) {
                    continue;
                }

                FileNameParser.KnownSegments.Add(pair.Key, type);
            }
            Default.KnownSubtitleFormatsRemoved = new StringCollection();
        }

        private static void ResetExcludedSegments() {
            foreach (string segment in Default.ExcludedSegmentsAdded) {
                FileNameParser.ExcludedSegments.Remove(segment);
            }
            Default.ExcludedSegmentsAdded = new StringCollection();

            foreach (string segment in Default.ExcludedSegmentsRemoved) {
                FileNameParser.ExcludedSegments.Add(segment);
            }
            Default.ExcludedSegmentsRemoved = new StringCollection();
        }

        private static void ResetRleaseGroups() {
            foreach (string segment in Default.ReleaseGroupsAdded) {
                FileNameParser.ReleaseGroups.Remove(segment);
            }
            Default.ReleaseGroupsAdded = new StringCollection();

            foreach (string segment in Default.ReleaseGroupsRemoved) {
                FileNameParser.ReleaseGroups.Add(segment);
            }
            Default.ReleaseGroupsRemoved = new StringCollection();
        }

        #endregion

        #endregion

        #region Load

        internal static void Load() {
            FileFeaturesSettings();
            FeatureDetectorSettings();
            FileNameParserSettings();
        }

        private static void FileNameParserSettings() {
            if (Default.KnownSegmentsAdded != null) {
                foreach (KeyValuePair<string, string> segment in Default.KnownSegmentsAdded) {
                    SegmentType type;
                    if (!Enum.TryParse(segment.Value, out type)) {
                        continue;
                    }

                    if (FileNameParser.KnownSegments.ContainsKey(segment.Key)) {
                        FileNameParser.KnownSegments[segment.Key] = type;
                        continue;
                    }

                    FileNameParser.KnownSegments.Add(segment.Key, type);
                }
            }

            if (Default.KnownSegmentsRemoved != null) {
                foreach (KeyValuePair<string, string> segment in Default.KnownSegmentsRemoved) {
                    FileNameParser.KnownSegments.Remove(segment.Key);
                }
            }

            if (Default.CustomLanguageMappingsAdded != null) {
                foreach (KeyValuePair<string, string> binding in Default.CustomLanguageMappingsAdded) {
                    if (FileNameParser.CustomLanguageMappings.ContainsKey(binding.Key)) {
                        FileNameParser.CustomLanguageMappings[binding.Key] = binding.Value;
                        continue;
                    }
                    FileNameParser.CustomLanguageMappings.Add(binding.Key, binding.Value);
                }
            }

            if (Default.CustomLanguageMappingsRemoved != null) {
                foreach (KeyValuePair<string, string> binding in Default.CustomLanguageMappingsRemoved) {
                    FileNameParser.CustomLanguageMappings.Remove(binding.Key);
                }
            }


            if (Default.ExcludedSegmentsAdded != null) {
                foreach (string segment in Default.ExcludedSegmentsAdded) {
                    FileNameParser.ExcludedSegments.Add(segment);
                }
            }

            if (Default.ExcludedSegmentsRemoved != null) {
                foreach (string segment in Default.ExcludedSegmentsRemoved) {
                    FileNameParser.ExcludedSegments.Remove(segment);
                }
            }

            if (Default.ReleaseGroupsAdded != null) {
                foreach (string group in Default.ReleaseGroupsAdded) {
                    FileNameParser.ReleaseGroups.Add(@group);
                }
            }
            if (Default.ReleaseGroupsRemoved != null) {
                foreach (string group in Default.ReleaseGroupsRemoved) {
                    FileNameParser.ReleaseGroups.Remove(@group);
                }
            }
        }

        private static void FeatureDetectorSettings() {
            if (Default.KnownVideoExtensionsAdded != null) {
                foreach (string format in Default.KnownVideoExtensionsAdded) {
                    FeatureDetector.VideoExtensions.Add(format);
                }
            }

            if (Default.KnownVideoExtensionsRemoved != null) {
                foreach (string format in Default.KnownVideoExtensionsRemoved) {
                    FeatureDetector.VideoExtensions.Remove(format);
                }
            }
        }

        private static void FileFeaturesSettings() {
            if (Default.KnownSubtitleFormatsAdded != null) {
                foreach (string format in Default.KnownSubtitleFormatsAdded) {
                    FileFeatures.KnownSubtitleFormats.Add(format);
                }
            }

            if (Default.KnownSubtitleFormatsRemoved != null) {
                foreach (string format in Default.KnownSubtitleFormatsRemoved) {
                    FileFeatures.KnownSubtitleFormats.Remove(format);
                }
            }

            if (Default.KnownSubtitleExtensionsAdded != null) {
                foreach (string extension in Default.KnownSubtitleExtensionsAdded) {
                    FileFeatures.KnownSubtitleExtensions.Add(extension);
                }
            }

            if (Default.KnownSubtitleExtensionsRemoved != null) {
                foreach (string extension in Default.KnownSubtitleExtensionsRemoved) {
                    FileFeatures.KnownSubtitleExtensions.Remove(extension);
                }
            }

            if (Default.AudioCodecIdBindingsAdded != null) {
                foreach (KeyValuePair<string, string> binding in Default.AudioCodecIdBindingsAdded) {
                    if (FileFeatures.AudioCodecIdMappings.ContainsKey(binding.Key)) {
                        FileFeatures.AudioCodecIdMappings[binding.Key] = binding.Value;
                        continue;
                    }

                    FileFeatures.AudioCodecIdMappings.Add(new CodecIdBinding(binding.Key, binding.Value));
                }
            }

            if (Default.AudioCodecIdBindingsRemoved != null) {
                foreach (KeyValuePair<string, string> binding in Default.AudioCodecIdBindingsRemoved) {
                    if (binding.Key != null) {
                        FileFeatures.AudioCodecIdMappings.Remove(binding.Key);
                    }
                }
            }

            if (Default.VideoCodecIdBindingsAdded != null) {
                foreach (KeyValuePair<string, string> binding in Default.VideoCodecIdBindingsAdded) {
                    if (FileFeatures.VideoCodecIdMappings.ContainsKey(binding.Key)) {
                        FileFeatures.VideoCodecIdMappings[binding.Key] = binding.Value;
                        continue;
                    }

                    FileFeatures.VideoCodecIdMappings.Add(new CodecIdBinding(binding.Key, binding.Value));
                }
            }

            if (Default.VideoCodecIdBindingsRemoved != null) {
                foreach (KeyValuePair<string, string> binding in Default.VideoCodecIdBindingsRemoved) {
                    FileFeatures.VideoCodecIdMappings.Remove(binding.Key);
                }
            }
        }

        #endregion

    }

}