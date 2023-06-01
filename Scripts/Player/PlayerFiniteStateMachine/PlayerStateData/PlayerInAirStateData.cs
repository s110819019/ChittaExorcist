using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.FSM
{
    [Serializable]
    public class PlayerInAirStateData : PlayerStateData
    {
        [field: SerializeField, Header("Movement")] public float AirMovementVelocity { get; private set; }

        [field: SerializeField, Header("CoyoteTime")] public bool UseCoyoteTime { get; private set; } = true;
        [field: SerializeField] public float CoyoteTime { get; private set; }
        
        [field: SerializeField, Header("Acceleration")] public bool UseAcceleration { get; private set; } = true;
        [field: SerializeField] public float AirAcceleration { get; private set; }
        [field: SerializeField] public float AirDeceleration { get; private set; }
        [field: SerializeField] public float AirTurnDeceleration { get; private set; }
        

        [field: SerializeField, Header("Fall Speed Curve")] public bool UseFallSpeedCurve { get; private set; } = true;
        [field: SerializeField] public AnimationCurve FallSpeedCurve { get; private set; }
        

        [field: SerializeField, Header("Jump Up Speed Curve")] public bool UseJumpUpSpeedCurve { get; private set; } = true;
        [field: SerializeField] public AnimationCurve JumpUpSpeedCurve { get; private set; }
        
        
        [field: SerializeField, Header("Variable Jump Height")] public bool UseVariableJumpHeight { get; private set; } = true;
        [field: SerializeField, Range(1.0f, 10.0f)] public float JumpCutoff { get; private set; }
        
        
        [field: SerializeField, Header("Jump Effect")] public float JumpParticleTime { get; private set; } = 0.1f;


        [field: SerializeField, TextArea, Header("Info Note")] public string InAirInfo { get; private set; }
    }
}