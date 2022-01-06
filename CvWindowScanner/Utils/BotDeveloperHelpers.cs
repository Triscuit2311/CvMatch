using System;
using System.Windows.Forms;
using CvWindowScanner.Modules;

namespace CvWindowScanner.Utils
{

    public static class BotDeveloperHelpers
    {
        private static int _count;
        private static string _sessionId;
        
        public static void Setup()
        {
            _sessionId = $"[{DateTime.Now.ToShortDateString().Replace('/','.')} @ {DateTime.Now.ToShortTimeString().Replace(':','.')}]";
            HotKeyManager.RegisterHotKey(Keys.Insert, KeyModifiers.NoRepeat);
            HotKeyManager.RegisterHotKey(Keys.Home, KeyModifiers.NoRepeat);
            HotKeyManager.HotKeyPressed += HotkeyHandler;
        }
        

        public static void HotkeyHandler(object sender, HotKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Insert:
                    SaveLastScreen();
                    break;
                case Keys.Home:
                    PrintCurPos();
                    break;
            }

        }

        public static void PrintCurPos()
        {
            var pt = InputWrapper.GetCursorPosition();
            Console.WriteLine($"Cursor Position:\n" +
                              $"\tScreen Space: ({pt.X}, {pt.Y})"+
                              $"\tWindow Space: ({pt.X-WindowScanner.WindowPosition.X}, {pt.Y-WindowScanner.WindowPosition.Y})"
            );
        }

        public static void SaveLastScreen()
        {
            if (CvSearch.LastFrame.Empty())
            {
                Console.WriteLine("No image to save.");
                return;
            }

            var file = $"screenshots\\{_sessionId}_{_count}.bmp";

            _count++;
            if (CvSearch.LastFrame.SaveImage(file))
            {
                Console.WriteLine(file);
                return;
            }
            Console.WriteLine(file);
        }


    }
}