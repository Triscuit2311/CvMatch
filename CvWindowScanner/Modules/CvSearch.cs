using System;
using System.Runtime.Remoting.Messaging;
using OpenCvSharp;
using System.Windows.Forms;
using CvWindowScanner.Modules;
using CvWindowScanner.Utils;
using OpenCvSharp.Extensions;
using Point = OpenCvSharp.Point;
using Rectangle = System.Drawing.Rectangle;

namespace CvWindowScanner
{
    public static class CvSearch
    {
        public static Mat LastFrame = new Mat();
        private static Mat GetLastScreenAsMat()
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
            Rectangle area = new Rectangle(0,0,LastFrame.Width,LastFrame.Height);
            
            switch (region)
            {
                case WindowRegion.FullWindow:
                    offsets = new Point(0, 0);
                    return LastFrame;
                case WindowRegion.UpperLeft   :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.UpperCenter :
                    area.X = LastFrame.Width / 3;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.UpperRight  :
                    area.X = (LastFrame.Width / 3)*2;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.MiddleLeft  :
                    area.X = 0;
                    area.Y = LastFrame.Height / 3;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.MiddleCenter:
                    area.X = LastFrame.Width / 3;
                    area.Y = LastFrame.Height / 3;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.MiddleRight :
                    area.X = (LastFrame.Width / 3)*2;
                    area.Y = LastFrame.Height / 3;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.LowerLeft   :
                    area.X = 0;
                    area.Y = (LastFrame.Height / 3)*2;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.LowerCenter :
                    area.X = LastFrame.Width / 3;
                    area.Y = (LastFrame.Height / 3)*2;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.LowerRight  :
                    area.X = (LastFrame.Width / 3)*2;
                    area.Y = (LastFrame.Height / 3)*2;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height / 3;
                    break;
                case WindowRegion.LeftHalf    :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 2;
                    area.Height = LastFrame.Height;
                    break;
                case WindowRegion.RightHalf   :
                    area.X = LastFrame.Width / 2;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 2;
                    area.Height = LastFrame.Height;
                    break;
                case WindowRegion.UpperHalf   :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = LastFrame.Width;
                    area.Height = LastFrame.Height / 2;
                    break;
                case WindowRegion.LowerHalf   :
                    area.X = 0;
                    area.Y = LastFrame.Height / 2;
                    area.Width = LastFrame.Width;
                    area.Height = LastFrame.Height / 2;
                    break;
                case WindowRegion.LeftThird   :
                    area.X = 0;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height;
                    break;
                case WindowRegion.CenterThird :
                    area.X = LastFrame.Width / 3;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height;
                    break;
                case WindowRegion.RightThird  :
                    area.X = (LastFrame.Width / 3)*2;
                    area.Y = 0;
                    area.Width = LastFrame.Width / 3;
                    area.Height = LastFrame.Height;
                    break;
                default:
                    break;
            }

            offsets = new Point(area.X, area.Y);
           return DXGICapturer.CropAtRect(LastFrame.ToBitmap(), area)
               .ToMat()
               .CvtColor(ColorConversionCodes.BGRA2BGR);
        }

        /// <summary>
        /// Refreshes the last frame the capturer.
        /// Call before attempting a scan.
        /// </summary>
        public static void Refresh()
        {
            LastFrame = GetLastScreenAsMat();
        }
        private static TemplateMatchResults TemplateMatchWithThreshold(Mat img, Mat template, double threshold)
        {
            if (img.Width == 0 || img.Height == 0 | template.Width == 0 || template.Height == 0)
            {
                return new TemplateMatchResults(false, new Point(0,0));
            }
            
            var res = new Mat();
            try
            {
                Cv2.MatchTemplate(img,template,res,TemplateMatchModes.CCoeffNormed);
                Cv2.Threshold(res, res, threshold, 1.0, ThresholdTypes.Tozero);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new TemplateMatchResults(false, new Point(0,0));
            }

            Cv2.MinMaxLoc(res,
                out double minVal,
                out double maxVal,
                out OpenCvSharp.Point minLoc,
                out OpenCvSharp.Point maxLoc);
            
            return new TemplateMatchResults(maxVal >= threshold, maxLoc);
        }

        /// <summary>
        /// Sets up the capturer with a chosen window as it's target.
        /// Call each time the window location needs to be updated.
        /// </summary>
        /// <param name="ptr">Window Handle Pointer.</param>
        /// <param name="origin">(out) The X,Y origin of the window, for calculations.</param>
        public static bool UpdateWindowCaptureLocation(IntPtr ptr, out Point origin)
        {
            var rect = ToScreenClampedRectangle(Natives.GetWindowRect(ptr));
            origin = new Point(rect.Location.X,rect.Location.Y);
            DXGICapturer.SetCaptureRect(rect);
            return true;
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
            var res = TemplateMatchWithThreshold(LastFrame, template, threshold);
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