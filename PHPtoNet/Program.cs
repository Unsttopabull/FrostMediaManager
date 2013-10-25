using System;
using System.IO;

namespace PHPSerialize {
    class Program {
        static void Main() {
            //const string TEST = "Tests/CoretisVO/CoretisVoMovie.txt";
            //const string TEST = "Tests/CoretisVO/CoretisVoMovieObjArr.txt";
            const string TEST = "Tests/Scripts/EmbeddedClass.txt";

            //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes("c:555;"));
            IScanner scanner = new Scanner(new StreamReader(TEST));

            ClassTest(scanner);
            //Hashtable mixedKeyArray = PHPArrayDeserializer.ParseMixedKeyArray(scanner);
            //double[] arr = PHPArrayDeserializer.ParseSingleTypeArray<double>(scanner);
            //Coretis_VO_Person[] persons = PHPArrayDeserializer.ParseSingleTypeArray<Coretis_VO_Person>(scanner);

            Console.WriteLine("FIN!");
            Console.ReadKey();
        }

        private static void ClassTest(IScanner scanner){
            //Coretis_VO_Movie tc = new Coretis_VO_Movie();
            //EmbeddedClass tc = new EmbeddedClass();

            //PHPObjectParser op = new PHPObjectParser(scanner);
            //op.Obj(ref tc);
        }
    }
}
