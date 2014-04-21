using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Frost.InfoParsers.Models;
using Newtonsoft.Json;

namespace Frost.InfoParsers {

    public abstract class ParsingClient {
        public List<IParsedMovie> AvailableMovies { get; protected set; }

        public abstract List<IParsedMovie> Parse();

        public void Serialize(string fileName) {
            JsonSerializer jser = new JsonSerializer();
            jser.Serialize(new StreamWriter(File.Create(fileName)), AvailableMovies);
        }

        public void PullAllMovieInfo() {
            Task[] tsk = new Task[AvailableMovies.Count];
            for (int i = 0; i < AvailableMovies.Count; i++) {
                Console.WriteLine(AvailableMovies[i]);
                while (tsk.Count(t => t != null && !t.IsCompleted) >= 5) {
                    Thread.Sleep(500);
                }
                tsk[i] = AvailableMovies[i].ParseMovieInfo();
            }

            Task.WaitAll(tsk.ToArray());
        }
    }

}