using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using OpenCvSharp;

namespace CvWindowScanner.Modules
{
    public class GameState
    {
        private class Indicator
        {
            public readonly Mat Template;
            public bool FoundFlag;

            public Indicator( Mat template, bool foundFlag)
            {
                Template = template;
                FoundFlag = foundFlag;
            }
        }
        
        
        private readonly List<Indicator> _indicators;
        private readonly List<Indicator> _exemptions;
        private readonly CvSearch.WindowRegion _scanRegion;
        private readonly double _threshold;
        public readonly string Name;
        public Point LastLocationScreen;
        public Point LastLocationWindow;
        public readonly int Priority;
        public bool State => _indicators.All(indicator => indicator.FoundFlag);
        

        public GameState(string name, CvSearch.WindowRegion scanRegion, List<Mat> indices,
            double threshold, List<Mat> exemptions = default, int priority = 1)
        {
            _threshold = threshold;
            Priority = priority;
            Name = name;
            _scanRegion = scanRegion;
            _indicators = new List<Indicator>();
            
            InitIndicators(indices);
            
            if (exemptions != null && exemptions.Count > 0)
            {
                _exemptions = new List<Indicator>();
                InitExemptions(exemptions);
            }
            PushIndicatorsToScanQueue();
        }

        private void PushIndicatorsToScanQueue()
        {
            foreach (var indicator in _indicators)
            {
                WindowScanner.PushToQueue(
                    true,
                    indicator.Template,
                    _scanRegion,
                    _threshold,
                    (b, p) =>
                    {
                        LastLocationScreen = p;
                        LastLocationWindow = p - WindowScanner.WindowPosition;
                        indicator.FoundFlag = b;
                    } );
            }

            if (_exemptions is null || _exemptions.Count <= 0) return;
            foreach (var indicator in _exemptions)
            {
                WindowScanner.PushToQueue(
                    true,
                    indicator.Template,
                    _scanRegion,
                    _threshold,
                    (b, p) => indicator.FoundFlag = false);
            }
        }

        private void InitIndicators(List<Mat> indices)
        {
            foreach (var mat in indices)
            {
                _indicators.Add(new Indicator(mat, false));
            }
        }
        
        private void InitExemptions(List<Mat> indices)
        {
            foreach (var mat in indices)
            {
                _exemptions.Add(new Indicator(mat, false));
            }
        }
    }
}