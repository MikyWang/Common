using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MilkSpun.Common
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T Instance { get; protected set; }
        public static bool IsInitialized => Instance is not null;

        protected virtual void Awake()
        {
            if (IsInitialized)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
