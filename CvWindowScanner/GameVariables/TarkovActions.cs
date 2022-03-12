using System;
using System.Threading;
using CvWindowScanner.Modules;
using CvWindowScanner.Utils;

namespace CvWindowScanner.GameVariables
{
    public static class TarkovActions
    {
        public static void StopCycle(GameState currentState,
            int currentCycle, bool isNewState)
        {
            
            switch (currentState.Tag)
            {
                case "MAINMENU":
                    if (TarkovTrackers.NeedsGrenade && !TarkovTrackers.OutOfGrenades)
                    {
                        ClickInventoryButton();
                        break;
                    }
                    // TODO: DONT FORGET TO REENABLE THIS
                    ClickEscapeFromTarkovButton();
                    break;
                case "PMCSELECTED":
                    ClickNextAtPmcSelect();
                    break;
                case "SCAVSELECTED":
                    SelectPmc();
                    break;
                case "FACTORYSELECTED":
                    StartRaidFromMapSelect();
                    break;
                case "FACTORYUNSELECTED":
                    SelectFactoryMap();
                    break;
                case "INGAME":
                    if(isNewState)
                        SetCurrentRaidStartTime();
                    if (TarkovTrackers.HoldingGrenade &&
                        (DateTime.Now - TarkovTrackers.CurrentRaidStartTime).TotalSeconds >=
                        TarkovTrackers.TimeInRaidBeforeUseGrenade)
                    {
                        InputWrapper.QuickPress(InputWrapper.VK.KEY_C);
                        InputWrapper.MoveMouseRelative(0,1000);
                        InputWrapper.QuickPress(InputWrapper.VK.KEY_G);
                        Thread.Sleep(3000);
                        InputWrapper.ClickAtCurrent(rightClick:true);
                        Thread.Sleep(6000);
                        break;
                    }

                    StrengthMovement();
                    //CovertMovement();
                    break;
                case "RAIDEND":
                    if (isNewState)
                    {
                        SetLastRaidTimeSpan();
                        TarkovTrackers.HoldingGrenade = false; // we never have grenade on exit
                        TarkovTrackers.RaidsCompleted++;
                    }
                    ClickReturnToMainMenu();
                    break;
                
                case "CONFIRMMAINMENURETURN":
                    Thread.Sleep(500);
                    ConfirmReturnToMenu();
                    break;
                
                case "INVENTORY":
                    if (TarkovTrackers.NeedsGrenade && !TarkovTrackers.OutOfGrenades)
                    {
                        TryGetGrenade();
                        break;
                    }

                    BackupToMainMenu();
                    break;
                case "ESCAPEAFTERLEAVING":
                    ClickEscapeFromTarkovButton();
                    Thread.Sleep(5000);
                    ClickReconnect();
                    break;

            }
            
        }

        private static void SetLastRaidTimeSpan()
        {
            TarkovTrackers.LastRaidTotalTime = DateTime.Now - TarkovTrackers.CurrentRaidStartTime;
            Console.WriteLine($"Last Raid Time: {TarkovTrackers.LastRaidTotalTime:g}");
            if (!(TarkovTrackers.LastRaidTotalTime.TotalSeconds >= TarkovTrackers.RaidLengthGrenadeThreshold) ||
                TarkovTrackers.OutOfGrenades) return;
            TarkovTrackers.NeedsGrenade = true;
        }

        private static void SetCurrentRaidStartTime()
        {
            TarkovTrackers.CurrentRaidStartTime = DateTime.Now;
            Console.WriteLine($"New Raid Started at {TarkovTrackers.CurrentRaidStartTime}");
        }

