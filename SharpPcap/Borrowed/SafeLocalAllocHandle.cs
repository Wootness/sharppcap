using System;
using System.Runtime.InteropServices;

namespace SharpPcap.Borrowed
{
#if Windows
    internal sealed class SafeLocalAllocHandle : SafeBuffer, IDisposable
    {
        internal static readonly SafeLocalAllocHandle Zero = new SafeLocalAllocHandle();

        private SafeLocalAllocHandle()
            : base(true)
        {
        }

        internal static SafeLocalAllocHandle LocalAlloc(int cb)
        {
            SafeLocalAllocHandle localAllocHandle = Kernel32.LocalAlloc(0, (UIntPtr)((ulong)cb));
            if (localAllocHandle.IsInvalid)
            {
                localAllocHandle.SetHandleAsInvalid();
                throw new OutOfMemoryException();
            }
            return localAllocHandle;
        }

        internal SafeLocalAllocHandle(IntPtr handle)
            : base(true)
        {
            this.SetHandle(handle);
        }

        internal static SafeLocalAllocHandle InvalidHandle
        {
            get
            {
                return new SafeLocalAllocHandle(IntPtr.Zero);
            }
        }

        protected override bool ReleaseHandle()
        {
            return Kernel32.LocalFree(this.handle) == IntPtr.Zero;
        }
    }
#endif
}