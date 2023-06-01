using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChittaExorcist
{
    public class EventChannelWithOneParameterAndReturn<T1, T2> : ScriptableObject
    {
        private event Func<T1, T2> Delegate;

        [field: SerializeField] private List<String> Listeners { get; set; }

        public void Broadcast(T1 param)
        {
            Delegate?.Invoke(param);
        }

        // public T2 BroadcastWithReturn(T1 param)
        // {
        //     return Delegate.Invoke(param);
        // }

        public void AddListener(Func<T1, T2> func)
        {
            Delegate += func;
            Listeners.Add(func.Target.ToString());
        }

        public void RemoveListener(Func<T1, T2> func)
        {
            Delegate -= func;
            Listeners.Remove(func.Target.ToString());
        }
    }
}
