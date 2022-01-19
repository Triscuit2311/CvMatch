using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CvWindowScanner.GameVariables;

namespace CvWindowScanner.Modules
{
    public static class BotBase
    {
        public static List<GameState> GameStates;
        private static GameState _lastState = default;
        private static GameState _currentState = default;
        private static int _gameCycles = 0;

        public static void SetStates(List<GameState> gameStates)
        {
            GameStates = gameStates;
        }
        public static void Run( int cycleSleep = 100, int windowRefreshCycles = 100 )
        {
            while (!WindowScanner.Initialized)
            {
                Thread.Sleep(10); 
            }
            
            TarkovTrackers.BotStartTime = DateTime.Now;
            while (WindowScanner.Initialized)
            {
                if (BotControls.GlobalPause)
                {
                    Thread.Sleep(100);
                    continue;
                }
                TarkovTrackers.BotRunTime = DateTime.Now - TarkovTrackers.BotStartTime;
                
                _gameCycles++;

                if (_gameCycles % windowRefreshCycles == 0)
                {
                    WindowScanner.UpdateWindowPointer();
                    WindowScanner.UpdateWindow();
                }
                
                Thread.Sleep(cycleSleep);
                var flag = false;

                var priority = 0;
                
                if (GameStates is null) continue;
                foreach (var state in GameStates.Where(state => state.State))
                {
                    if (priority >= state.Priority) continue;
                    priority = state.Priority;
                    _currentState = state;
                    flag = true;
                }
                
                if(flag)
                    BotAction.StopCycle(_currentState, _gameCycles, _currentState != _lastState);
                if (!flag || _currentState == _lastState) continue;
                _lastState = _currentState;


                Console.WriteLine($"[{TarkovTrackers.BotRunTime.Days}:" +
                                  $"{TarkovTrackers.BotRunTime.Hours}:" +
                                  $"{TarkovTrackers.BotRunTime.Minutes}:" +
                                  $"{TarkovTrackers.BotRunTime.Seconds}] " +
                                  $"Raids: {TarkovTrackers.RaidsCompleted} | " +
                                  $"State: {_currentState.Name}");
            }
            
        }

        
    }
}