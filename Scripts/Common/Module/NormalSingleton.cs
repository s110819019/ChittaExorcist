using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChittaExorcist.Common.Module
{
    public class NormalSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            Instance = this as T;
        }
    }
}
