using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using CvWindowScanner.Modules;
using CvWindowScanner.Utils;
using OpenCvSharp;


namespace CvWindowScanner
{
    internal class Program
    {

        //add wait for state
        public static void Main(string[] args)
        {
            
            
            WindowScanner.Init("EscapeFromTarkov");

            
            while (!WindowScanner.Initialized)
            {
                Thread.Sleep(100); 
            }
            
            List<GameState> gameStates = new List<GameState>{

                new GameState(
                    "In Game",
                    CvSearch.WindowRegion.UpperHalf,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\ingame_v_slot_marker.bmp"),
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\ingame_1_slot_marker.bmp"),
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\ingame_2_slot_marker.bmp")
                    },
                    0.9),
                new GameState(
                    "Main Menu",
                    CvSearch.WindowRegion.LowerLeft,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\main_menu_tab_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "Inventory",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\inventory_tab_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "PMC Select -> PMC Selected",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\pmc_selected_button.bmp")
                    },
                    0.9),
                new GameState(
                    "PMC Select -> SCAV Selected",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\scav_selected_button.bmp")
                    },
                    0.9),
                new GameState(
                "Map Select -> Factory Selected",
                CvSearch.WindowRegion.MiddleCenter,
                new List<Mat>
                {
                    Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\factory_text_highlighted.bmp")
                },
                0.9),
                new GameState(
                    "Map Select -> Factory Unselected",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\factory_text_unselected.bmp")
                    },
                    0.9),
                new GameState(
                    "Loading raid -> Loading",
                    CvSearch.WindowRegion.UpperCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\loading_raid_deploying_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Loading raid -> Starting",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\loading_raid_get_ready_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Post Raid -> Raid Ended",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\post_raid_lost_items_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Post Raid -> Confirm Return to Main Menu",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\proceed_to_main_menu_text.bmp")
                    },
                    0.9)
            };

            GameObject go = new GameObject(
                "Grenade",
                CvSearch.WindowRegion.FullWindow,
                0.8,
                Cv2.ImRead(
                    "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\f1_grenade.bmp")
            );

           
            var lastState = "";
            int counter = 0;
            while (WindowScanner.Initialized)
            {
                counter++;
                
                Thread.Sleep(100);
                var flag = false;
                var currState = "Unknown";
                foreach (var state in gameStates.Where(state => state.State))
                {
                    currState = state.Name;
                    flag = true;
                }

                if (!flag)
                    currState = "Unknown";

                // if (go.WaitForFind(100))
                // {
                //    // SetCursorPos(WindowScanner.WindowPosition);
                // }


                if (currState == lastState) continue;
                lastState = currState;
                
                Console.WriteLine($"[{counter}] State: {currState}");


            }
            
           // DebugScreenShotter.Setup();
            
            Console.ReadLine();
            WindowScanner.Stop();
        }


    }
    
  
     
     
}