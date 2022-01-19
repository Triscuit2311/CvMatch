using System;
using System.Collections.Generic;
using System.Threading;
using CvWindowScanner.Utils;

namespace CvWindowScanner.Modules
{
    public class BotControls
    {
        public static bool GlobalPause = false;
        public static bool ActionPause = false;


        public static void TargetBot(string title,
        Action<GameState,int,bool> StopCycleCB,
        Action<GameState,int,bool> AsyncCycleCB,
        int threadSleep,
        Action variableSetupCallback,
        Func<List<GameState>> gameStatesFunc)
        {
            Console.WriteLine($"Targeting {title}, standby.");
            GlobalPause = true;
            Thread.Sleep(5000);
            WindowScanner.Init(title);
            
            variableSetupCallback();
            BotAction.Init(StopCycleCB,AsyncCycleCB,threadSleep);

            BotHotkeyHandler.Setup();
            DeveloperHotkeys.Setup();
            
            BotBase.SetStates(gameStatesFunc());
            GlobalPause = false;
        }
    }
}