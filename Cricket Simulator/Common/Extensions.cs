using System;
using System.Collections.Generic;
using System.Linq;


namespace Cricket_Simulator.Common
{
    public static class Extensions
    {
        public static bool In<T>(this T item, params T[] items)
        {
           

            if (items == null)
                throw new ArgumentNullException("items");

            

            return items.Contains(item);
        }

        public static bool NotIn<T>(this T item, params T[] items)
        {
            

            if (items == null)
                throw new ArgumentNullException("items");

            

            return !items.Contains(item);
        }

        public static bool IsBetween<T>(this T item, T start, T end)
        {
            

            return Comparer<T>.Default.Compare(item, start) >= 0
                && Comparer<T>.Default.Compare(item, end) <= 0;

            
        }

        public static string PadCenter(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(nameof(str));

            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);

            
        }
    }
}