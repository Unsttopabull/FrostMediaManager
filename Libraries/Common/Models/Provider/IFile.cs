using System;

namespace Frost.Common.Models.Provider {

    public interface IFile : IMovieEntity {

        ///<summary>The File Extension without beginning point</summary>
        ///<value>The file extension withot begining point</value>
        ///<example>\eg{ ''<c>mp4</c>''}</example>
        string Extension { get; }

        /// <summary>Gets or sets the filename.</summary>
        /// <value>The filename in folder.</value>
        /// <example>\eg{ ''<c>Wall_E</c>''}</example>
        string Name { get; }

        /// <summary>Gets or sets the path to the folder that contains this file</summary>
        /// <value>The full path to the folder that contains this file with trailing '/' without quotes (" or ')</value>
        /// <example>\eg{
        /// 	<list type="bullet">
        ///         <item><description>''<c>C:/Movies/</c>''</description></item>
        /// 		<item><description>''<c>smb://MYXTREAMER/Xtreamer_PRO/sda1/Movies/</c>''</description></item>
        /// 	</list>}
        /// </example>
        string FolderPath { get; }

        /// <summary>Gets or sets the file size in bytes.</summary>
        /// <value>The file size in bytes.</value>
        long? Size { get; }

        /// <summary>Gets or sets the date and time the file was added.</summary>
        /// <value>The date and time the file was added.</value>
        DateTime DateAdded { get; }

        ///// <summary>Gets or sets the details about audio streams in this file</summary>
        ///// <value>The details about audio streams in this file</value>
        //IEnumerable<IAudio> AudioDetails { get; }

        ///// <summary>Gets or sets the details about video streams in this file</summary>
        ///// <value>The details about video streams in this file</value>
        //IEnumerable<IVideo> VideoDetails { get; }

        ///// <summary>Gets or sets the details about subtitles in this file</summary>
        ///// <value>The details about subtitles in this file</value>
        //IEnumerable<ISubtitle> Subtitles { get; }

        /// <summary>Gets the full path to the file.</summary>
        /// <value>A full path filename to the fille or <b>null</b> if any of <b>FolderPath</b> or <b>FileName</b> are null</value>
        string FullPath { get; }

        /// <summary>Gets the name with extension.</summary>
        /// <value>The name with extension.</value>
        string NameWithExtension { get; }
    }

}