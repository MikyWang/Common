using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MilkSpun.Extentions
{
    public static class Extention
    {
        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var comp))
            {
                Object.DestroyImmediate(comp);
            }
        }

        public static T RefreshComponent<T>(this GameObject gameObject) where T : Component
        {
            gameObject.RemoveComponent<T>();
            var comp = gameObject.AddComponent<T>();
            return comp;
        }
    
        public static T GetComponentExt<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.TryGetComponent<T>(out var comp))
            {
                return comp;
            }
            return gameObject.AddComponent<T>();
        }
    
    }
}