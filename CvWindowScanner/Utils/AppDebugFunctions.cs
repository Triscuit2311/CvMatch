using System;
using System.Windows.Forms;
using CvWindowScanner.Modules;

namespace CvWindowScanner.Utils
{

    public static class AppDebugFunctions
    {
        private static int _count;
        private static string _sessionId;
        
        public static void Setup()
        {
            _sessionId = $"[{DateTime.Now.ToShortDateString().Replace('/','.')} @ {DateTime.Now.ToShortTimeString().Replace(':','.')}]";
            HotKeyManager.RegisterHotKey(Keys.Insert, KeyModifiers.NoRepeat);
            //HotKeyManager.HotKeyPressed += SaveLastScreen;
            HotKeyManager.HotKeyPressed += SaveMouseOffset;

        }
        

        private static void SaveMouseOffset(object sender, HotKeyEventArgs e)
        {
            var pt = InputWrapper.GetCursorPosition();
            Console.WriteLine($"Cursor Position:\n" +
                              $"\tScreen Space: ({pt.X}, {pt.Y})"+
                              $"\tWindow Space: ({pt.X-WindowScanner.WindowPosition.X}, {pt.Y-WindowScanner.WindowPosition.Y})"
            );
        }

        private static void SaveLastScreen(object sender, HotKeyEventArgs hotKeyEventArgs)
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