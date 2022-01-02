using System;
using System.Windows.Forms;

namespace CvWindowScanner.Utils
{

    public static class DebugScreenShotter
    {
        private static int _count;
        private static string _sessionId;
        
        public static void Setup()
        {
            _sessionId = $"[{DateTime.Now.ToShortDateString().Replace('/','.')} @ {DateTime.Now.ToShortTimeString().Replace(':','.')}]";
            HotKeyManager.RegisterHotKey(Keys.Insert, KeyModifiers.NoRepeat);
            HotKeyManager.HotKeyPressed += SaveLastScreen;
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