using System;

namespace CvWindowScanner.Utils
{
    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if(val.CompareTo(max) > 0) return max;
            else return val;
        }
        
        public static int ClampInt(this int val, int min, int max)
        {
            if (val < min) return min;
            return val > max ? max : val;
        }
    }
}