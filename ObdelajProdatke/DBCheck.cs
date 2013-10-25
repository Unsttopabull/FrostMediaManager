using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Common;
using Trinet.Networking;

namespace ObdelajProdatke {

    public class DBCheck {
        private const string XJB_DB_LOC = @"\\{0}\{1}\{2}\scripts\Xtreamering\var\db";
        private const string XJB_DB_LOC2 = @"\\{0}\{1}\{2}\scripts\var\db";
        private const string WIN_XBMC_DB_LOC = @"XBMC\userdata\Database\";

        private static readonly string[] XtNames = new[] { "Xtreamer", "Xtreamer_PRO" };
        private static readonly string[] HostNames = new[] { "MYXTREAMER" };

        public static string FindDB(DBSystem sistem) {
            switch (sistem) {
                case DBSystem.Xtreamer:
                    return FindXjbDB();
                case DBSystem.XBMC:
                    return FindXbmcDB();
                default:
                    throw new ArgumentOutOfRangeException("sistem");
            }
        }

        public static string FindXjbDriveLoc() {
            string dbLoc = FindDB(DBSystem.Xtreamer);
            if (dbLoc != null) {
                int num = 0;
                string driveLoc = dbLoc.TakeWhile(c => {
                    if (num > 4) {
                        return false;
                    }

                    if (c == '\\') {
                        num++;
                    }
                    return true;
                }).Aggregate("", (str, c) => str + c);
                return driveLoc.TrimEnd('/', '\\');
            }
            return null;
        }

        private static string FindXbmcDB() {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fn = Path.Combine(appData, WIN_XBMC_DB_LOC);

            string[] di = Directory.GetFiles(fn);

            //escapamo separatorje med mapami da regex ne pomotoma proba narobe razumeti vzorca
            fn = fn.Replace(@"\", @"\\");
            return di.FirstOrDefault(file => Regex.IsMatch(file, fn + @"MyVideos\d+\.db"));
        }

        private static string FindXjbDB() {
            string loc = null;
            foreach (string hostName in HostNames) {
                loc = CheckHostNameForDB(DBSystem.Xtreamer, hostName);
            }
            return loc;
        }

        private static string CheckHostNameForDB(DBSystem sistem, string hostName) {
            //preverimo če hostname obstaja v DNS
            if (CheckDNS(hostName)) {
                ShareCollection shares = ShareCollection.GetShares(hostName);
                return (shares != null)
                               ? CheckShares(sistem, hostName, shares)
                               : null;
            }

            //Console.WriteLine(@"Searching all");
            return XtNames.Select(xtName => string.Format(XJB_DB_LOC, hostName, xtName, "sda1"))
                          .Where(Directory.Exists)
                          .FirstOrDefault();
        }

        private static string CheckShares(DBSystem sistem, string hostName, ShareCollection shares) {
            //vse neskrite diskovne omrežne mape v skupni rabi na podanem gostitelju
            DirectoryInfo[] share = shares.Cast<Share>()
                                          .Where(shr => shr.ShareType == ShareType.Disk)
                                          .Select(shr => shr.Root.GetDirectories())
                                          .FirstOrDefault();

            if (share != null) {
                string netName = null;
                if (share.Length > 0) {
                    netName = share[0].Root.Name;
                }

                foreach (DirectoryInfo folder in share) {
                    string checkShares = CheckShare(sistem, hostName, netName, folder.Name);
                    if (checkShares != null) {
                        return checkShares;
                    }
                }
            }
            return null;
        }

        private static string CheckShare(DBSystem sistem, string hostName, string netName, string folderName) {
            string loc = string.Format(XJB_DB_LOC, hostName, netName, folderName);

            string pot = CheckDBLocation(sistem, loc);
            if (pot != null) {
                return pot;
            }

            loc = string.Format(XJB_DB_LOC2, hostName, netName, folderName);
            return CheckDBLocation(sistem, loc);
        }

        public static string CheckDBLocation(DBSystem sistem, string loc) {
            switch (sistem) {
                case DBSystem.Xtreamer:
                    return CheckXjbDBLocation(loc);
                case DBSystem.XBMC:
                    return CheckXmbcDBLocation(loc);
                default:
                    throw new ArgumentOutOfRangeException("sistem");
            }
        }

        //TODO: Implementiraj
        private static string CheckXmbcDBLocation(string loc) {
            throw new NotImplementedException();
        }

        private static string CheckXjbDBLocation(string loc) {
            string dbPath = loc + @"\xjb.db";

            return !File.Exists(dbPath)
                           ? null
                           : dbPath;
        }

        private static bool CheckDNS(string hostName) {
            try {
                Dns.GetHostEntry(hostName);
            }
            catch (SocketException) {
                return false;
            }
            return true;
        }
    }
}
