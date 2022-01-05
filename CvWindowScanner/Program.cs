using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using CvWindowScanner.GameVariables;
using CvWindowScanner.Modules;
using CvWindowScanner.Utils;
using OpenCvSharp;
using SharpDX;
using Point = OpenCvSharp.Point;


namespace CvWindowScanner
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            WindowScanner.Init("EscapeFromTarkov");
            TarkovVars.SetupStates();
            BotBase.Run(TarkovVars.GameStates);
            WindowScanner.Stop();
        }

    }
}