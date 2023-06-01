using System;
using ChittaExorcist.Common.Interfaces;
using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class PoiseReceiver : CoreComponent, IStunnable
    {
        public event Action OnStun;
        
        public void Stun()
        {
            OnStun?.Invoke();
        }
    }
}