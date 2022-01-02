using System;
using System.Collections.Generic;
using System.Threading;
using CvWindowScanner.Modules;
using CvWindowScanner.Utils;
using OpenCvSharp;


namespace CvWindowScanner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            var bitcoinTemplate = 
                Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\template.bmp");

            WindowScanner.Init("Untitled - Notepad");
            
            while (!WindowScanner.Initialized)
            {
                Thread.Sleep(100); 
            }

            GameState bitcoinPresent = new GameState(
                CvSearch.WindowRegion.FullWindow,
                new List<Mat> {bitcoinTemplate},
                0.7);

            while (WindowScanner.Initialized)
            {
                break;
            }
            
            DebugScreenShotter.Setup();
            
            
            Console.ReadLine();
            
            WindowScanner.Stop();
        }
        

    }
    
  
     
     
}