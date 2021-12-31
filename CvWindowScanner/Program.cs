using System;
using System.Threading;
using OpenCvSharp;


namespace CvWindowScanner
{
    internal class Program
    {

        public static void Main(string[] args)
        {

            var template = Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\template.bmp");

            WindowScanner.Init();


            Console.WriteLine("Pushing..");
            
            // Preserve to get 
            WindowScanner.PushToQueue(
                true,
                template,
                CvSearch.WindowRegion.FullWindow,
                0.7,
                (b,p)=> Console.WriteLine($"[{(b?"":"NOT ")}FOUND]{(b? $" @ ({p.X}, {p.Y})":"")}"));
            
            
            Thread.Sleep(100000);
            
            
            WindowScanner.Stop();


        }
        
        
    }
    
  
     
     
}