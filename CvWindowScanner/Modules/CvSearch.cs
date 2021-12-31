using System;
using OpenCvSharp;
using System.Windows.Forms;
using CvWindowScanner.Utils;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using Rectangle = System.Drawing.Rectangle;

namespace CvWindowScanner
{
    public static class CvSearch
    {
        private static Mat _lastFrame = new Mat();
        static Mat GetLastScreenAsMat()
        {
            return DXGICapturer.GetLast().ToMat().CvtColor(ColorConversionCodes.BGRA2BGR);
        }

        /// <summary>
        /// Window Regions for use with FindImageOnCaptureWindowRegion(...).
        /// </summary>
        public enum WindowRegion
        {
            FullWindow,
            UpperLeft,
            UpperCenter,
            UpperRight,
            MiddleLeft,
            MiddleCenter,
            MiddleRight,
            LowerLeft,
            LowerCenter,
            LowerRight,
            LeftHalf,
            RightHalf,
            UpperHalf,
            LowerHalf,
            LeftThird,
            CenterThird,
            RightThird
        }

        private class TemplateMatchResults
        {
            public OpenCvSharp.Point Loc;
            public bool Success;

            public TemplateMatchResults(bool success, Point loc)
            {
                Success = success;
                Loc = loc;
            }
        }
        private static Rectangle GetGameWindow(string title)
        {
            return ToScreenClampedRectangle(
                Natives.GetWindowRect(
                    Natives.GetHwnd(title)));  
        }
        private static Rectangle ToScreenClampedRectangle(Rectangle rect)
        {
            return new Rectangle(
                rect.Left.Clamp(1,Screen.PrimaryScreen.Bounds.Width-2),
                rect.Top.Clamp(1,Screen.PrimaryScreen.Bounds.Height-2),
                rect.Width.Clamp(1, (int)Screen.PrimaryScreen.Bounds.Width - rect.Left - 1),
                rect.Height.Clamp(1,(int)Screen.PrimaryScreen.Bounds.Height - rect.Top -1));
        }
        private static Mat GetLastFrameRegion(WindowRegion region, out Point offsets)
        {
            Rectangle area = new Rectangle(0,0,_lastFrame.Width,_lastFrame.Height);
            
            switch (region)
            {
                case WindowRegion.FullWindow:
                    offsets = new Point(0, 0);
                    return _lastFrame;
                case WindowRegion.UpperLeft   :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.UpperCenter :
                    area.X = _lastFrame.Width / 3;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.UpperRight  :
                    area.X = (_lastFrame.Width / 3)*2;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.MiddleLeft  :
                    area.X = 0;
                    area.Y = _lastFrame.Height / 3;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.MiddleCenter:
                    area.X = _lastFrame.Width / 3;
                    area.Y = _lastFrame.Height / 3;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.MiddleRight :
                    area.X = (_lastFrame.Width / 3)*2;
                    area.Y = _lastFrame.Height / 3;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.LowerLeft   :
                    area.X = 0;
                    area.Y = (_lastFrame.Height / 3)*2;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.LowerCenter :
                    area.X = _lastFrame.Width / 3;
                    area.Y = (_lastFrame.Height / 3)*2;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.LowerRight  :
                    area.X = (_lastFrame.Width / 3)*2;
                    area.Y = (_lastFrame.Height / 3)*2;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height / 3;
                    break;
                case WindowRegion.LeftHalf    :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 2;
                    area.Height = _lastFrame.Height;
                    break;
                case WindowRegion.RightHalf   :
                    area.X = _lastFrame.Width / 2;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 2;
                    area.Height = _lastFrame.Height;
                    break;
                case WindowRegion.UpperHalf   :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = _lastFrame.Width;
                    area.Height = _lastFrame.Height / 2;
                    break;
                case WindowRegion.LowerHalf   :
                    area.X = 0;
                    area.Y = _lastFrame.Height / 2;
                    area.Width = _lastFrame.Width;
                    area.Height = _lastFrame.Height / 2;
                    break;
                case WindowRegion.LeftThird   :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height;
                    break;
                case WindowRegion.CenterThird :
                    area.X = _lastFrame.Width / 3;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height;
                    break;
                case WindowRegion.RightThird  :
                    area.X = (_lastFrame.Width / 3)*2;
                    area.Y = 0;
                    area.Width = _lastFrame.Width / 3;
                    area.Height = _lastFrame.Height;
                    break;
                default:
                    break;
            }

            offsets = new Point(area.X, area.Y);
           return DXGICapturer.CropAtRect(_lastFrame.ToBitmap(), area)
               .ToMat()
               .CvtColor(ColorConversionCodes.BGRA2BGR);
        }

        /// <summary>
        /// Refreshes the last frame the capturer.
        /// Call before attempting a scan.
        /// </summary>
        public static void Refresh()
        {
            _lastFrame = GetLastScreenAsMat();
        }
        private static TemplateMatchResults TemplateMatchWithThreshold(Mat img, Mat template, double threshold)
        {
            Mat res = new Mat();
            Cv2.MatchTemplate(img,template,res,TemplateMatchModes.CCoeffNormed);
            Cv2.Threshold(res, res, threshold, 1.0, ThresholdTypes.Tozero);

            Cv2.MinMaxLoc(res,
                out double minVal,
                out double maxVal,
                out OpenCvSharp.Point minLoc,
                out OpenCvSharp.Point maxLoc);
            
           
            if (maxVal >= threshold)
            {

                return new TemplateMatchResults(true, maxLoc);
            }

            return new TemplateMatchResults(false, minLoc);
        }

        /// <summary>
        /// Sets up the capturer with a chosen window as it's target.
        /// Call each time the window location needs to be updated.
        /// </summary>
        /// <param name="title">Title of the window to capture.</param>
        /// <param name="origin">(out) The X,Y origin of the window, for calculations.</param>
        public static void UpdateWindowCaptureLocation(string title, out System.Drawing.Point origin)
        {
            var area = GetGameWindow(title);
            origin = area.Location;
            DXGICapturer.SetCaptureRect(area);
        }

        /// <summary>
        /// Scans entire target window for template.
        /// </summary>
        /// <param name="template">Template image to search for.</param>
        /// <param name="threshold">Match threshold</param>
        /// <param name="loc">Window origin relative coordinates of match. (0,0) on failure.</param>
        /// <returns>true if match was found within threshold.</returns>
        public static  bool FindImageOnCaptureWindow(Mat template, double threshold, out Point loc)
        {
            var res = TemplateMatchWithThreshold(_lastFrame, template, threshold);
            loc = res.Loc;
            return res.Success;
        }

        /// <summary>
        /// Scans region of target window for template.
        /// </summary>
        /// <param name="region">Area of window to search. Use of BitmapRegion.FullWindow allowed.</param>
        /// <param name="template">Template image to search for.</param>
        /// <param name="threshold">Match threshold</param>
        /// <param name="loc">Window origin relative coordinates of match. (0,0) on failure.</param>
        /// <returns>true if match was found within threshold.</returns>
        public static bool FindImageOnCaptureWindowRegion(WindowRegion region, Mat template,
            double threshold, out Point loc)
        {
            var res = TemplateMatchWithThreshold(
                GetLastFrameRegion(region, out var offsets),
                template, threshold);
            loc = res.Loc + offsets;
            return res.Success;
        }
    }
}