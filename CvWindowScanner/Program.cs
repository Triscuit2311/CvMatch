using System;
using System.Threading;
using OpenCvSharp;


namespace CvWindowScanner
{
    internal class Program
    {
        private static string _currentState = "";

        public static void Main(string[] args)
        {
            
            var template = Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\template.bmp");

            WindowScanner.Init("Untitled - Notepad");
            
            while (!WindowScanner.Initialized)
            {
                Thread.Sleep(100); 
            }
            
            
            // Preserved scan
            WindowScanner.PushToQueue(
                true,
                template,
                CvSearch.WindowRegion.FullWindow,
                0.7,
                (b,p)=> _currentState =(b? "Inventory" : "Unknown"));

            while (WindowScanner.Initialized)
            {
                Console.WriteLine($"State: {_currentState}");
                Thread.Sleep(1000);
            }
            
            
            WindowScanner.Stop();


        }
        
        
    }
    
  
     
     
}