        private static void ConfirmReturnToMenu()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["ConfrimReturnToMainMenu"]);
            ReturnCursorToRootLoc();
        }

        private static void ClickReturnToMainMenu()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["ReturnToMainMenuButtonPostRaid"]);
            ReturnCursorToRootLoc();
        }

        private static void StrengthMovement()
        {
            InputWrapper.QuickPress(InputWrapper.VK.SPACE);
            Thread.Sleep(100);
            InputWrapper.PressKeyFor(InputWrapper.VK.KEY_W,1000);
            Thread.Sleep(100);
            InputWrapper.QuickPress(InputWrapper.VK.SPACE);
            Thread.Sleep(100);
            InputWrapper.PressKeyFor(InputWrapper.VK.KEY_S,1000);
            Thread.Sleep(100);
        }
        private static void CovertMovement()
        {
            //TODO: Focus window if not in focus
            InputWrapper.QuickPress(InputWrapper.VK.KEY_C);
            Thread.Sleep(100);
            InputWrapper.PressKeyFor(InputWrapper.VK.KEY_W, 1000);
            Thread.Sleep(100);
            InputWrapper.PressKeyFor(InputWrapper.VK.KEY_D, 1000);
            Thread.Sleep(100);
            InputWrapper.PressKeyFor(InputWrapper.VK.KEY_S, 1000);
            Thread.Sleep(100);
            InputWrapper.PressKeyFor(InputWrapper.VK.KEY_A, 1000);
            Thread.Sleep(100);
            InputWrapper.QuickPress(InputWrapper.VK.KEY_C);
            Thread.Sleep(250);
        }

        private static void SelectFactoryMap()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["FactoryMapSelect"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500);
        }

        private static void StartRaidFromMapSelect()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["MapSelectReadyButton"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500);
        }
        private static void ClickReconnect()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["ReconnectButtonLocation"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500);
        }

        private static void SelectPmc()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["PMCSelectButton"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500);
        }

        private static void ClickNextAtPmcSelect()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["PMCSelectNextButton"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500);
        }

        private static void ClickEscapeFromTarkovButton()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["EscapeFromTarkovButton"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500);
        }
        private static void ClickInventoryButton()
        {
            InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["InventoryButtonLocation"]);
            ReturnCursorToRootLoc();
            Thread.Sleep(500); // need time to transition?
        }
        

        static void ReturnCursorToRootLoc()
        {
            Thread.Sleep(100);
            InputWrapper.SetCursorWindowRelative(TarkovVars.ConstantPosOffsets["GameAreaRoot"]);
            Thread.Sleep(100);
        }

        static void TryGetGrenade()
        {
            //todo: add scroll in stash

            if (MoveGrenadeOfType("F1Grenade") ||
                MoveGrenadeOfType("RGD5Grenade") ||
                MoveGrenadeOfType("VOG17Grenade") ||
                MoveGrenadeOfType("VOG25Grenade") ||
                MoveGrenadeOfType("M67Grenade"))
            {
                return;
                
            }
            
            TarkovTrackers.GrenadeFindRetries--;
            if (TarkovTrackers.GrenadeFindRetries > 0) return;
                
            TarkovTrackers.NeedsGrenade = false;
            TarkovTrackers.OutOfGrenades = true;
            Console.WriteLine("Out of nades");
        }

        private static bool MoveGrenadeOfType(string type)
        {
            if (!TarkovVars.GameObjects[type].FindObject(CvSearch.WindowRegion.RightHalf, out var pt))
                return false;
            InputWrapper.SetCursorWindowRelative(pt);
            InputWrapper.ClickAtCurrent(release: false);
            Thread.Sleep(100);
            InputWrapper.SetCursorWindowRelative(TarkovVars.ConstantPosOffsets["PocketOneLocation"]);
            Thread.Sleep(100);
            InputWrapper.ClickAtCurrent();
            Thread.Sleep(500);
            InputWrapper.SetCursorWindowRelative(TarkovVars.ConstantPosOffsets["GameAreaRoot"]);
            Thread.Sleep(1000);
            
            if (!TarkovVars.GameObjects[type].FindObject(CvSearch.WindowRegion.LeftHalf, out var pt2))
                return false;
            TarkovTrackers.NeedsGrenade = false;
            TarkovTrackers.HoldingGrenade = true;
            return true;

        }

        static void BackupToMainMenu()
        {
            for (int i = 0; i < 10; i++)
            {
                InputWrapper.QuickPress(InputWrapper.VK.ESCAPE);
                Thread.Sleep(50);
            }
            Thread.Sleep(1000); // So we don't try to grab another grenade or whatever
        }
        
        public static void AsyncCycle(GameState currentState,
            int currentCycle, bool isNewState)
        {
            
        }
        
    }
}