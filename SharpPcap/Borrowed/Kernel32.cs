using System;
using System.Runtime.InteropServices;

namespace SharpPcap.Borrowed
{
#if Windows
    internal static class Kernel32
    {
        internal static int OverlappedSize = IntPtr.Size * 3 + 8;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern SafeLocalAllocHandle LocalAlloc(int uFlags, UIntPtr sizetdwBytes);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr handle);
    }
#endif
}