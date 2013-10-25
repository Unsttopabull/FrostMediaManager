using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace Trinet.Networking {
    /// <summary>
    /// A collection of shares
    /// </summary>
    public class ShareCollection : ReadOnlyCollectionBase
    {
        #region Platform
		
        /// <summary>Is this an NT platform?</summary>
        protected static bool IsNT
        {
            get { return (PlatformID.Win32NT == Environment.OSVersion.Platform); }
        }

        /// <summary>Returns true if this is Windows 2000 or higher</summary>
        protected static bool IsW2KUp
        {
            get {
                OperatingSystem os = Environment.OSVersion;
                return (PlatformID.Win32NT == os.Platform) && (os.Version.Major >= 5);
            }
        }
		
        #endregion

        #region Interop
		
        #region Constants
		
        /// <summary>Maximum path length</summary>
        protected const int MAX_PATH = 260;
        /// <summary>No error</summary>
        protected const int NO_ERROR = 0;
        /// <summary>Access denied</summary>
        protected const int ERROR_ACCESS_DENIED = 5;
        /// <summary>Access denied</summary>
        protected const int ERROR_WRONG_LEVEL = 124;
        /// <summary>More data available</summary>
        protected const int ERROR_MORE_DATA = 234;
        /// <summary>Not connected</summary>
        protected const int ERROR_NOT_CONNECTED = 2250;
        /// <summary>Level 1</summary>
        protected const int UNIVERSAL_NAME_INFO_LEVEL = 1;
        /// <summary>Max extries (9x)</summary>
        protected const int MAX_SI50_ENTRIES = 20;
		
        #endregion
		
        #region Structures
		
        /// <summary>Unc name</summary>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        protected struct UNIVERSAL_NAME_INFO 
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpUniversalName;
        }

        /// <summary>Share information, NT, level 2</summary>
        /// <remarks>
        /// Requires admin rights to work. 
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        protected struct SHARE_INFO_2 
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string NetName;
            public ShareType ShareType;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Remark;
            public int Permissions;
            public int MaxUsers;
            public int CurrentUsers;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Path;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Password;
        }
		
        /// <summary>Share information, NT, level 1</summary>
        /// <remarks>
        /// Fallback when no admin rights.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
        protected struct SHARE_INFO_1 
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string NetName;
            public ShareType ShareType;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string Remark;
        }
		
        /// <summary>Share information, Win9x</summary>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
        protected struct SHARE_INFO_50 
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=13)]
            public string NetName;

            public byte bShareType;
            public ushort Flags;
			
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Remark;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Path;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=9)]
            public string PasswordRW;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=9)]
            public string PasswordRO;
			
            public ShareType ShareType
            {
                get { return (ShareType)((int)bShareType & 0x7F); }
            }
        }
		
        /// <summary>Share information level 1, Win9x</summary>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
        protected struct SHARE_INFO_1_9x 
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=13)]
            public string NetName;
            public byte Padding;

            public ushort bShareType;
			
            [MarshalAs(UnmanagedType.LPTStr)]
            public string Remark;
			
            public ShareType ShareType
            {
                get { return (ShareType)(bShareType & 0x7FFF); }
            }
        }

        #endregion
		
        #region Functions
		
        /// <summary>Get a UNC name</summary>
        [DllImport("mpr", CharSet=CharSet.Auto)]
        protected static extern int WNetGetUniversalName (string lpLocalPath,
                                                          int dwInfoLevel, ref UNIVERSAL_NAME_INFO lpBuffer, ref int lpBufferSize);

        /// <summary>Get a UNC name</summary>
        [DllImport("mpr", CharSet=CharSet.Auto)]
        protected static extern int WNetGetUniversalName (string lpLocalPath,
                                                          int dwInfoLevel, IntPtr lpBuffer, ref int lpBufferSize);

        /// <summary>Enumerate shares (NT)</summary>
        [DllImport("netapi32", CharSet=CharSet.Unicode)]
        protected static extern int NetShareEnum (string lpServerName, int dwLevel,
                                                  out IntPtr lpBuffer, int dwPrefMaxLen, out int entriesRead,
                                                  out int totalEntries, ref int hResume);

        /// <summary>Enumerate shares (9x)</summary>
        [DllImport("svrapi", CharSet=CharSet.Ansi)]
        protected static extern int NetShareEnum(
                [MarshalAs(UnmanagedType.LPTStr)] string lpServerName, int dwLevel,
                IntPtr lpBuffer, ushort cbBuffer, out ushort entriesRead,
                out ushort totalEntries);

        /// <summary>Free the buffer (NT)</summary>
        [DllImport("netapi32")]
        protected static extern int NetApiBufferFree(IntPtr lpBuffer);
		
        #endregion
		
        #region Enumerate shares
		
        /// <summary>Enumerates the shares on Windows NT</summary>
        /// <param name="server">The server name</param>
        /// <param name="shares">The ShareCollection</param>
        protected static void EnumerateSharesNT(string server, ShareCollection shares)
        {
            int level = 2;
            int hResume = 0;
            IntPtr pBuffer = IntPtr.Zero;

            try 
            {
                int entriesRead;
                int totalEntries;
                int nRet = NetShareEnum(server, level, out pBuffer, -1,  out entriesRead, out totalEntries, ref hResume);

                if (ERROR_ACCESS_DENIED == nRet) 
                {
                    //Need admin for level 2, drop to level 1
                    level = 1;
                    nRet = NetShareEnum(server, level, out pBuffer, -1, out entriesRead, out totalEntries, ref hResume);
                }

                if (NO_ERROR != nRet || entriesRead <= 0) {
                    return;
                }

                Type t = (2 == level) ? typeof(SHARE_INFO_2) : typeof(SHARE_INFO_1);
                int offset = Marshal.SizeOf(t);

                for (int i=0, lpItem = pBuffer.ToInt32(); i<entriesRead; i++, lpItem+=offset) 
                {
                    IntPtr pItem = new IntPtr(lpItem);
                    if (1 == level) 
                    {
                        SHARE_INFO_1 si = (SHARE_INFO_1)Marshal.PtrToStructure(pItem, t);
                        shares.Add(si.NetName, string.Empty, si.ShareType, si.Remark);
                    }
                    else 
                    {
                        SHARE_INFO_2 si = (SHARE_INFO_2)Marshal.PtrToStructure(pItem, t);
                        shares.Add(si.NetName, si.Path, si.ShareType, si.Remark);
                    }
                }
            }
            finally 
            {
                // Clean up buffer allocated by system
                if (IntPtr.Zero != pBuffer) {
                    NetApiBufferFree(pBuffer);
                }
            }
        }
		
        /// <summary>Enumerates the shares on Windows 9x</summary>
        /// <param name="server">The server name</param>
        /// <param name="shares">The ShareCollection</param>
        protected static void EnumerateShares9x(string server, ShareCollection shares)
        {
            int level = 50;
            int nRet = 0;
            ushort entriesRead, totalEntries;
			
            Type t = typeof(SHARE_INFO_50);
            int size = Marshal.SizeOf(t);
            ushort cbBuffer = (ushort)(MAX_SI50_ENTRIES * size);
            //On Win9x, must allocate buffer before calling API
            IntPtr pBuffer = Marshal.AllocHGlobal(cbBuffer);

            try 
            {
                nRet = NetShareEnum(server, level, pBuffer, cbBuffer, out entriesRead, out totalEntries);
				
                if (ERROR_WRONG_LEVEL == nRet)
                {
                    level = 1;
                    t = typeof(SHARE_INFO_1_9x);
                    size = Marshal.SizeOf(t);
					
                    nRet = NetShareEnum(server, level, pBuffer, cbBuffer, out entriesRead, out totalEntries);
                }

                if (NO_ERROR == nRet || ERROR_MORE_DATA == nRet) 
                {
                    for (int i=0, lpItem=pBuffer.ToInt32(); i<entriesRead; i++, lpItem+=size) 
                    {
                        IntPtr pItem = new IntPtr(lpItem);
						
                        if (1 == level)
                        {
                            SHARE_INFO_1_9x si = (SHARE_INFO_1_9x)Marshal.PtrToStructure(pItem, t);
                            shares.Add(si.NetName, string.Empty, si.ShareType, si.Remark);
                        }
                        else
                        {
                            SHARE_INFO_50 si = (SHARE_INFO_50)Marshal.PtrToStructure(pItem, t);
                            shares.Add(si.NetName, si.Path, si.ShareType, si.Remark);
                        }
                    }
                }
                else
                    Console.WriteLine(nRet);
				
            }
            finally 
            {
                //Clean up buffer
                Marshal.FreeHGlobal(pBuffer);
            }
        }
		
        /// <summary>Enumerates the shares</summary>
        /// <param name="server">The server name</param>
        /// <param name="shares">The ShareCollection</param>
        protected static void EnumerateShares(string server, ShareCollection shares)
        {
            if (!string.IsNullOrEmpty(server) && !IsW2KUp) 
            {
                server = server.ToUpper();
				
                // On NT4, 9x and Me, server has to start with "\\"
                if (!('\\' == server[0] && '\\' == server[1])) {
                    server = @"\\" + server;
                }
            }

            if (IsNT) {
                EnumerateSharesNT(server, shares);
            }
            else {
                EnumerateShares9x(server, shares);
            }
        }
		
        #endregion
		
        #endregion
		
        #region Static methods

        /// <summary>
        /// Returns true if fileName is a valid local file-name of the form:
        /// X:\, where X is a drive letter from A-Z
        /// </summary>
        /// <param name="fileName">The filename to check</param>
        /// <returns></returns>
        public static bool IsValidFilePath(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) {
                return false;
            }

            char drive = char.ToUpper(fileName[0]);
            if ('A' > drive || drive > 'Z') {
                return false;

            }

            if (Path.VolumeSeparatorChar != fileName[1]) {
                return false;
            }

            return Path.DirectorySeparatorChar == fileName[2];
        }

        /// <summary>
        /// Returns the UNC path for a mapped drive or local share.
        /// </summary>
        /// <param name="fileName">The path to map</param>
        /// <returns>The UNC path (if available)</returns>
        public static string PathToUnc(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) {
                return string.Empty;
            }
			
            fileName = Path.GetFullPath(fileName);
            if (!IsValidFilePath(fileName)) {
                return fileName;
            }
			
            int nRet = 0;
            UNIVERSAL_NAME_INFO rni = new UNIVERSAL_NAME_INFO();
            int bufferSize = Marshal.SizeOf(rni);

            nRet = WNetGetUniversalName(fileName, UNIVERSAL_NAME_INFO_LEVEL, ref rni, ref bufferSize);
			
            if (ERROR_MORE_DATA == nRet) 
            {
                IntPtr pBuffer = Marshal.AllocHGlobal(bufferSize);;
                try 
                {
                    nRet = WNetGetUniversalName(fileName, UNIVERSAL_NAME_INFO_LEVEL, pBuffer, ref bufferSize);

                    if (NO_ERROR == nRet) 
                    {
                        rni = (UNIVERSAL_NAME_INFO)Marshal.PtrToStructure(pBuffer, typeof(UNIVERSAL_NAME_INFO));
                    }
                }
                finally 
                {
                    Marshal.FreeHGlobal(pBuffer);
                }
            }

            switch (nRet) 
            {
                case NO_ERROR:
                    return rni.lpUniversalName;

                case ERROR_NOT_CONNECTED:
                    //Local file-name
                    ShareCollection shi = LocalShares;
                    if (null != shi)
                    {
                        Share share = shi[fileName];
                        if (null != share)
                        {
                            string path = share.Path;
                            if (!string.IsNullOrEmpty(path))
                            {
                                int index = path.Length;
                                if (Path.DirectorySeparatorChar != path[path.Length - 1])
                                    index++;
								
                                fileName = (index < fileName.Length)
                                                   ? fileName.Substring(index)
                                                   : string.Empty;
								
                                fileName = Path.Combine(share.ToString(), fileName);
                            }
                        }
                    }
					
                    return fileName;

                default:
                    Console.WriteLine("Unknown return value: {0}", nRet);
                    return string.Empty;
            }
        }

        /// <summary>
        /// Returns the local <see cref="Share"/> object with the best match
        /// to the specified path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Share PathToShare(string fileName) 
        {
            if (string.IsNullOrEmpty(fileName)) {
                return null;
            }

            fileName = Path.GetFullPath(fileName);
            if (!IsValidFilePath(fileName)) {
                return null;
            }

            ShareCollection shi = LocalShares;
            return (null == shi)
                         ? null
                         : shi[fileName];
        }
		
        #endregion
		
        #region Local shares
		
        /// <summary>The local shares</summary>
        private static ShareCollection _local;

        /// <summary>Return the local shares</summary>
        public static ShareCollection LocalShares 
        {
            get { return _local ?? (_local = new ShareCollection()); }
        }
		
        /// <summary>Return the shares for a specified machine</summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public static ShareCollection GetShares(string server)
        {
            return new ShareCollection(server);
        }

        #endregion
		
        #region Private Data

        #endregion
		
        #region Constructor

        /// <summary>Default constructor - local machine</summary>
        public ShareCollection() 
        {
            Server = string.Empty;
            EnumerateShares(Server, this);
        }

        /// <summary>Constructor</summary>
        /// <param name="server"></param>
        public ShareCollection(string server) 
        {
            Server = server;
            EnumerateShares(Server, this);
        }
		
        #endregion
		
        #region Add
		
        protected void Add(Share share)
        {
            InnerList.Add(share);
        }
		
        protected void Add(string netName, string path, ShareType shareType, string remark)
        {
            InnerList.Add(new Share(Server, netName, path, shareType, remark));
        }
		
        #endregion
		
        #region Properties

        /// <summary>Returns the name of the server this collection represents</summary>
        public string Server { get; private set; }

        /// <summary>Returns the <see cref="Share"/> at the specified index.</summary>
        public Share this[int index] 
        {
            get { return (Share)InnerList[index]; }
        }

        /// <summary>Returns the <see cref="Share"/> which matches a given local path</summary>
        /// <param name="path">The path to match</param>
        public Share this[string path] 
        {
            get 
            {
                if (string.IsNullOrEmpty(path)) {
                    return null;
                }
				
                path = Path.GetFullPath(path);
                if (!IsValidFilePath(path)) return null;

                Share match = null;
				
                foreach (Share s in InnerList) {
                    if (s.IsFileSystem && s.MatchesPath(path)) 
                    {
                        //Store first match
                        if (null == match) {
                            match = s;

                            // If this has a longer path,
                            // and this is a disk share or match is a special share, 
                            // then this is a better match
                        }
                        else if (match.Path.Length < s.Path.Length) {
                            if (ShareType.Disk == s.ShareType || ShareType.Disk != match.ShareType)
                                match = s;
                        }
                    }
                }

                return match;
            }
        }
		
        #endregion

        #region Implementation of ICollection

        /// <summary>Copy this collection to an array</summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Share[] array, int index) 
        {
            InnerList.CopyTo(array, index);
        }

        #endregion
    }
}