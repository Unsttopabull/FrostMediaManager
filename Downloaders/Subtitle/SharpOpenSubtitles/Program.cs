using CookComputing.XmlRpc;
using Frost.SharpOpenSubtitles.Models.UI;

namespace Frost.SharpOpenSubtitles {
    public class Program {
        private readonly OpenSubtitlesClient _rpc;

        public Program(OpenSubtitlesClient rpc) {
            _rpc = rpc;
        }

        /// <summary>Get available translations for given program</summary>
        /// <param name="program">Name of the program/client application you want translations for (currently supported: subdownloader, oscar).</param>
        /// <returns>Returns a structure containing all available translations for a program program.</returns>
        public AvailableTranslationsInfo GetAvailableTranslations(string program) {
            return _rpc.Proxy.GetAvailableTranslations(_rpc.Token, program);
        }

        /// <summary>Get a translation for given program and language.</summary>
        /// <param name="iso639">Language ​ISO639-1 2-letter code.</param>
        /// <param name="format">
        /// Available formats:
        /// <list type="bullet">
        ///     <item><description>gnugettext compatible: mo, po</description></item>
        ///     <item><description>additional: txt, xml</description></item>
        /// </list>
        /// </param>
        /// <param name="program">Name of the program/client application you want translations for (currently supported: subdownloader, oscar).</param>
        /// <remarks>
        /// This function is used to provide multi-language support for client applications in a single environment.<br />
        /// The language translation strings are stored on the server which provides user interface to edit/update them.<br />
        /// Updated strings can then be downloaded to client application using this method.
        /// </remarks>
        /// <returns>Returns base64-encoded translation strings for program <paramref name="program"/> and language <paramref name="iso639"/> in format <paramref name="format"/>.</returns>
        public Translation GetTranslation(string iso639, string format, string program) {
            return _rpc.Proxy.GetTranslation(_rpc.Token, iso639, format, program);
        }

        /// <summary>Checks for the latest version of application <paramref name="programName"/>.</summary>
        /// <param name="programName">Name of the program/client application you want to check (currently supported: subdownloader, oscar).</param>
        /// <returns>Returns download links and changelog (latest features and bugfixes).</returns>
        public UpdateInfo AutoUpdate([XmlRpcParameter("program_name")] string programName) {
            return _rpc.Proxy.AutoUpdate(programName);
        }
    }
}
