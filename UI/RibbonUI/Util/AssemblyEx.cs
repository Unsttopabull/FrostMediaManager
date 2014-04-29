using System;
using System.IO;
using System.Reflection;
using Frost.Common;
using Frost.InfoParsers;

namespace RibbonUI.Util {
    public static class AssemblyEx {

        public static bool CheckIsProvider(string plugin, out Assembly assembly, out Plugin provider) {
            string path = Path.Combine(Directory.GetCurrentDirectory(), plugin);
            if (!CheckIsAssembly(path)) {
                assembly = null;
                provider = null;
                return false;
            }

            try {
                Assembly asm = Assembly.LoadFile(path);
                FrostProviderAttribute isProvider = asm.GetCustomAttribute<FrostProviderAttribute>();
                if (isProvider != null) {
                    assembly = asm;
                    provider = new Plugin(isProvider.SystemName, isProvider.IconPath);
                    return true;
                }

                assembly = null;
                provider = null;
                return false;
            }
            catch {
                assembly = null;
                provider = null;
                return false;
            }
        }

        public static bool CheckIsPlugin(string plugin, out Assembly assembly) {
            string path = Path.Combine(Directory.GetCurrentDirectory(), plugin);
            if (!CheckIsAssembly(path)) {
                assembly = null;
                return false;
            }

            try {
                Assembly asm = Assembly.LoadFile(path);
                FrostPluginAttribute isPlugin = asm.GetCustomAttribute<FrostPluginAttribute>();
                if (isPlugin != null) {
                    assembly = asm;
                    return true;
                }

                assembly = null;
                return false;
            }
            catch(Exception e) {
                assembly = null;
                return false;
            }
        }

        /// <summary>Checks if the file is a valid CLR assembly.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <remarks>Taken from http://geekswithblogs.net/rupreet/archive/2005/11/02/58873.aspx </remarks>
        private static bool CheckIsAssembly(string fileName) {
            uint[] dataDictionaryRva = new uint[16];
            uint[] dataDictionarySize = new uint[16];

            using (Stream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read)) {
                using (BinaryReader reader = new BinaryReader(fs)) {

                    //PE Header starts @ 0x3C (60). Its a 4 byte header.
                    fs.Position = 0x3C;

                    uint peHeader = reader.ReadUInt32();

                    //Moving to PE Header start location...
                    fs.Position = peHeader;
                    reader.ReadUInt32();

                    //We can also show all these value, but we will be       
                    //limiting to the CLI header test.

                    reader.ReadUInt16();
                    reader.ReadUInt16();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt32();
                    reader.ReadUInt16();
                    reader.ReadUInt16();

                    /*
                     *    Now we are at the end of the PE Header and from here, the PE Optional Headers starts...
                     *    To go directly to the datadictionary, we'll increase the stream’s current position to with 96 (0x60). 96 because,
                     *    28 for Standard fields, 68 for NT-specific fields
                     *    From here DataDictionary starts...and its of total 128 bytes.
                     *    DataDictionay has 16 directories in total, doing simple maths 128/16 = 8.
                     * 
                     *    So each directory is of 8 bytes.
                     *    In this 8 bytes, 4 bytes is of RVA and 4 bytes of Size.
                     *
                     *    btw, the 15th directory consist of CLR header! if its 0, its not a CLR file :)
                     */
                    ushort dataDictionaryStart = Convert.ToUInt16(Convert.ToUInt16(fs.Position) + 0x60);
                    fs.Position = dataDictionaryStart;
                    for (int i = 0; i < 15; i++) {
                        dataDictionaryRva[i] = reader.ReadUInt32();
                        dataDictionarySize[i] = reader.ReadUInt32();
                    }

                    return dataDictionaryRva[14] != 0;
                }
            }
        }
    }
}
