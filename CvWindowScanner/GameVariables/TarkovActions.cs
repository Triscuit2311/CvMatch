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
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["EscapeFromTarkovButton"]);
                    ReturnCursorToRootLoc();
                    break;
                case "PMCSELECTED":
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["PMCSelectNextButton"]);
                    ReturnCursorToRootLoc();
                    break;
                case "SCAVSELECTED":
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["PMCSelectButton"]);
                    ReturnCursorToRootLoc();
                    break;
                case "FACTORYSELECTED":
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["MapSelectReadyButton"]);
                    ReturnCursorToRootLoc();
                    break;
                case "FACTORYUNSELECTED":
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["FactoryMapSelect"]);
                    ReturnCursorToRootLoc();
                    break;
                case "INGAME":
                    InputWrapper.PressKeyFor(InputWrapper.VK.KEY_W,2000);
                    InputWrapper.PressKeyFor(InputWrapper.VK.KEY_S,2000);
                    Thread.Sleep(1000);
                    break;
                case "RAIDEND":
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["ReturnToMainMenuButtonPostRaid"]);
                    ReturnCursorToRootLoc();
                    break;
                case "CONFIRMMAINMENURETURN":
                    InputWrapper.ClickOnInWindow(TarkovVars.ConstantPosOffsets["ConfrimReturnToMainMenu"]);
                    ReturnCursorToRootLoc();
                    break;
                    
            }
            
            
            
        }


        static void ReturnCursorToRootLoc()
        {
            Thread.Sleep(100);
            InputWrapper.SetCursorWindowRelative(TarkovVars.ConstantPosOffsets["GameAreaRoot"]);
            Thread.Sleep(100);
        }
        
        public static void AsyncCycle(GameState currentState,
            int currentCycle, bool isNewState)
        {
            
        }
        
    }
}