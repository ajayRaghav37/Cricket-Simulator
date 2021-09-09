using System;
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
    }
}