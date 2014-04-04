/*
 *  CPOL Licence http://www.codeproject.com/info/cpol10.aspx 
 *  Copyright @ Richard Deeming 
 *  
 *  Modified and adapted by Martin Kraner
 */
using System;
using System.IO;

namespace Trinet.Networking
{
	#region Share Type
	
	/// <summary>Type of share</summary>
	[Flags]
	public enum ShareType
	{
		/// <summary>Disk share</summary>
		Disk		= 0,
		/// <summary>Printer share</summary>
		Printer		= 1,
		/// <summary>Device share</summary>
		Device		= 2,
		/// <summary>IPC share</summary>
		IPC			= 3,
		/// <summary>Special share</summary>
		Special		= -2147483648, // 0x80000000,
	}
	
	#endregion
	
	#region Share
	
	/// <summary>Information about a local share</summary>
	public class Share
	{
		#region Private data

	    #endregion
		
		#region Constructor

		/// <summary>Constructor</summary>
		public Share(string server, string netName, string path, ShareType shareType, string remark) 
		{
			if (ShareType.Special == shareType && "IPC$" == netName)
			{
				shareType |= ShareType.IPC;
			}
			
			Server = server;
			NetName = netName;
			Path = path;
			ShareType = shareType;
			Remark = remark;
		}
		
		#endregion
		
		#region Properties

	    /// <summary>The name of the computer that this share belongs to</summary>
	    public string Server { get; private set; }

	    /// <summary>Share name</summary>
	    public string NetName { get; private set; }

	    /// <summary>Local path</summary>
	    public string Path { get; private set; }

	    /// <summary>Share type </summary>
	    public ShareType ShareType { get; private set; }

	    /// <summary>Comment</summary>
	    public string Remark { get; private set; }

	    /// <summary>Returns true if this is a file system share</summary>
		public bool IsFileSystem 
		{
			get 
			{
				// Shared device
				if (0 != (ShareType & ShareType.Device)) {
				    return false;
				}

				// IPC share
				if (0 != (ShareType & ShareType.IPC)) {
				    return false;
				}

				// Shared printer
				if (0 != (ShareType & ShareType.Printer)) {
				    return false;
				}
				
				// Standard disk share
				if (0 == (ShareType & ShareType.Special)) {
				    return true;
				}
				
				// Special disk share (e.g. C$)
				return (ShareType.Special == ShareType) && !string.IsNullOrEmpty(NetName);
			}
		}

		/// <summary>Get the root of a disk-based share</summary>
		public DirectoryInfo Root 
		{
			get {
			    if (IsFileSystem) {
				    if (string.IsNullOrEmpty(Server)) {
				        return string.IsNullOrEmpty(Path)
				                       ? new DirectoryInfo(ToString())
				                       : new DirectoryInfo(Path);
				    }
				    return new DirectoryInfo(ToString());
				}
			    return null;
			}
		}
		
		#endregion

		/// <summary>Returns the path to this share</summary>
		/// <returns></returns>
		public override string ToString() {
		    return string.Format(@"\\{0}\{1}", string.IsNullOrEmpty(Server)
		                                               ? Environment.MachineName
		                                               : Server, NetName);
		}

	    /// <summary>Returns true if this share matches the local path</summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public bool MatchesPath(string path) 
		{
			if (!IsFileSystem) {
			    return false;
			}

			return string.IsNullOrEmpty(path) || path.ToLower().StartsWith(Path.ToLower());
	    }
	}
	
	#endregion
}