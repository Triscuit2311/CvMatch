using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;


namespace CvWindowScanner.Utils
{
    public class Natives
    {
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref N_RECT rect);

        private struct N_RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        public static Rectangle GetWindowRect(IntPtr hwnd)
        {
            N_RECT nativeRect = default;
            GetWindowRect(hwnd, ref nativeRect);

            return new Rectangle(
                (int)nativeRect.left,
                (int)nativeRect.top,
                (int)nativeRect.right - (int)nativeRect.left,
                (int)nativeRect.bottom - (int)nativeRect.top);
        }
        

        public static IntPtr GetHwnd(string title, bool showinfo = false)
        {
            foreach (Process window in Process.GetProcesses())
            {
                window.Refresh();
                if (window.MainWindowHandle != IntPtr.Zero && window.MainWindowTitle.ToLower().Contains(title.ToLower()))
                {
                    if(showinfo)
                        Console.WriteLine($"[{window.MainWindowTitle}] -> [0x{window.MainWindowHandle.ToInt32():X}]");
                    return window.MainWindowHandle;
                }
            }
            return IntPtr.Zero;
        }
        
        public static double GetWindowsScaling()
        {
            return Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
        }
        

    }
}