using System.Runtime.InteropServices;

namespace ZapretMouseMacro
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MSG
    {
        public System.IntPtr hwnd;
        public uint message;
        public System.IntPtr wParam;
        public System.IntPtr lParam;
        public uint time;
        public POINT pt;
    }
}
