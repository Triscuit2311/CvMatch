using System;

namespace CvWindowScanner.Utils
{
    public static class Extensions
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            if(val.CompareTo(max) > 0) return max;
            return val;
        }
    }
}