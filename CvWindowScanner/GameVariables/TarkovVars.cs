﻿using System.Collections.Generic;
using CvWindowScanner.Modules;
using OpenCvSharp;

namespace CvWindowScanner.GameVariables
{
    public static class TarkovVars
    {
        public static List<GameState> GameStates = default;
        public static void  SetupStates()
        {

            GameStates = new List<GameState>
            {
                new GameState(
                    "In Game",
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
                    CvSearch.WindowRegion.LowerLeft,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\main_menu_tab_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "Inventory",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\inventory_tab_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "PMC Select -> PMC Selected",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\pmc_selected_button.bmp")
                    },
                    0.9),
                new GameState(
                    "PMC Select -> SCAV Selected",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\scav_selected_button.bmp")
                    },
                    0.9),
                new GameState(
                    "Map Select -> Factory Selected",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\factory_text_highlighted.bmp")
                    },
                    0.9),
                new GameState(
                    "Map Select -> Factory Unselected",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\factory_text_unselected.bmp")
                    },
                    0.9),
                new GameState(
                    "Loading raid -> Loading",
                    CvSearch.WindowRegion.UpperCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\loading_raid_deploying_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Loading raid -> Starting",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\loading_raid_get_ready_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Post Raid -> Raid Ended",
                    CvSearch.WindowRegion.LowerCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\post_raid_lost_items_text.bmp")
                    },
                    0.9),
                new GameState(
                    "Post Raid -> Confirm Return to Main Menu",
                    CvSearch.WindowRegion.MiddleCenter,
                    new List<Mat>
                    {
                        Cv2.ImRead(
                            "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\proceed_to_main_menu_text.bmp")
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
                }
            };

            // public GameObject go = new GameObject(
            //     "Grenade",
            //     CvSearch.WindowRegion.FullWindow,
            //     0.8,
            //     Cv2.ImRead(
            //         "C:\\Users\\trisc\\RiderProjects\\TarkovTests\\CvWindowScanner\\img\\Tarkov\\f1_grenade.bmp")
            // );

    }
}