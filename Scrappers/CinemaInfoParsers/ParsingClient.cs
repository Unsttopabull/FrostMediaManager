using System.Collections.Generic;

namespace Frost.InfoParsers {

    public abstract class ParsingClient {

        public ParsingClient(string name) {
            Name = name;
        }

        public string Name { get; set; }

        public IList<ParsedMovie> AvailableMovies { get; protected set; }

        public abstract List<ParsedMovie> Parse();
        public abstract ParsedMovieInfo ParseMovieInfo(ParsedMovie movie);

        //public void PullAllMovieInfo() {
        //    Task[] tsk = new Task[AvailableMovies.Count];
        //    for (int i = 0; i < AvailableMovies.Count; i++) {
        //        Console.WriteLine(AvailableMovies[i]);
        //        while (tsk.Count(t => t != null && !t.IsCompleted) >= 5) {
        //            Thread.Sleep(500);
        //        }
        //        tsk[i] = ParseMovieInfo(AvailableMovies[i]);
        //    }

        //    Task.WaitAll(tsk.ToArray());
        //}
    }

}