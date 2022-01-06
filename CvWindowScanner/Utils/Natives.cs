using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;


namespace CvWindowScanner.Utils
{
    public class Natives
    {
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, ref NRect rect);
        
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        private struct NRect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public NRect(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
        }

        public static Rectangle GetWindowRect(IntPtr hwnd)
        {
            NRect nativeRect = default;
            GetWindowRect(hwnd, ref nativeRect);

            return new Rectangle(
                nativeRect.left,
                nativeRect.top,
                nativeRect.right - nativeRect.left,
                nativeRect.bottom - nativeRect.top);
        }
        

        public static bool GetHwnd(string title,out IntPtr ptr, bool showinfo = false)
        {
            foreach (var window in Process.GetProcesses())
            {
                window.Refresh();
                if (window.MainWindowHandle != IntPtr.Zero && window.MainWindowTitle.ToLower().Contains(title.ToLower()))
                {
                    if (showinfo)
                    {
                        Console.WriteLine($"[{window.MainWindowTitle}] -> [0x{window.MainWindowHandle.ToInt32():X}]");
                        
                        
                    }

                    ptr = window.MainWindowHandle;
                    return true;
                }
            }
            Console.WriteLine($"Window with title [{title}] not found. Windows available:");
            foreach (var window in Process.GetProcesses())
            {
                if (window.MainWindowHandle != IntPtr.Zero)
                {
                    Console.WriteLine($"\t{window.MainWindowTitle}");
                }
            }

            ptr = IntPtr.Zero;
            return false;
        }
        
        // public static double GetWindowsScaling()
        // {
        //     return Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
        // }
        

    }
    
    
    public class WindowHandleInfo
    {
        private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr lParam);

        private IntPtr _MainHandle;

        public WindowHandleInfo(IntPtr handle)
        {
            this._MainHandle = handle;
        }

        public List<IntPtr> GetAllChildHandles()
        {
            List<IntPtr> childHandles = new List<IntPtr>();

            GCHandle gcChildhandlesList = GCHandle.Alloc(childHandles);
            IntPtr pointerChildHandlesList = GCHandle.ToIntPtr(gcChildhandlesList);

            try
            {
                EnumWindowProc childProc = new EnumWindowProc(EnumWindow);
                EnumChildWindows(this._MainHandle, childProc, pointerChildHandlesList);
            }
            finally
            {
                gcChildhandlesList.Free();
            }

            return childHandles;
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam)
        {
            GCHandle gcChildhandlesList = GCHandle.FromIntPtr(lParam);

            if (gcChildhandlesList == null || gcChildhandlesList.Target == null)
            {
                return false;
            }

            List<IntPtr> childHandles = gcChildhandlesList.Target as List<IntPtr>;
            childHandles.Add(hWnd);

            return true;
        }
    }
}