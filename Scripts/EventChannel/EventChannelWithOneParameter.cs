using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ChittaExorcist.EventChannel
{
    public abstract class EventChannelWithOneParameter<T> : ScriptableObject
    {
        private event Action<T> Delegate;

        [field: SerializeField] private List<String> Listeners { get; set; }

        public void Broadcast(T param)
        {
            Delegate?.Invoke(param);
        }

        public void AddListener(Action<T> action)
        {
            Delegate += action;
            Listeners.Add(action.Target.ToString());
        }

        public void RemoveListener(Action<T> action)
        {
            Delegate -= action;
            Listeners.Remove(action.Target.ToString());
        }
    }
}
