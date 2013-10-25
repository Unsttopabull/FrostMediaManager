namespace Common.Models.PHP {
    public class Coretis_VO_Genre {
        public Coretis_VO_Genre() {    
        }

        public Coretis_VO_Genre(int id, string name) : this(name){
            this.id = id;
        }

        public Coretis_VO_Genre(string name) {
            this.name = name;
        }

        /// <summary>The id for this row in DB</summary>
        public int id { get; set; } // id in DB

        /// <summary>Name of the genre</summary>
        public string name { get; set; } // horror, comedy
    }
}
