using System.Collections.Generic;

namespace CvWindowScanner.Modules
{
    public static class GameObjectManager
    {
        private static List<GameObject> _objectQueue;

        public static void CheckObjects()
        {
            if (_objectQueue is null) return;
            if (_objectQueue.Count <= 0) return;

            foreach (var obj in _objectQueue)
            {
               // var flag = CvSearch.FindImageOnCaptureWindowRegion()
            }
        }
    }
}