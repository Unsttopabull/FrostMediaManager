using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Trinet.Networking;

namespace Frost.Providers.Xtreamer.Provider {
    public class XtFindDb {
        private const string XJB_DB_LOC = @"\\{0}\{1}\{2}\scripts\Xtreamering\var\db";
        private const string XJB_DB_LOC2 = @"\\{0}\{1}\{2}\scripts\var\db";

        private static readonly string[] XtNames = { "Xtreamer", "Xtreamer_PRO" };
        private static readonly string[] HostNames = { "MYXTREAMER" };

        public static string FindXjbDriveLocation(string dbLoc = null) {
            if (dbLoc == null) {
                dbLoc = FindXjbDB();
            }

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

        public static string FindXjbDB() {
            string loc = null;
            foreach (string hostName in HostNames) {
                loc = CheckHostNameForDB(hostName);
            }
            return loc;
        }

        private static string CheckHostNameForDB(string hostName) {
            //preverimo če hostname obstaja v DNS
            if (CheckDns(hostName)) {
                ShareCollection shares = ShareCollection.GetShares(hostName);
                return shares != null
                    ? CheckShares(hostName, shares)
                    : null;
            }

            return XtNames.Select(xtName => string.Format(XJB_DB_LOC, hostName, xtName, "sda1"))
                          .Where(Directory.Exists)
                          .FirstOrDefault();
        }

        private static string CheckShares(string hostName, ShareCollection shares) {
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
                    string checkShares = CheckShare(hostName, netName, folder.Name);
                    if (checkShares != null) {
                        return checkShares;
                    }
                }
            }
            return null;
        }

        private static string CheckShare(string hostName, string netName, string folderName) {
            string loc = string.Format(XJB_DB_LOC, hostName, netName, folderName);

            string pot = CheckXjbDBLocation(loc);
            if (pot != null) {
                return pot;
            }

            loc = string.Format(XJB_DB_LOC2, hostName, netName, folderName);
            return CheckXjbDBLocation(loc);
        }

        private static string CheckXjbDBLocation(string loc) {
            string dbPath = loc + @"\xjb.db";

            return !File.Exists(dbPath)
                           ? null
                           : dbPath;
        }

        private static bool CheckDns(string hostName) {
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
