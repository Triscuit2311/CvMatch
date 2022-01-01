using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Drawing.Configuration;
using System.Threading;
using CvWindowScanner.Utils;
using OpenCvSharp;
using Point = OpenCvSharp.Point;


namespace CvWindowScanner
{
    public static class WindowScanner
    {
        private static Thread _scanThread;
        private static bool _threadStopFlag;
        public static bool Initialized { get; private set; }
        private static List<ScanData> _scanQueue = new List<ScanData>();
        private static string _windowTitle;
        private static IntPtr _windowPtr = IntPtr.Zero;
        
        

        private class ScanData
        {
            public Mat Template;
            public double Threshold;
            public readonly CvSearch.WindowRegion Region;
            private readonly Action<bool, Point> _callback;
            public bool Preserve = false;

            public ScanData(bool preserve, Mat template, CvSearch.WindowRegion region, double threshold, Action<bool, Point> callback)
            {
                Preserve = preserve;
                Region = region;
                Template = template;
                _callback = callback;
                this.Threshold = threshold;
            }

            public void PerformCallback(bool success, Point screenPosition)
            {
                _callback(success, screenPosition);
            }
            
        }

        /// <summary>
        /// Pushes new scan to scanning queue.
        /// </summary>
        /// <param name="preserve">If true, scan data will persist. Otherwise scan will be run once and then removed from scan queue.</param>
        /// <param name="template">Template image to scan for.</param>
        /// <param name="region">Window region to scan for this template.</param>
        /// <param name="threshold">Threshold at which match must meet to succeed.</param>
        /// <param name="callback">Success flag and Screen Position captured for callback.</param>
        /// <exception cref="Exception">Throw if window scanner needs to be initialized. Use Init().</exception>
        public static void PushToQueue(bool preserve, Mat template, CvSearch.WindowRegion region,double threshold, Action<bool, Point> callback)
        {
            if (!Initialized)
                throw new Exception("WindowScanner needs to be initialized.");
            
            _scanQueue.Add(new ScanData(
                preserve,
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

            if (!Natives.GetHwnd(_windowTitle, out _windowPtr, true)) return;


            Initialized = true;
            _scanThread = new Thread(new ThreadStart(ScanThreadOperation));
            _scanThread.Start();
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

        private static void ScanThreadOperation()
        {
            DXGICapturer.Init();
            
            //todo: add UpdateWindow() so we may update the window externally at discretion
            CvSearch.UpdateWindowCaptureLocation(_windowPtr, out var windowPosition);
               
            while (true)
            {
                if (_threadStopFlag) break;
                if (_scanQueue.Count <= 0){ Thread.Sleep(100); continue;}

                CvSearch.Refresh();
                for (var i = 0; i <= _scanQueue.Count-1; i++)
                {
                    var flag = CvSearch.FindImageOnCaptureWindowRegion(
                        _scanQueue[i].Region,
                        _scanQueue[i].Template,
                        _scanQueue[i].Threshold,
                        out var loc);
                    
                    //todo: adjust location to be screen-relative
                    _scanQueue[i].PerformCallback(flag, loc);
                    
                    if(!_scanQueue[i].Preserve)
                        _scanQueue.RemoveAt(0);
                }
                

            }
            
            DXGICapturer.Stop();
        }
        
    }
}