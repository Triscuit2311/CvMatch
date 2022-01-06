using System;
using System.ComponentModel;
using System.Threading;
using CvWindowScanner.Utils;
using OpenCvSharp;

namespace CvWindowScanner.Modules
{
    public static  class BotAction
    {
        private static Action<GameState,int,bool> _stopCycleCB;
        private static Action<GameState,int,bool> _threadCycleCB;

        private static int _threadSleep;
        private static Thread _scanThread;
        private static bool _threadStopFlag;

        private static GameState _currentState;
        private static int _currentCycle;
        private static bool _isNewState;
        
        

        public static void Init(Action<GameState,int,bool> stopCycleCallback, 
            Action<GameState,int,bool> threadCycleCallback, int threadSleep)
        {
            _stopCycleCB = stopCycleCallback;
            _threadCycleCB = threadCycleCallback;
            _threadSleep = threadSleep;
            
            _scanThread = new Thread(new ThreadStart(ThreadFunction));
            _scanThread.Start();
        }

        private static void ThreadFunction()
        {
            while (true)
            {
                if (_threadStopFlag) break;
                Thread.Sleep(_threadSleep);
                
                if (_currentState == null) continue;
                _threadCycleCB(_currentState, _currentCycle, _isNewState);
            }
        }
        
        public static void StopCycle(GameState currentState, int currentCycle, bool isNewState)
        {
            if (currentState is null) return;

            _currentState = currentState;
            _currentCycle = currentCycle;
            _isNewState = isNewState;
            
            _stopCycleCB(_currentState, _currentCycle, _isNewState);
        }
    }
}