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
           
            TarkovVars.SetupStates();
            BotAction.Init(TarkovActions.StopCycle,TarkovActions.AsyncCycle,100);
            
            BotDeveloperHelpers.Setup();
            
            BotBase.Run(TarkovVars.GameStates); //main loop
            
            WindowScanner.Stop();
        }

    }
}