﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Frost.Common.Models.DB.XBMC {

    /// <summary>This table stores bookmarks, which are timestamps representing the point in a video where a user stopped playback, an explicit bookmark requested by the user, or an automatically generated episode bookmark.</summary>
    [Table("bookmark")]
    public class XbmcBookmark : IEquatable<XbmcBookmark> {

        public XbmcBookmark() {
            File = new XbmcFile();
        }

        /// <summary>Gets or sets the Id of the bookmark in the database.</summary>
        /// <value>The Id of the bookmark in the database</value>
        [Key]
        [Column("idBookmark")]
        public long Id { get; set; }

        /// <summary>Gets or sets the time in seconds of the bookmark location</summary>
        /// <value>The time in seconds of the bookmark location</value>
        [Column("timeInSeconds")]
        public long TimeInSeconds { get; set; }

        /// <summary>Gets or sets the time in seconds of the video</summary>
        /// <value>Time in seconds of the video</value>
        [Column("totalTimeInSeconds")]
        public long TotalTimeInSeconds { get; set; }

        /// <summary>Gets or sets the thumbnail for the bookmark</summary>
        /// <value>Thumbnail for the bookmark</value>
        [Column("thumbNailImage")]
        public string ThumbnailImage { get; set; }

        /// <summary>Gets or set player used to store bookmark</summary>
        /// <value>Player used to store bookmark</value>
        [Column("player")]
        public string Player { get; set; }

        /// <summary>Gets or sets the player's internal state in xml</summary>
        /// <value>Player's internal state in xml</value>
        [Column("playerState")]
        public string PlayerState { get; set; }

        /// <summary>Gets or sets the time in seconds of bookmark location</summary>
        /// <value>The time in seconds of bookmark location</value>
        [Column("type")]
        public BookmarkType Type { get; set; }

        /// <summary>Gets or sets the file for which this bookmark this bookmark was created.</summary>
        /// <value>The file for which this bookmark this bookmark was created.</value>
        public virtual XbmcFile File { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(XbmcBookmark other) {
            if (other == null) {
                return false;
            }

            if (ReferenceEquals(this, other)) {
                return true;
            }

            if (Id != 0 && other.Id != 0) {
                return Id == other.Id;
            }

            return TimeInSeconds == other.TimeInSeconds &&
                   TotalTimeInSeconds == other.TotalTimeInSeconds &&
                   ThumbnailImage == other.ThumbnailImage &&
                   Player == other.Player &&
                   PlayerState == other.PlayerState &&
                   Type == other.Type;
        }

        internal class Configuration : EntityTypeConfiguration<XbmcBookmark> {

            public Configuration() {
                HasRequired(b => b.File)
                    .WithOptional(f => f.Bookmark)
                    .Map(m => m.MapKey("idFile"));
            }

        }

    }

}
