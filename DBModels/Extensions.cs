using System;
using System.Collections.Generic;
using System.IO;
using Frost.Common;
using Frost.Common.Util;
using Frost.Model.Xbmc.DB;
using Frost.Model.Xbmc.NFO;
using File = Frost.Models.Frost.DB.Files.File;

namespace Frost.Models.Frost {

    public static class Extensions {
        /// <summary>Adds the file info as an instance <see cref="File">File</see> to the collection if the file with specified filename it exists.</summary>
        /// <param name="files">The files collection to add to.</param>
        /// <param name="filename">The filename to check.</param>
        /// <returns>Returns <b>true</b> if the fille exist and there was no error retrieving its info; otherwise <b>false</b>.</returns>
        public static void AddFile(this ICollection<File> files, string filename) {
            try {
                FileInfo fi = new FileInfo(filename);

                files.Add(new File(fi.Name, fi.Extension, fi.FullName, fi.Length));
            }
            catch (Exception) {
            }
        }

        /// <summary>Gets the files containing the movie as a <see cref="HashSet{T}"/> with <see cref="File">File</see>elements.</summary>
        /// <returns>A <see cref="HashSet{T}"/> with <see cref="File">File</see> elements.</returns>
        public static ObservableHashSet<File> GetFiles(this XbmcXmlMovie xm) {
            ObservableHashSet<File> files = new ObservableHashSet<File>();
            if (string.IsNullOrEmpty(xm.FilenameAndPath)) {
                return files;
            }

            string fn = xm.FilenameAndPath;

            //if file is stacked split into individual filenames
            if (fn.StartsWith(XbmcFile.STACK_PREFIX)) {
                //remove the "stack://" prefix
                fn = fn.Replace(XbmcFile.STACK_PREFIX, "");

                foreach (string fileName in fn.SplitWithoutEmptyEntries(XbmcFile.STACK_FILE_SEPARATOR)) {
                    files.AddFile(fileName.Trim().ToWinPath());
                }
            }
            else {
                //if not then just add the filename as is
                files.AddFile(fn.ToWinPath());
            }
            return files;
        }
    }

}