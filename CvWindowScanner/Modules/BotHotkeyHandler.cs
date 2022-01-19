using System;
using System.Windows.Forms;
using CvWindowScanner.GameVariables;
using CvWindowScanner.Utils;

namespace CvWindowScanner.Modules
{
    public static class BotHotkeyHandler
    {

        public static void Setup()
        {
            HotKeyManager.RegisterHotKey(Keys.End, KeyModifiers.NoRepeat);
            HotKeyManager.RegisterHotKey(Keys.Oemtilde, KeyModifiers.NoRepeat);
            HotKeyManager.HotKeyPressed += HotkeyHandler;
        }
        

        public static void HotkeyHandler(object sender, HotKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.End:
                    BotControls.ActionPause = !BotControls.ActionPause;
                    Console.WriteLine($"Bot Actions are now {(BotControls.ActionPause? "Paused":"Resumed")}.");
                    break;
                case Keys.Oemtilde:
                    break;
            }

        }
        
        
      
    }
}