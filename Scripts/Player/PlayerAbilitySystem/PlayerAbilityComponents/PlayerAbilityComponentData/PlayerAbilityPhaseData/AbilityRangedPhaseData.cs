using System;
using UnityEngine;

namespace ChittaExorcist.PlayerSettings.PlayerAbilitySystem
{
    [Serializable]
    public class AbilityRangedPhaseData : PlayerAbilityPhaseData
    {
        [field: SerializeField, Header("Basic")] public GameObject ProjectilePrefab { get; private set; }
        [field: SerializeField] public Vector2 StartPosition { get; private set; }
        [field: SerializeField] public Vector2 StartDirection { get; private set; }
        [field: SerializeField] public float TravelTime { get; private set; }
        [field: SerializeField] public AnimationCurve SpeedCurve { get; private set; }
        
        [field: SerializeField] public bool UseFallGravity { get; private set; }
        // [field: SerializeField] public bool ShouldFlip { get; private set; }

        [field: SerializeField, Header("Combat")] public float Damage { get; private set; }
        [field: SerializeField] public float PoiseDamage { get; private set; }
        [field: SerializeField] public float KnockbackStrength { get; private set; }
        [field: SerializeField] public Vector2 KnockbackDirection { get; private set; }
    }
}