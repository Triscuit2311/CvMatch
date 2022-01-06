using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenCvSharp;

namespace CvWindowScanner.Modules
{
    public class GameObject
    {
        private bool _requested = false;
        private CvSearch.WindowRegion _reqRegion = default;
        private Mat _template;
        private double _threshold;
        public Point ScreenLocation;
        public Point WindowLocation;

        public GameObject(Mat template, double threshold)
        {
            _template = template;
            _threshold = threshold;
        }

        public bool FindObject(CvSearch.WindowRegion region, out Point pt,
            double threshold = -1, int waitTime = 500)
        {
            if (threshold < 0) threshold = _threshold;
            return CvSearch.FindImageOnCaptureWindowRegion(region, _template, threshold, out pt);
        }
        
    }
}