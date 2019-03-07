using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SharpPcap.Borrowed
{
#if Windows
    internal static class HostInformationPal
    {
        private static object s_syncObject = new object();

        public static Interop.FIXED_INFO GetFixedInfo()
        {
            uint pOutBufLen = 0;
            Interop.FIXED_INFO fixedInfo = new Interop.FIXED_INFO();
            uint networkParams = Interop.GetNetworkParams(SafeLocalAllocHandle.InvalidHandle, ref pOutBufLen);
            while ((int)networkParams == 111)
            {
                SafeLocalAllocHandle pFixedInfo;
                using (pFixedInfo = Kernel32.LocalAlloc(0, (UIntPtr)pOutBufLen))
                {
                    if (pFixedInfo.IsInvalid)
                        throw new OutOfMemoryException();
                    networkParams = Interop.GetNetworkParams(pFixedInfo, ref pOutBufLen);
                    if ((int)networkParams == 0)
                        fixedInfo = Marshal.PtrToStructure<Interop.FIXED_INFO>(pFixedInfo.DangerousGetHandle());
                }
            }
            if ((int)networkParams != 0)
                throw new Win32Exception((int)networkParams);
            return fixedInfo;
        }
    }
#endif
}