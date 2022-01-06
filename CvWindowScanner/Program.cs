using System;
using System.Threading;
using CvWindowScanner.GameVariables;
using CvWindowScanner.Modules;
using CvWindowScanner.Utils;


namespace CvWindowScanner
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            WindowScanner.Init("EscapeFromTarkov");
           
            TarkovVars.Setup();
            BotAction.Init(TarkovActions.StopCycle,TarkovActions.AsyncCycle,100);
            
            BotHotkeyHandler.Setup();
            DeveloperHotkeys.Setup();
            
            BotBase.Run(TarkovVars.GameStates); //main loop
            
            WindowScanner.Stop();
        }

    }
}