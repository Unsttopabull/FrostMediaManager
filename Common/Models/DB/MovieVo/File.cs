using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models.DB.MovieVo {

    public class File {

        public File(string name, string extension, string pathOnDrive, long? size) {
            Extension = extension;
            Name = name;
            FolderPath = pathOnDrive;
            Size = size;

            Movie = new Movie();
        }

        [Key]
        public long Id { get; set; }

        public string Extension { get; set; }

        public string Name { get; set; }

        public string FolderPath { get; set; }

        public long? Size { get; set; }

        public long MovieId { get; set; }

        [Required]
        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }
    }
}
