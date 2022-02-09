using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace MilkSpun.Common
{
    public static class Utils
    {
        public static float Remap(float target, Vector2 oldRange, Vector2 newRange)
        {
            var dest = (target - oldRange.x) / (oldRange.y - oldRange.x) * (newRange.y - newRange.x) + newRange.x;
            return dest;
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

    }
}
