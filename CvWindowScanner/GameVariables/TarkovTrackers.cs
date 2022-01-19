using System;

namespace CvWindowScanner.GameVariables
{
    public static class TarkovTrackers
    {
        // Info
        public static int RaidsCompleted = 0;
        public static DateTime BotStartTime;
        public static TimeSpan BotRunTime;
        
        // Grenade Management
        public static bool NeedsGrenade = true;
        public static bool HoldingGrenade = false;
        public static bool OutOfGrenades = false;
        public static int GrenadeFindRetries = 10;
        public static double RaidLengthGrenadeThreshold = 60;
        public static double TimeInRaidBeforeUseGrenade = 15;
        
        
        // Last Raid
        public static DateTime CurrentRaidStartTime;
        public static TimeSpan LastRaidTotalTime;
        
        // Queue Timer
        public static DateTime CurrentQueueStartTime;
        public static TimeSpan CurrentQueueTime;
        public static double QueueTimeoutThreshold = 60 * 10;

        // Menu Timer


        //TODO: Add auto-restart timer on long game load time.
    }
}