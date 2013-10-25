using System.Collections.Generic;
using System.Collections.ObjectModel;
using Common.Models.DB.MovieVo;

namespace WPF_Jukebox {
    public class Movies : ObservableCollection<Movie>{
         public void AddRange(IEnumerable<Movie> movies) {
             foreach (Movie movie in movies) {
                 Add(movie);
             }
         }
    }
}