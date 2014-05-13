using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using log4net;
using log4net.Config;
using File = System.IO.File;

namespace Frost.Tester {

    internal class Program {
        private static readonly string Filler;
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        static Program() {
            Filler = string.Join("", Enumerable.Repeat("_", Console.BufferWidth));
        }

        private static void Main() {
            if (File.Exists("log4Net.config")) {
                XmlConfigurator.Configure(new FileInfo("log4Net.config"));
            }
            else {
                BasicConfigurator.Configure();
            }

            Stopwatch sw = Stopwatch.StartNew();

            TimeSpan time = default(TimeSpan);

            //TestTraktTv();
            //TestOpenSubtitlesOrg();

            using (PoGenerator pg = new PoGenerator("en.po")) {
                pg.Generate();
            }

            sw.Stop();

            Console.WriteLine(Filler);
            Console.WriteLine("\tFIN: " + sw.Elapsed);
            Console.WriteLine(Filler);
            Console.Read();
        }
    }

}