using System.Collections.Generic;
using CvWindowScanner.Modules;
using OpenCvSharp;

namespace CvWindowScanner.GameVariables
{
    public class NotepadVars
    {
         public static List<GameState> GetStates()
         {
             return new List<GameState>
             {
                 new GameState(
                     "AFK Notification",
                     "AFK",
                     CvSearch.WindowRegion.FullWindow,                        
                     new List<Mat>
                     {
                         Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\a_on_screen.bmp")
                     },
                 0.8),
             };
         }

         public static void  Setup()
        {

        }
         
    }
}