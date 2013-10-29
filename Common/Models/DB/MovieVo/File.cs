using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class File {

        public File() {
            Movie = new Movie();

            AudioDetails = new HashSet<Audio>();
            VideoDetails = new HashSet<Video>();
            Subtitles = new HashSet<Subtitle>();
        }

        public File(string name, string extension, string pathOnDrive, long? size) : this() {
            Extension = extension;
            Name = name;
            FolderPath = pathOnDrive;
            Size = size;
        }

        [Key]
        public long Id { get; set; }

        public string Extension { get; set; }

        public string Name { get; set; }

        public string FolderPath { get; set; }

        public long? Size { get; set; }

        public DateTime DateAdded { get; set; }

        public long MovieId { get; set; }

        [Required]
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        public virtual ICollection<Audio> AudioDetails { get; set; }
        public virtual ICollection<Video> VideoDetails { get; set; }
        public virtual ICollection<Subtitle> Subtitles { get; set; }

        public string GetFullPath() {
            return FolderPath + Name;
        }
    }
}
