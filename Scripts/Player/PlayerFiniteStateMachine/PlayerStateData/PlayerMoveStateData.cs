using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerMoveStateData : PlayerStateData
    {
        [field: SerializeField, Header("Movement")] public float MovementVelocity { get; private set; }
        
        [field: SerializeField, Header("Acceleration")] public bool UseAcceleration { get; private set; } = true;
        [field: SerializeField] public float Acceleration { get; private set; }
        [field: SerializeField] public float Deceleration { get; private set; }
        [field: SerializeField] public float TurnDeceleration { get; private set; }
        
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}