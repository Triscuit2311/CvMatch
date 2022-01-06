using System;
using System.Windows.Forms;
using CvWindowScanner.Utils;

namespace CvWindowScanner.Modules
{
    public static class BotHotkeyHandler
    {

        public static void Setup()
        {
                       HotKeyManager.RegisterHotKey(Keys.End, KeyModifiers.NoRepeat);
            HotKeyManager.HotKeyPressed += HotkeyHandler;
        }
        

        public static void HotkeyHandler(object sender, HotKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.End:
                    BotAction.Paused = !BotAction.Paused;
                    Console.WriteLine($"Bot Actions are now {(BotAction.Paused? "Paused":"Resumed")}.");
                    break;

            }

        }
        
        
      
    }
}