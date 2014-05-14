using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Frost.Common.Util {

    /// <summary>Handles mounting and unmouting of an ISO image files as CD/DVD drives.</summary>
    public static class IsoImageMount {
        private static readonly bool Win8Plus;

        static IsoImageMount() {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Version.Major > 6 || Environment.OSVersion.Version.Minor > 2) {
                Win8Plus = true;
            }
        }

        /// <summary>Mounts the specified file name as an ISO image with the first available drive letter.</summary>
        /// <param name="fileName">Path to the ISO image file.</param>
        /// <param name="closeHandle">if set to <c>true</c> closes the disk handle upon function exit.</param>
        /// <returns>Returns the drive handle that is a mounted ISO image.</returns>
        /// <exception cref="System.NotSupportedException">The operation is only supported in Windows 8 / Windows Server 2012 or newer.</exception>
        /// <exception cref="System.InvalidOperationException">Throws when image mouting has not succeded. See exception error message.</exception>
        public static IntPtr Mount(string fileName, bool closeHandle = true) {
            if (!Win8Plus) {
                throw new NotSupportedException("The operation is only supported in Windows 8 / Windows Server 2012 or newer.");
            }

            IntPtr handle = IntPtr.Zero;
            ErrorCode openResult = (ErrorCode) OpenVirtualDisk(new VirtualStorageType { DeviceId = 0, VendorId = Guid.Empty }, fileName, VirtualDiskAccessMask.VirtualDiskAccessRead,
                                                               OpenVirtualDiskFlag.OpenVirtualDiskFlagNone, IntPtr.Zero, ref handle);

            if (openResult != ErrorCode.Success) {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Native error {0}.", openResult));
            }

            // attach disk - permanently
            AttachVirtualDiskParameters attachParameters = new AttachVirtualDiskParameters { Version = AttachVirtualDiskVersion.AttachVirtualDiskVersion1 };
            ErrorCode attachResult = (ErrorCode) AttachVirtualDisk(handle, IntPtr.Zero, AttachVirtualDiskFlag.AttachVirtualDiskFlagPermanentLifetime | AttachVirtualDiskFlag.AttachVirtualDiskFlagReadOnly,
                                                                    0, ref attachParameters, IntPtr.Zero);

            

            if (attachResult != ErrorCode.Success) {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Native error {0}.", attachResult));
            }

            if (closeHandle) {
                // close handle to disk
                CloseHandle(handle);
            }
            return handle;
        }

        /// <summary>Unmounts the ISO image file using the drive handle.</summary>
        /// <param name="handle">The drive handle that represents the ISO image file.</param>
        /// <exception cref="System.NotSupportedException">The operation is only supported in Windows 7 / Windows Server 2008 R2 or newer.</exception>
        public static void Unmount(IntPtr handle) {
            if (!Win8Plus) {
                throw new NotSupportedException("The operation is only supported in Windows 8 / Windows Server 2008 R2 or newer.");
            }

            DetachVirtualDisk(handle, DetachVirtualDiskFlag.DetachVirtualDiskFlagNone, 0);
        }

        private enum ErrorCode {
            Success = 0,
            InvalidParameter = 87,
            UnsupportedCompression = 618,
            FileEncrypted = 6002,
            FileSystemLimitation = 665,
            FileCorrupt = 1392
        }

        private enum DetachVirtualDiskFlag {
            DetachVirtualDiskFlagNone = 0x00000000
        }

        [Flags]
        private enum AttachVirtualDiskFlag : int {
            AttachVirtualDiskFlagNone = 0x00000000,
            AttachVirtualDiskFlagReadOnly = 0x00000001,
            AttachVirtualDiskFlagNoDriveLetter = 0x00000002,
            AttachVirtualDiskFlagPermanentLifetime = 0x00000004,
            AttachVirtualDiskFlagNoLocalHost = 0x00000008
        }

        private enum AttachVirtualDiskVersion : int {
            AttachVirtualDiskVersionUnspecified = 0,
            AttachVirtualDiskVersion1 = 1
        }

        private enum OpenVirtualDiskFlag {
            OpenVirtualDiskFlagNone = 0x00000000,
            OpenVirtualDiskFlagNoParents = 0x00000001,
            OpenVirtualDiskFlagBlankFile = 0x00000002,
            OpenVirtualDiskFlagBootDrive = 0x00000004,
            OpenVirtualDiskFlagCachedIo = 0x00000008,
            OpenVirtualDiskFlagCustomDiffChain = 0x00000010
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct AttachVirtualDiskParameters {
            public AttachVirtualDiskVersion Version;
            public AttachVirtualDiskParametersVersion1 Version1;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct AttachVirtualDiskParametersVersion1 {
            public ulong Reserved;
        }

        private enum VirtualDiskAccessMask {
            VirtualDiskAccessNone = 0x00000000,
            VirtualDiskAccessAttachRo = 0x00010000,
            VirtualDiskAccessAttachRw = 0x00020000,
            VirtualDiskAccessDetach = 0x00040000,
            VirtualDiskAccessGetInfo = 0x00080000,
            VirtualDiskAccessCreate = 0x00100000,
            VirtualDiskAccessMetaops = 0x00200000,
            VirtualDiskAccessRead = 0x000d0000,
            VirtualDiskAccessAll = 0x003f0000,
            VirtualDiskAccessWritable = 0x00320000
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct VirtualStorageType {
            public ulong DeviceId;
            public Guid VendorId;
        }


        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 AttachVirtualDisk(IntPtr virtualDiskHandle, IntPtr securityDescriptor, AttachVirtualDiskFlag flags, Int32 providerSpecificFlags,
                                                     ref AttachVirtualDiskParameters parameters, IntPtr overlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr DetachVirtualDisk(IntPtr handle, DetachVirtualDiskFlag flags, ulong providerSpeficicFlags);

        [DllImport("virtdisk.dll", CharSet = CharSet.Unicode)]
        private static extern Int32 OpenVirtualDisk(VirtualStorageType virtualStorageType, string path, VirtualDiskAccessMask virtualDiskAccessMask,
                                                   OpenVirtualDiskFlag flags, IntPtr parameters, ref IntPtr handle);
    }
}
