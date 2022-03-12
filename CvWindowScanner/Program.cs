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
            BotControls.TargetBot(
                "EscapeFromTarkov",
                TarkovActions.StopCycle,
                TarkovActions.AsyncCycle,
                100,
                TarkovVars.Setup,
                TarkovVars.GetStates);
            
            //BotBase.SetStates(TarkovVars.GameStates);
            BotBase.Run();
            WindowScanner.Stop();
        }

    }
}