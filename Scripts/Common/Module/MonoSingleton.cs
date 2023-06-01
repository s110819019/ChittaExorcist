using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChittaExorcist.Common.Module
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null && _applicationIsQuitting)
                {
                    return null;
                }
                else
                {
                    _applicationIsQuitting = false;
                }
                
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                if (_instance is not null) DontDestroyOnLoad(_instance.gameObject);
            }
            else
            {
                // Destroy(_instance.gameObject);
                Destroy(this.gameObject);
            }
        }
        
        private static bool _applicationIsQuitting;

        protected virtual void OnDestroy()
        {
            _applicationIsQuitting = true;
        }

        // [RuntimeInitializeOnLoadMethod]
        // static void RunOnStart()
        // {
        //     Application.quitting += () => applicationIsQuitting = true;
        // }
    }
}
