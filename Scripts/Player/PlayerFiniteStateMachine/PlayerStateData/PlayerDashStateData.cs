using System;
using ChittaExorcist.GameCore.AudioSettings;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerDashStateData : PlayerStateData
    {
        [field: SerializeField] public float DashDuration { get; private set; }
        [field: SerializeField] public float DashVelocity { get; private set; }
        [field: SerializeField] public float DashCooldown { get; private set; }
        [field: SerializeField] public AnimationCurve DashSpeedCurve { get; private set; }
        [field: SerializeField] public float InitialGravityScale { get; private set; }
        [field: SerializeField] public float DashDrag { get; private set; }
        // [field: SerializeField] public float MaxDashHoldTime { get; private set; }
        [field: SerializeField] public float DistanceBetweenAfterImages { get; private set; }
        [field: SerializeField] public AudioDataSO AudioData { get; private set; }
    }
}