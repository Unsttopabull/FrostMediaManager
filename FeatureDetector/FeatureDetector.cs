using System;
using System.Collections.Generic;
using Frost.DetectFeatures.Util;
using Frost.SharpMediaInfo;

namespace Frost.DetectFeatures {

    /// <summary>Represents how the <see cref="FeatureDetector"/> will used the information in a NFO file if found.</summary>
    public enum NFOPriority {
        /// <summary>Ignore the NFO file completely.</summary>
        Ignore,
        /// <summary>If <see cref="FeatureDetector"/> and NFO both contain informaton about a particular feature use the one in the NFO.</summary>
        OverrideDetected,
        /// <summary>Use NFO information only for things not detected by <see cref="FeatureDetector"/>.</summary>
        OnlyNotDetected
    }

    /// <summary>A class used for detecting file information and features.</summary>
    public class FeatureDetector : IDisposable {

        internal readonly MediaInfoList MediaList;
        internal readonly Dictionary<string, FileNameInfo> FileNameInfos;

        /// <summary>Initializes a new instance of the <see cref="FeatureDetector"/> class.</summary>
        public FeatureDetector() {
            FileNameInfos = new Dictionary<string, FileNameInfo>();
            MediaList = new MediaInfoList();
        }

        /// <param name="filePath">The filepath of the file to check for features.</param>
        /// <param name="nfoPriority">How to handle information in a NFO file if found.</param>
        public FileFeatures Detect(string filePath, NFOPriority nfoPriority = NFOPriority.OnlyNotDetected) {
            if (string.IsNullOrEmpty(filePath)) {
                throw new ArgumentNullException("filePath");
            }

            FileFeatures file = new FileFeatures(filePath, nfoPriority, this);
            file.Detect();

            return file;
        }

        #region IDisposable

        /// <summary>Gets a value indicating whether this object has already been disposed.</summary>
        /// <value>Is <c>true</c> if this object has already been disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose() {
            if (!IsDisposed) {
                MediaList.Close();
                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        void IDisposable.Dispose() {
            Dispose();
        }

        ~FeatureDetector() {
            Dispose();
        }
        #endregion

    }

}