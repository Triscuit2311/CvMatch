﻿using System;
using System.Collections.Generic;
using System.Threading;
using CvWindowScanner.Utils;
using OpenCvSharp;
using Point = OpenCvSharp.Point;


namespace CvWindowScanner.Modules
{
    public static class WindowScanner
    {
        private static Thread _scanThread;
        private static bool _threadStopFlag;
        public static bool Initialized { get; private set; }
        private static List<ScanData> _stateQueue = new List<ScanData>();

        private static string _windowTitle;
        private static IntPtr _windowPtr = IntPtr.Zero;
        public static Point WindowPosition = new Point();
        
        

        private class ScanData
        {
            public Mat Template;
            public double Threshold;
            public readonly CvSearch.WindowRegion Region;
            private readonly Action<bool, Point> _callback;

            public ScanData(Mat template, CvSearch.WindowRegion region, double threshold, Action<bool, Point> callback)
            {
                Region = region;
                Template = template;
                _callback = callback;
                Threshold = threshold;
            }

            public void PerformCallback(bool success, Point screenPosition)
            {
                _callback(success, screenPosition);
            }
            
        }

        /// <summary>
        /// Pushes new scan to state scanning queue.
        /// </summary>
        /// <param name="template">Template image to scan for.</param>
        /// <param name="region">Window region to scan for this template.</param>
        /// <param name="threshold">Threshold at which match must meet to succeed.</param>
        /// <param name="callback">Success flag and Screen Position captured for callback.</param>
        /// <exception cref="Exception">Throw if window scanner needs to be initialized. Use Init().</exception>
        public static void PushToStateQueue( Mat template, CvSearch.WindowRegion region,
            double threshold, Action<bool, Point> callback)
        {
            if (!Initialized)
                throw new Exception("WindowScanner needs to be initialized.");
            
            _stateQueue.Add(new ScanData(
                template,
                region,
                threshold,
                callback));
        }

        /// <summary>
        /// Spawns the scan thread.
        /// </summary>
        public static void Init(string windowTitle)
        {
            _windowTitle = windowTitle;


            UpdateWindowPointer();
            
            if (!Initialized)
            {
                _scanThread = new Thread(new ThreadStart(ScanThreadOperation));
                _scanThread.Start();
            }
            _stateQueue.Clear();

            Initialized = true;
        }
        
        /// <summary>
        /// Stops the scan thread.
        /// </summary>
        public static void Stop()
        {
            Initialized = false;
            _threadStopFlag = true;
            _scanThread.Join();
        }

        public static void UpdateWindowPointer()
        {
            while (!Natives.GetHwnd(_windowTitle,_windowPtr, out _windowPtr, true))
            {
                Console.WriteLine($"Window [{_windowTitle}] Not found trying again in 5 seconds.");
                Thread.Sleep(5000);
            }
        }
        public static bool UpdateWindow()
        {
            var flag = CvSearch.UpdateWindowCaptureLocation(_windowPtr, out WindowPosition);
            return flag;
        }

        private static void ScanThreadOperation()
        {
            DXGICapturer.Init();
            
            //todo: add UpdateWindow() so we may update the window externally at discretion
            UpdateWindow();
               
            while (true)
            {
                if (BotControls.GlobalPause)
                {
                    Thread.Sleep(100);
                    continue;
                }
                
                if (_threadStopFlag) break;
                if (_stateQueue.Count <= 0){ Thread.Sleep(100); continue;}

                CvSearch.Refresh();
                for (var i = 0; i <= _stateQueue.Count-1; i++)
                {

                        var flag = CvSearch.FindImageOnCaptureWindowRegion(
                            _stateQueue[i].Region,
                            _stateQueue[i].Template,
                            _stateQueue[i].Threshold,
                            out var loc);

                        // adjust location to be screen-relative
                        loc += WindowPosition;
                        _stateQueue[i].PerformCallback(flag, loc);

                }
                

            }
            
            DXGICapturer.Stop();
        }
        
    }
}