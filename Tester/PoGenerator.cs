using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Frost.Tester {

    internal class PoGenerator : IDisposable {
        private const string XAML_PATTERN = "{l:T ([^},]*),?[^}]*}";
        //private const string XAML_PATTERN = "{l:T ([^,]*),?[^}]*}";
        private const string CS_PATTERN = "Gettext\\.T\\((\\\"[^)]*\\\")\\)";
        private static readonly Regex XamlRegex = new Regex(XAML_PATTERN);
        private static readonly Regex CsRegex = new Regex(CS_PATTERN);
        private readonly HashSet<string> _unique;
        private readonly StreamWriter _sw;

        public PoGenerator(string outputFilePath) {
            _unique = new HashSet<string>();
            _sw = new StreamWriter(outputFilePath);
        }

        public void Generate() {
            DirectoryInfo di = new DirectoryInfo(@"E:\Workspace\Ostalo\Repos\Git\FrostMediaManager2");

            OutputGettextStrings(XamlRegex, di.EnumerateFiles("*.xaml", SearchOption.AllDirectories), true);
            OutputGettextStrings(CsRegex, di.EnumerateFiles("*.cs", SearchOption.AllDirectories), false);
        }

        private void OutputGettextStrings(Regex regex, IEnumerable<FileInfo> files, bool quotes) {
            string quote = quotes ? "\"" : "";

            foreach (FileInfo codeFile in files) {
                string fileText;
                using (StreamReader sr = codeFile.OpenText()) {
                    fileText = sr.ReadToEnd();
                }

                MatchCollection matchCollection = regex.Matches(fileText);
                if (matchCollection.Count == 0) {
                    continue;
                }

                foreach (Match match in matchCollection) {
                    if (match.Groups.Count < 2) {
                        continue;
                    }

                    string value = match.Groups[1].Value;
                    if (!_unique.Contains(value)) {
                        _sw.WriteLine("msgid " + quote + value + quote);
                        _sw.WriteLine("msgstr " + quote + value + quote);
                        _sw.WriteLine();

                        _unique.Add(value);
                    }
                }
            }
        }

        #region IDisposable

        public bool IsDisposed { get; private set; }

        public void Dispose() {
            Dispose(false);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        private void Dispose(bool finalizer) {
            if (!IsDisposed) {
                if (_sw != null) {
                    try {
                        _sw.Dispose();
                    }
                    catch {
                    }
                }

                if (!finalizer) {
                    GC.SuppressFinalize(this);
                }
                IsDisposed = true;
            }
        }

        ~PoGenerator() {
            Dispose(true);
        }

        #endregion
    }

}