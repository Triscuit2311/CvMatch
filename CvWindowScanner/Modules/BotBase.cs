using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CvWindowScanner.Modules
{
    public static class BotBase
    {
        public static List<GameState> GameStates;
        private static GameState _lastState = default;
        private static GameState _currentState = default;
        private static int _gameCycles = 0;

        public static void Run(List<GameState> gameStates, int cycleSleep = 100, int windowRefreshCycles = 1000 )
        {
            GameStates = gameStates;
            
            while (!WindowScanner.Initialized)
            {
                Thread.Sleep(10); 
            }
            
            while (WindowScanner.Initialized)
            {
                _gameCycles++;
                if (_gameCycles % windowRefreshCycles == 0) WindowScanner.UpdateWindow();
                Thread.Sleep(cycleSleep);
                var flag = false;

                var priority = 0;
                foreach (var state in GameStates.Where(state => state.State))
                {
                    if (priority >= state.Priority) continue;
                    priority = state.Priority;
                    _currentState = state;
                    flag = true;
                }
                
                BotAction.StopCycle(_currentState, _gameCycles, flag && _currentState != _lastState);
                
                if (!flag || _currentState == _lastState) continue;
                _lastState = _currentState;

                Console.WriteLine($"[{_gameCycles}] State: {_currentState.Name}");
                
            }
            
        }

        
    }
}