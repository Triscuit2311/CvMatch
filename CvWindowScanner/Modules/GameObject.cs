using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenCvSharp;

namespace CvWindowScanner.Modules
{
    public class GameObject
    {
        private Mat _template;
        private readonly CvSearch.WindowRegion _scanRegion;
        private readonly double _threshold;
        public readonly string Name;
        public Point LastLocationScreen;
        public Point LastLocationWindow;
        public bool Found;
        private bool _keepTracking;


        public GameObject(string name, CvSearch.WindowRegion scanRegion,
            double threshold, Mat template)
        {
            _threshold = threshold;
            _template = template;
            Name = name;
            _scanRegion = scanRegion;
        }

        public void KeepTracking()
        {
            if (_keepTracking)
                return;
            _keepTracking = true;

            WindowScanner.PushToQueue(
                true,
                _template,
                _scanRegion,
                _threshold,
                (b, p) =>
                {
                    Found = b;
                    LastLocationScreen = p;
                    LastLocationWindow = p - WindowScanner.WindowPosition;
                });
        }

        public void FindPassive()
        {
            if (_keepTracking)
                return;
            WindowScanner.PushToQueue(
                false,
                _template,
                _scanRegion,
                _threshold,
                (b, p) =>
                {
                    Found = b;
                    LastLocationScreen = p;
                    LastLocationWindow = p - WindowScanner.WindowPosition;
                });
        }
        
        public bool WaitForFind(int msTimeout)
        {
            for (var i = 0; i < msTimeout/100; i++)
            {
                if (!_keepTracking)
                {
                    WindowScanner.PushToQueue(
                        false,
                        _template,
                        _scanRegion,
                        _threshold,
                        (b, p) =>
                        {
                            Found = b;
                            LastLocationScreen = p;
                            LastLocationWindow = p - WindowScanner.WindowPosition;
                        });
                }
                Thread.Sleep(100);
                if (Found)
                    return true;
            }

            return false;
        }
        
    }
}