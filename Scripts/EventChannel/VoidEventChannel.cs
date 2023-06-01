using System;
using UnityEngine;

namespace ChittaExorcist.EventChannel
{
    [CreateAssetMenu(fileName = "NewVoidEventChannel", menuName = "Custom Data/Event Channel/Void Event Channel")]
    public class VoidEventChannel : ScriptableObject
    {
        private event Action Delegate;

        public void Broadcast()
        {
            Delegate?.Invoke();
        }

        public void AddListener(Action action)
        {
            Delegate += action;
        }

        public void RemoveListener(Action action)
        {
            Delegate -= action;
        }
    }
}