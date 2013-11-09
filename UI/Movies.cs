using System.Collections.Generic;
using System.Collections.ObjectModel;
using Frost.Common.Models.DB.MovieVo;

namespace Frost.UI {
    public class Movies : ObservableCollection<Movie>{
         public void AddRange(IEnumerable<Movie> movies) {
             foreach (Movie movie in movies) {
                 Add(movie);
             }
         }
    }
}