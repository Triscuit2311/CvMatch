using System.Collections.Generic;
using CvWindowScanner.Modules;
using OpenCvSharp;

namespace CvWindowScanner.GameVariables
{
    public static class TarkovVars
    {
        public static List<GameState> GameStates = default;

        public static List<GameState> GetStates()
        {
            return GameStates;
        }

        public static void  Setup()
        {
            GameStates = new List<GameState>
            {
                new GameState(
                    "AFK Notification",
                    "AFK",
                    CvSearch.WindowRegion.FullWindow,                        
                    new List<Mat>
                    {
                        Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\AFK_title.bmp")
                    },
                    0.85, priority:2),
                new GameState(
                    "In Game",
                    "INGAME",
                    CvSearch.WindowRegion.UpperHalf,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\ingame_v_slot_marker.bmp"),
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\ingame_1_slot_marker.bmp"),
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\ingame_2_slot_marker.bmp")
                    },
                    0.9),
                new GameState(
                    "Main Menu",
                    "MAINMENU",
                    CvSearch.WindowRegion.LowerLeft,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\main_menu_tab_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "Inventory",
                    "INVENTORY",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\inventory_tab_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "PMC Select -> PMC Selected",
                    "PMCSELECTED",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\pmc_selected_button.bmp")
                    },
                    0.9),
                new GameState(
                    "PMC Select -> SCAV Selected",
                    "SCAVSELECTED",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\scav_selected_button.bmp")
                    },
                    0.9),
                new GameState(
                    "Map Select -> Factory Selected",
                    "FACTORYSELECTED",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\factory_text_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "Map Select -> Factory Unselected",
                    "FACTORYUNSELECTED",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\factory_text_unselected.bmp")
                    },
                    0.9),
                new GameState(
                    "Loading raid -> Loading",
                    "LOADINGRAID",
                    CvSearch.WindowRegion.UpperCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\loading_raid_deploying_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Loading raid -> Starting",
                    "STARTINGRAID",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\loading_raid_get_ready_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Post Raid -> Raid Ended",
                    "RAIDEND",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\post_raid_lost_items_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Post Raid -> Confirm Return to Main Menu",
                    "CONFIRMMAINMENURETURN",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\proceed_to_main_menu_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Left Raid -> Select Escape Screen",
                    "ESCAPEAFTERLEAVING",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\PMC_still_in_raid.bmp")
                    },
                    0.9),
                new GameState(
                    "Left Raid -> Reconnect",
                    "RECONNECTTORAID",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\tried_to_leave_dc.bmp")
                    },
                    0.9)
            };

        }

        public static Dictionary<string, Point> ConstantPosOffsets = new Dictionary<string, Point>
            {
                {
                    "GameAreaRoot",
                    new Point(15, 50)
                },
                {
                    "ExitAfkMessage",
                    new Point(524, 455)
                },
                {
                    "EscapeFromTarkovButton",
                    new Point(513, 586)
                },
                {
                    "PMCSelectButton",
                    new Point(643, 608)
                },
                {
                    "PMCSelectNextButton",
                    new Point(516, 742)
                },
                {
                    "FactoryMapSelect",
                    new Point(369,390)
                },
                {
                    "MapSelectReadyButton",
                    new Point(691, 783)
                },
                {
                    "ReturnToMainMenuButtonPostRaid",
                    new Point(524, 789)
                },
                {
                    "ConfrimReturnToMainMenu",
                    new Point(470, 461)
                },               
                {
                    "PocketOneLocation",
                    new Point(394, 211)
                },
                {
                    "InventoryButtonLocation",
                    new Point(525, 635)
                },
                {
                    "ReconnectButtonLocation",
                    new Point(525, 719)
                },
                
            };

        public static Dictionary<string, GameObject> GameObjects = new Dictionary<string, GameObject>
        {
            {
                "F1Grenade",
            new GameObject(
                Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\f1_grenade.bmp"),
                    0.85)
            },
            {
                "RGD5Grenade",
                new GameObject(
                    Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\rgd5_grenade.bmp"),
                    0.85)
            },
            {
                "VOG25Grenade",
                new GameObject(
                    Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\vog25_grenade.bmp"),
                    0.85)
            },
            {
                "VOG17Grenade",
                new GameObject(
                    Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\vog17_grenade.bmp"),
                    0.85)
            }, 
            {
                "M67Grenade",
                new GameObject(
                    Cv2.ImRead("C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\m67_grenade.bmp"),
                    0.85)
            },
       };

    }
}